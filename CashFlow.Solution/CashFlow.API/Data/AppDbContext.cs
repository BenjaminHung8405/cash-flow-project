using CashFlow.API.Domain.Entities;
using CashFlow.API.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Text.Json;

namespace CashFlow.API.Data
{
    /// <summary>
    /// Entity Framework Core DbContext for the multi-tenant financial application.
    /// Implements multi-tenancy with shared database and schema, global query filters,
    /// and automatic audit trail generation.
    /// </summary>
    public class AppDbContext : DbContext
    {
        private readonly ITenantService _tenantService;

        public AppDbContext(DbContextOptions<AppDbContext> options, ITenantService tenantService)
            : base(options)
        {
            _tenantService = tenantService;
        }

        #region DbSets
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Fund> Funds { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Get the current tenant ID for query filters
            var currentTenantId = GetCurrentTenantIdSafe();

            #region Entity Configuration
            // Configure RolePermission composite key
            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionCode });

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionCode)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure relationships
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Fund)
                .WithMany(f => f.Transactions)
                .HasForeignKey(t => t.FundId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region Multi-Tenancy Global Query Filters
            // Automatically filter all queries by current tenant
            modelBuilder.Entity<User>()
                .HasQueryFilter(x => x.TenantId == currentTenantId);

            modelBuilder.Entity<Role>()
                .HasQueryFilter(x => x.TenantId == currentTenantId);

            modelBuilder.Entity<Fund>()
                .HasQueryFilter(x => x.TenantId == currentTenantId);

            modelBuilder.Entity<Transaction>()
                .HasQueryFilter(x => x.TenantId == currentTenantId);

            modelBuilder.Entity<AuditLog>()
                .HasQueryFilter(x => x.TenantId == currentTenantId);
            #endregion

            #region Seed Data
            // Seed default permissions
            modelBuilder.Entity<Permission>().HasData(
                new Permission { Code = "VOUCHER_CREATE", Name = "Lập phiếu thu/chi" },
                new Permission { Code = "VOUCHER_VIEW", Name = "Xem phiếu thu/chi" },
                new Permission { Code = "REPORT_VIEW", Name = "Xem báo cáo" },
                new Permission { Code = "FUND_MANAGE", Name = "Quản lý quỹ" },
                new Permission { Code = "USER_MANAGE", Name = "Quản lý người dùng" },
                new Permission { Code = "ROLE_MANAGE", Name = "Quản lý vai trò" }
            );
            #endregion
        }

        /// <summary>
        /// Safely retrieves the current tenant ID for query filters.
        /// Returns a default GUID if tenant service is unavailable (useful for migrations).
        /// </summary>
        private Guid GetCurrentTenantIdSafe()
        {
            try
            {
                return _tenantService.GetCurrentTenantId();
            }
            catch
            {
                // Return a default GUID that won't match any real tenant
                // This is useful for migrations or when called outside HTTP context
                return Guid.Empty;
            }
        }

        /// <summary>
        /// Override SaveChanges to automatically inject TenantId and generate audit logs.
        /// </summary>
        public override int SaveChanges()
        {
            ProcessChangeTracker();
            return base.SaveChanges();
        }

        /// <summary>
        /// Override SaveChangesAsync to automatically inject TenantId and generate audit logs.
        /// </summary>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ProcessChangeTracker();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void ProcessChangeTracker()
        {
            var currentTenantId = GetCurrentTenantIdSafe();
            var currentUserId = GetCurrentUserIdSafe();
            var now = DateTime.UtcNow;

            var auditLogs = new List<AuditLog>();

            foreach (var entry in ChangeTracker.Entries())
            {
                // Only process BaseTenantEntity descendants
                if (entry.Entity is BaseTenantEntity tenantEntity)
                {
                    // Inject TenantId for new entities
                    if (entry.State == EntityState.Added)
                    {
                        tenantEntity.TenantId = currentTenantId;
                        tenantEntity.CreatedAt = now;
                        tenantEntity.CreatedBy = currentUserId ?? "System";
                    }
                }

                // Generate audit log for all changes
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
                {
                    var auditLog = CreateAuditLog(entry, currentTenantId, currentUserId, now);
                    if (auditLog != null)
                    {
                        auditLogs.Add(auditLog);
                    }
                }
            }

            // Add audit logs without triggering SaveChanges again
            if (auditLogs.Any())
            {
                AuditLogs.AddRange(auditLogs);
                foreach (var log in auditLogs)
                {
                    Entry(log).State = EntityState.Added;
                }
            }
        }

        /// <summary>
        /// Creates an audit log entry for a changed entity.
        /// </summary>
        private AuditLog CreateAuditLog(EntityEntry entry, Guid tenantId, string userId, DateTime timestamp)
        {
            var entityType = entry.Entity.GetType();
            var tableName = entityType.Name;

            // Get the primary key value
            var primaryKey = entry.Metadata.FindPrimaryKey();
            string recordId = "Unknown";

            if (primaryKey != null)
            {
                var keyProperties = primaryKey.Properties;
                if (keyProperties.Count == 1)
                {
                    recordId = entry.CurrentValues[keyProperties[0].Name]?.ToString() ?? "Unknown";
                }
            }

            // Determine action
            string action = entry.State switch
            {
                EntityState.Added => "INSERT",
                EntityState.Modified => "UPDATE",
                EntityState.Deleted => "DELETE",
                _ => "UNKNOWN"
            };

            // Skip audit log creation if the entity is already an AuditLog to prevent recursion
            if (tableName == nameof(AuditLog))
            {
                return null;
            }

            // Get old and new values
            string oldValues = entry.State == EntityState.Deleted || entry.State == EntityState.Modified
                ? SerializeValues(entry.OriginalValues)
                : null;

            string newValues = entry.State == EntityState.Added || entry.State == EntityState.Modified
                ? SerializeValues(entry.CurrentValues)
                : null;

            // Create and return the audit log
            return new AuditLog
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                UserId = userId ?? "System",
                Action = action,
                TableName = tableName,
                RecordId = recordId,
                OldValues = oldValues,
                NewValues = newValues,
                Timestamp = timestamp
            };
        }

        /// <summary>
        /// Serializes entity values to JSON format for audit trail storage.
        /// </summary>
        private string SerializeValues(PropertyValues values)
        {
            var dictionary = new Dictionary<string, object>();

            foreach (var property in values.Properties)
            {
                var value = values[property];

                // Skip null values and properties that aren't scalar types
                if (value == null || property.PropertyInfo == null)
                {
                    continue;
                }

                dictionary[property.Name] = value;
            }

            return JsonSerializer.Serialize(dictionary, new JsonSerializerOptions
            {
                WriteIndented = false,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });
        }

        /// <summary>
        /// Safely retrieves the current user ID.
        /// </summary>
        private string GetCurrentUserIdSafe()
        {
            try
            {
                return _tenantService.GetCurrentUserId();
            }
            catch
            {
                return "System";
            }
        }
    }
}

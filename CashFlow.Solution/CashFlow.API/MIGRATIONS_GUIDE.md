# Database Migrations Guide

This guide explains how to create and manage Entity Framework Core migrations for the multi-tenant financial application.

## Prerequisites

Ensure you have the EF Core tools installed:

```powershell
dotnet tool install --global dotnet-ef
```

Or update if already installed:

```powershell
dotnet tool update --global dotnet-ef
```

## Initial Setup

### 1. Create Initial Migration

Navigate to the solution root and create the initial migration:

```powershell
cd C:\Users\ACER\source\repos\cash-flow-project\CashFlow.Solution

dotnet ef migrations add InitialCreate -p CashFlow.API
```

This will create a `Migrations` folder with the initial schema.

### 2. Apply Migration to Database

```powershell
dotnet ef database update -p CashFlow.API
```

This creates the database and all tables defined in the migration.

## Verification

### Check Migration Status

```powershell
dotnet ef migrations list -p CashFlow.API
```

### View Generated SQL

```powershell
dotnet ef migrations script -p CashFlow.API
```

To see SQL for a specific migration range:

```powershell
dotnet ef migrations script InitialCreate -p CashFlow.API
```

## Making Schema Changes

### Add a New Column to User

1. Update the `User` entity:

```csharp
public class User : BaseTenantEntity
{
    // ... existing properties ...
    
    [MaxLength(20)]
    public string PhoneNumber { get; set; }  // ← New property
}
```

2. Create a migration:

```powershell
dotnet ef migrations add AddPhoneNumberToUser -p CashFlow.API
```

3. Review the generated migration file in `Migrations/` folder

4. Apply the migration:

```powershell
dotnet ef database update -p CashFlow.API
```

### Add a New Entity

1. Create the entity class (example: `Department`):

```csharp
public class Department : BaseTenantEntity
{
    [Required]
    [MaxLength(100)]
    public string DepartmentName { get; set; }

    [MaxLength(255)]
    public string Description { get; set; }

    public bool IsActive { get; set; } = true;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
```

2. Add DbSet to `AppDbContext`:

```csharp
public DbSet<Department> Departments { get; set; }
```

3. Configure in `OnModelCreating`:

```csharp
modelBuilder.Entity<Department>()
    .HasQueryFilter(x => x.TenantId == currentTenantId);
```

4. Create migration:

```powershell
dotnet ef migrations add AddDepartmentEntity -p CashFlow.API
```

5. Apply migration:

```powershell
dotnet ef database update -p CashFlow.API
```

## Common Migration Scenarios

### Scenario 1: Add Foreign Key Relationship

**Before:**
```csharp
public class User : BaseTenantEntity
{
    public string Username { get; set; }
    public Guid RoleId { get; set; }
    public virtual Role Role { get; set; }
}
```

**After:**
```csharp
public class User : BaseTenantEntity
{
    public string Username { get; set; }
    public Guid RoleId { get; set; }
    public virtual Role Role { get; set; }
    
    public Guid? DepartmentId { get; set; }  // ← New FK
    public virtual Department Department { get; set; }
}
```

**Steps:**
```powershell
# Create migration
dotnet ef migrations add AddDepartmentFkToUser -p CashFlow.API

# Apply
dotnet ef database update -p CashFlow.API
```

### Scenario 2: Change Column Type or Constraints

Update the property:

```csharp
[Column(TypeName = "decimal(18,4)")]  // Changed from (18,2)
public decimal Amount { get; set; }
```

Create and apply migration:

```powershell
dotnet ef migrations add ChangeAmountPrecision -p CashFlow.API
dotnet ef database update -p CashFlow.API
```

### Scenario 3: Rename Column

1. Create a new property with the desired name
2. Create migration
3. In the migration file, use `RenameColumn` instead of drop/create
4. Apply the migration

**Migration file example:**
```csharp
protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.RenameColumn(
        name: "CreateAt",
        table: "Users",
        newName: "CreatedAt");
}
```

### Scenario 4: Drop a Column

Update the entity to remove the property, then:

```powershell
dotnet ef migrations add RemoveUnusedColumn -p CashFlow.API
dotnet ef database update -p CashFlow.API
```

## Troubleshooting

### Issue: "Unable to create a DbContext"

**Solution**: Ensure your `appsettings.json` is configured and the project builds successfully.

```bash
dotnet build CashFlow.API
```

### Issue: "There are pending model changes"

**Solution**: Create a migration:

```powershell
dotnet ef migrations add DescribeYourChanges -p CashFlow.API
```

### Issue: "The model snapshot is out of date"

**Solution**: Verify the entity changes are reflected in the migration, then rebuild:

```powershell
dotnet clean CashFlow.API
dotnet build CashFlow.API
```

### Issue: Migration failed to apply

**Steps to recover:**

1. Check if the migration was partially applied:
   ```sql
   SELECT * FROM __EFMigrationsHistory
   ```

2. If there's a failed migration, you can remove it:
   ```powershell
   dotnet ef migrations remove -p CashFlow.API
   ```

3. Fix the issue in your entity or migration file

4. Recreate and reapply:
   ```powershell
   dotnet ef migrations add YourMigration -p CashFlow.API
   dotnet ef database update -p CashFlow.API
   ```

## Best Practices

### 1. Descriptive Migration Names

Good:
```
dotnet ef migrations add AddDepartmentEntity
dotnet ef migrations add AddPhoneNumberToUser
dotnet ef migrations add ChangeAmountPrecisionInTransaction
```

Bad:
```
dotnet ef migrations add Update1
dotnet ef migrations add Fix
```

### 2. Review Generated Migrations

Always review the generated migration file before applying:

```powershell
# Generated migrations are in: CashFlow.API/Migrations/
# Look for: [Timestamp]_[MigrationName].cs
```

### 3. One Logical Change Per Migration

Create a migration for each logical change, not multiple unrelated changes.

### 4. Test Migrations Locally

Always test migrations locally before deploying to production.

### 5. Keep Migrations Clean

Avoid overly complex migrations. If needed, split into multiple migrations.

## Production Considerations

### Avoid: Auto-migrations in Production

**Don't do this:**
```csharp
dbContext.Database.EnsureCreatedAsync(); // Only for development
```

**Instead:**
```powershell
# Manually apply migrations in production
dotnet ef database update -p CashFlow.API
```

### Backup Before Migration

Always backup your production database before applying migrations:

```powershell
# SQL Server backup example
BACKUP DATABASE [CashFlowDB] TO DISK = 'C:\backup\CashFlowDB.bak'
```

### Script Migrations for Deployment

Generate a SQL script for manual review:

```powershell
dotnet ef migrations script -p CashFlow.API -o "migrations.sql"
```

This creates `migrations.sql` that can be reviewed before execution.

## Seed Data

After creating migrations, you may want to seed initial data:

```csharp
// In AppDbContext.OnModelCreating()
modelBuilder.Entity<Permission>().HasData(
    new Permission { Code = "VOUCHER_CREATE", Name = "Create Vouchers" },
    new Permission { Code = "REPORT_VIEW", Name = "View Reports" }
);
```

Then create a migration:

```powershell
dotnet ef migrations add SeedPermissions -p CashFlow.API
dotnet ef database update -p CashFlow.API
```

## Useful Commands Reference

```powershell
# List all migrations
dotnet ef migrations list -p CashFlow.API

# Create a new migration
dotnet ef migrations add MigrationName -p CashFlow.API

# Remove the last migration (not yet applied to database)
dotnet ef migrations remove -p CashFlow.API

# Update database to specific migration
dotnet ef database update MigrationName -p CashFlow.API

# Revert to previous migration
dotnet ef database update PreviousMigrationName -p CashFlow.API

# Revert all migrations
dotnet ef database update 0 -p CashFlow.API

# Generate SQL script
dotnet ef migrations script -p CashFlow.API -o migrations.sql

# Delete and recreate database
dotnet ef database drop -p CashFlow.API
dotnet ef database update -p CashFlow.API
```

## Related Documentation

- [Entity Framework Core Migrations](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
- [MULTI_TENANCY_ARCHITECTURE.md](./MULTI_TENANCY_ARCHITECTURE.md)
- [QUICKSTART.md](./QUICKSTART.md)

---

**Migration management is critical for database evolution!** 🗄️

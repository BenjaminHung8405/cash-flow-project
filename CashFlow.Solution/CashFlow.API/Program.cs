using CashFlow.API.Data;
using CashFlow.API.Infrastructure.Extensions;
using CashFlow.API.Infrastructure.Services;
using CashFlow.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
// DEPENDENCY INJECTION & CONFIGURATION
// ============================================================

// Multi-tenancy configuration
builder.Services.AddMultiTenancy();
builder.Services.AddMultiTenantDbContext(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCorsConfiguration();

// Core API services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<DatabaseService>();

// ============================================================
// APPLICATION PIPELINE
// ============================================================

var app = builder.Build();

// Apply database migrations automatically (development only)
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
    }

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
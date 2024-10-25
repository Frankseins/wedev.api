using Microsoft.EntityFrameworkCore;
using wedev.Domain;
namespace wedev.Infrastructure;

public class GlobalDbContext : DbContext
{
    public GlobalDbContext(DbContextOptions<GlobalDbContext> options)
        : base(options) { }
    
    public GlobalDbContext() { }
    public DbSet<Server> Servers { get; set; }
    public DbSet<Database> Databases { get; set; }
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<Mandant> Mandanten { get; set; }
    public DbSet<DeploymentEnvironment> DeploymentEnvironments { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Fluent API für Default-Werte und Konfigurationen
        modelBuilder.Entity<Server>()
            .Property(s => s.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<Database>()
            .Property(d => d.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<Tenant>()
            .Property(t => t.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<Mandant>()
            .Property(m => m.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        modelBuilder.Entity<DeploymentEnvironment>()
            .HasKey(e => e.EnvironmentId);
        
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=wedev-db.postgres.database.azure.com;Port=5432;Database=db_main;Username=postgres;Password=!1wedev-db2024");
        }
    }
    
}
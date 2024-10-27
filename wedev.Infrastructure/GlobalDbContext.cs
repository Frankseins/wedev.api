using Microsoft.EntityFrameworkCore;
using wedev.Domain;
using wedev.Domain.Global;

namespace wedev.Infrastructure;

public class GlobalDbContext : DbContext
{
    public GlobalDbContext(DbContextOptions<GlobalDbContext> options)
        : base(options) { }
    
    public GlobalDbContext() { }
    
    public DbSet<Server> Servers { get; set; }
    public DbSet<Database> Databases { get; set; }
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<App> Apps { get; set; }
    public DbSet<DeploymentEnvironment> DeploymentEnvironments { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    // New DbSet properties for the additional entities
    public DbSet<User> Users { get; set; }
    public DbSet<UserGroup> UserGroups { get; set; }
    public DbSet<UserTenant> UserTenants { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<GroupRole> GroupRoles { get; set; }
    public DbSet<Group> Groups { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Set default values for timestamps
        modelBuilder.Entity<Server>().Property(s => s.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<Database>().Property(d => d.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<Tenant>().Property(t => t.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<App>().Property(a => a.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<DeploymentEnvironment>().HasKey(e => e.EnvironmentId);

        modelBuilder.Entity<User>().Property(u => u.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<User>().Property(u => u.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<Role>().Property(r => r.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<Role>().Property(r => r.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<Group>().Property(g => g.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<Group>().Property(g => g.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Define composite keys and relationships

        // UserTenant composite key
        modelBuilder.Entity<UserTenant>()
            .HasKey(ut => new { ut.UserId, ut.TenantId });

        // UserGroup composite key and foreign key configuration
        modelBuilder.Entity<UserGroup>()
            .HasKey(ug => new { ug.UserId, ug.TenantId, ug.GroupId });

        modelBuilder.Entity<UserGroup>()
            .HasOne(ug => ug.UserTenant)
            .WithMany(ut => ut.UserGroups)
            .HasForeignKey(ug => new { ug.UserId, ug.TenantId });

        modelBuilder.Entity<UserGroup>()
            .HasOne(ug => ug.Group)
            .WithMany(g => g.UserGroups)
            .HasForeignKey(ug => ug.GroupId);

        // GroupRole composite key and foreign key configuration
        modelBuilder.Entity<GroupRole>()
            .HasKey(gr => new { gr.GroupId, gr.RoleId });

        modelBuilder.Entity<GroupRole>()
            .HasOne(gr => gr.Group)
            .WithMany(g => g.GroupRoles)
            .HasForeignKey(gr => gr.GroupId);

        modelBuilder.Entity<GroupRole>()
            .HasOne(gr => gr.Role)
            .WithMany(r => r.GroupRoles)
            .HasForeignKey(gr => gr.RoleId);

        // Configure relationships for UserTenant
        modelBuilder.Entity<UserTenant>()
            .HasOne(ut => ut.User)
            .WithMany(u => u.UserTenants)
            .HasForeignKey(ut => ut.UserId);

        modelBuilder.Entity<UserTenant>()
            .HasOne(ut => ut.Tenant)
            .WithMany(t => t.UserTenants)
            .HasForeignKey(ut => ut.TenantId);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=wedev-db.postgres.database.azure.com;Port=5432;Database=db_cloud_config;Username=postgres;Password=!1wedev-db2024");
        }
    }

}


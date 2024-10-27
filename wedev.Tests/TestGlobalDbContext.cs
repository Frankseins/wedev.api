using Microsoft.EntityFrameworkCore;
using wedev.Domain.Global;
using wedev.Infrastructure;

public class TestGlobalDbContext : GlobalDbContext
{
    public TestGlobalDbContext(DbContextOptions<GlobalDbContext> options) : base(options) { }

    public DbSet<App> Apps { get; set; }
    public  DbSet<Database> Databases { get; set; }
    public  DbSet<Tenant> Tenants { get; set; }
    public  DbSet<User> Users { get; set; }
    public  DbSet<Group> Groups { get; set; }
    public  DbSet<Role> Roles { get; set; }
}
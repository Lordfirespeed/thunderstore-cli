using Microsoft.EntityFrameworkCore;

namespace StreamBigJson;

public class PackageIndexContext : DbContext
{
    public DbSet<Package> Packages { get; set; }
    public DbSet<PackageVersion> PackageVersions { get; set; }

    public string DbPath { get; }

    public PackageIndexContext()
    {
        DbPath = "./index.db";
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}

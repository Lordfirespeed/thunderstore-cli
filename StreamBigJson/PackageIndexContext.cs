using Microsoft.EntityFrameworkCore;

namespace StreamBigJson;

public class PackageIndexContext : DbContext
{
    public DbSet<Package> Packages { get; set; }
    public DbSet<PackageVersion> PackageVersions { get; set; }
    public DbSet<PackageCategory> PackageCategories { get; set; }
    public DbSet<PackageDependency> PackageDependencies { get; set; }

    public required string DbPath { get; init; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}

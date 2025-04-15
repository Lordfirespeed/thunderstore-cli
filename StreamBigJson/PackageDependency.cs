using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace StreamBigJson;

[PrimaryKey(nameof(PackageVersionUuid), nameof(DependencyNamespace), nameof(DependencyName))]
[Index(nameof(PackageVersionUuid))]
public class PackageDependency
{
    public required Guid PackageVersionUuid { get; init; }

    public PackageDependency() { }

    [SetsRequiredMembers]
    public PackageDependency(Guid packageVersionUuid, string moniker)
    {
        PackageVersionUuid = packageVersionUuid;
        var parts = moniker.Split("-");
        if (parts.Length < 3)
            throw new ArgumentException($"moniker should contain at least 3 parts, got {moniker}", nameof(moniker));
        DependencyNamespace = string.Join("-", parts[..^2]);
        DependencyName = parts[^2];
        DependencyVersionString = parts[^1];
    }

    [MaxLength(128)]
    public required string DependencyNamespace { get; init; }

    [MaxLength(128)]
    public required string DependencyName { get; init; }

    [MaxLength(32)]
    public required string DependencyVersionString { get; init; }

    [NotMapped]
    public string Moniker => $"{DependencyNamespace}-{DependencyName}-{DependencyVersionString}";
}

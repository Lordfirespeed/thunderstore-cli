using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace StreamBigJson;

[PrimaryKey(nameof(PackageVersionUuid))]
public class PackageVersion
{
    [JsonPropertyName("uuid4")]
    [JsonRequired]
    public required Guid PackageVersionUuid { get; init; }

    #region version number handling
    [JsonPropertyName("version_number")]
    [JsonRequired]
    [MaxLength(32)]
    public string VersionNumberString
    {
        get => VersionNumber.ToString();
        init
        {
            try
            {
                VersionNumber = new Version(value);
            }
            catch (Exception ex) when (false
                || ex is ArgumentException
                || ex is ArgumentNullException
                || ex is ArgumentOutOfRangeException
                || ex is FormatException
                || ex is OverflowException
            )
            { }
        }
    }

    [JsonIgnore]
    [NotMapped]
    public Version VersionNumber { get; init; } = null!;
    #endregion

    [JsonPropertyName("description")]
    [JsonRequired]
    [MaxLength(1024)]
    public required string Description { get; init; }

    [JsonPropertyName("icon")]
    [JsonRequired]
    [MaxLength(512)]
    public required Uri IconUrl { get; init; }

    [JsonPropertyName("download_url")]
    [JsonRequired]
    [MaxLength(512)]
    public required Uri DownloadUrl { get; init; }

    [JsonPropertyName("website_url")]
    [MaxLength(512)]
    public Uri? WebsiteUrl { get; init; }

    [JsonPropertyName("downloads")]
    [JsonRequired]
    public required long DownloadCount { get; init; }

    [JsonPropertyName("date_created")]
    [JsonRequired]
    public required DateTime CreatedAt { get; init; }

    [JsonPropertyName("is_active")]
    [JsonRequired]
    public required bool IsActive { get; init; }

    [JsonPropertyName("file_size")]
    [JsonRequired]
    public required long FileSize { get; init; }

    #region dependencies handling
    [JsonPropertyName("dependencies")]
    [JsonRequired]
    [NotMapped]
    public required List<string> DependencyMonikers
    {
        get => Dependencies.Select(dependency => dependency.Moniker).ToList();
        init
        {
            Dependencies = value
                .Select(moniker => new PackageDependency(PackageVersionUuid, moniker))
                .ToList();
        }
    }

    public List<PackageDependency> Dependencies { get; init; } = null!;

    #endregion

    [JsonIgnore]
    public Guid? PackageUuid { get; set; }
    [JsonIgnore]
    [ForeignKey(nameof(PackageUuid))]
    public Package? Package { get; set; }
}

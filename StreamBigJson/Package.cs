using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace StreamBigJson;

[PrimaryKey(nameof(PackageUuid))]
[Index(nameof(Namespace))]
[Index(nameof(Name))]
[Index(nameof(Namespace), nameof(Name), IsUnique = true)]
public class Package
{
    [JsonPropertyName("uuid4")]
    [JsonRequired]
    public required Guid PackageUuid { get; init; }

    [JsonPropertyName("name")]
    [JsonRequired]
    [MaxLength(128)]
    public required string Name { get; init; }

    [JsonPropertyName("owner")]
    [JsonRequired]
    [MaxLength(128)]
    public required string Namespace { get; init; }

    [JsonPropertyName("package_url")]
    [JsonRequired]
    [MaxLength(512)]
    public required Uri PackageUrl { get; init; }

    [JsonPropertyName("donation_link")]
    [MaxLength(512)]
    public Uri? DonationUrl { get; init; }

    [JsonPropertyName("date_created")]
    [JsonRequired]
    public required DateTime CreatedAt { get; init; }

    [JsonPropertyName("date_updated")]
    [JsonRequired]
    public required DateTime UpdatedAt { get; init; }

    [JsonPropertyName("rating_score")]
    [JsonRequired]
    public required long RatingScore { get; init; }

    [JsonPropertyName("is_pinned")]
    [JsonRequired]
    public required bool IsPinned { get; init; }

    [JsonPropertyName("is_deprecated")]
    [JsonRequired]
    public required bool IsDeprecated { get; init; }

    [JsonPropertyName("has_nsfw_content")]
    [JsonRequired]
    public required bool HasNsfwContent { get; init; }

    #region categories handling
    [JsonPropertyName("categories")]
    [JsonRequired]
    [NotMapped]
    public required List<string> CategoryLabels
    {
        get => Categories.Select(category => category.CategoryLabel).ToList();
        init
        {
            Categories = value
                .Select(label => new PackageCategory
                {
                    PackageUuid = PackageUuid,
                    CategoryLabel = label,
                })
                .ToList();
        }
    }

    [JsonIgnore]
    public List<PackageCategory> Categories { get; init; } = null!;
    #endregion

    [JsonPropertyName("versions")]
    [JsonRequired]
    public required List<PackageVersion> Versions { get; init; }
}

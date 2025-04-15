using System.ComponentModel.DataAnnotations;
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

    [JsonPropertyName("versions")]
    [JsonRequired]
    public required List<PackageVersion> Versions { get; init; }
}

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace StreamBigJson;

[PrimaryKey(nameof(PackageUuid), nameof(CategoryLabel))]
[Index(nameof(PackageUuid))]
[Index(nameof(CategoryLabel))]
public class PackageCategory
{
    public required Guid PackageUuid { get; init; }

    [MaxLength(128)]
    public required string CategoryLabel { get; init; }
}

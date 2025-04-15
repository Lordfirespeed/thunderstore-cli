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

    [JsonIgnore]
    public Guid? PackageUuid { get; set; }
    [JsonIgnore]
    [ForeignKey(nameof(PackageUuid))]
    public Package? Package { get; set; }
}

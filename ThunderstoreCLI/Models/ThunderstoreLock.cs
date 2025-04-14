using Tomlet;
using Tomlet.Attributes;
using Tomlet.Models;

namespace ThunderstoreCLI.Models;

public class ThunderstoreLock : BaseToml<ThunderstoreLock>
{
    const string ClassLockfileVersionString = "0.1.0";

    [TomlProperty("lockfile-version")]
    public string LockfileVersionString
    {
        get => ClassLockfileVersionString;
        set
        {
            if (value == ClassLockfileVersionString)
                return;
            throw new InvalidOperationException("Lockfile version mismatch");
        }
    }

    [TomlProperty("packages")]
    public Package[]? Packages { get; set; }

    static ThunderstoreLock()
    {
        TomletMain.RegisterMapper<IPackageSource>(
            TomletMain.ValueFrom,
            toml =>
            {
                if (toml is not TomlTable tomlTable)
                    throw new NotSupportedException();
                if (tomlTable.ContainsKey("registry"))
                    return TomletMain.To<RegistryPackageSource>(tomlTable);
                if (tomlTable.ContainsKey("virtual"))
                    return TomletMain.To<VirtualPackageSource>(tomlTable);
                throw new NotSupportedException();
            }
        );
    }

    public interface IPackageSource;

    public class RegistryPackageSource : IPackageSource
    {
        [TomlProperty("registry")]
        public string? RegistryUrlString { get; set; }
    }

    public class VirtualPackageSource : IPackageSource
    {
        [TomlProperty("virtual")]
        public string? SourcePathString { get; set; }
    }

    public class Dependency
    {
        [TomlProperty("namespace")]
        public string? Namespace { get; set; }

        [TomlProperty("name")]
        public string? Name { get; set; }
    }

    public class Requirement : Dependency
    {
        [TomlProperty("specifier")]
        public string? Specifier { get; set; }
    }

    public class Package
    {
        [TomlProperty("namespace")]
        public string? Namespace { get; set; }

        [TomlProperty("name")]
        public string? Name { get; set; }

        [TomlProperty("version")]
        public string? VersionString { get; set; }

        [TomlProperty("source")]
        public IPackageSource? Source { get; set; }

        [TomlProperty("dependencies")]
        public Dependency[]? Dependencies { get; set; }

        [TomlProperty("dependency-groups")]
        public Dictionary<string, Dependency[]>? DependencyGroups { get; set; }

        [TomlProperty("metadata")]
        public PackageMetadata? Metadata { get; set; }
    }

    public class PackageMetadata
    {
        [TomlProperty("dependency-requirements")]
        public Requirement[]? Requirements { get; set; }

        [TomlProperty("dependency-group-requirements")]
        public Dictionary<string, Requirement[]>? RequirementGroups { get; set; }
    }
}

using ThunderstoreCLI.Configuration;
using Tomlet;
using Tomlet.Attributes;
using Tomlet.Models;

namespace ThunderstoreCLI.Models;

[TomlDoNotInlineObject]
public class ThunderstoreProject : BaseToml<ThunderstoreProject>
{
    public abstract class ProjectDataBase
    {
        [TomlProperty("namespace")]
        public string? Namespace { get; set; }

        [TomlProperty("name")]
        public string? Name { get; set; }

        [TomlProperty("version")]
        public string? VersionNumber { get; set; }

        [TomlProperty("description")]
        public string? Description { get; set; }

        [TomlProperty("repository-url")]
        public string? RepositoryUrl { get; set; }

        [TomlProperty("url")]
        public string? ProjectUrl { get; set; }

        [TomlProperty("contains-nsfw-content")]
        public bool? ContainsNsfwContent { get; set; }

        [TomlProperty("dependencies"), TomlDoNotInlineObject]
        public Dictionary<string, string>? Dependencies { get; set; } = [];

        [TomlProperty("dependency-group"), TomlDoNotInlineObject]
        public Dictionary<string, Dictionary<string, string>>? DependencyGroups { get; set; } = [];
    }

    /// <summary>
    /// Top-level package data, common to all communities.
    /// </summary>
    [TomlDoNotInlineObject]
    public class ProjectData : ProjectDataBase
    {
        [TomlProperty("community")]
        public CommunityData[] Communities { get; set; } = [];
    }

    [TomlDoNotInlineObject]
    public class CommunitySpecificProjectData : ProjectDataBase
    {
        [TomlProperty("categories")]
        public string[]? Categories { get; set; } = [];
    }

    [TomlDoNotInlineObject]
    public class CommunityData
    {
        [TomlProperty("name")]
        public string? Name { get; set; }

        [TomlProperty("project")]
        public CommunitySpecificProjectData? Project { get; set; }
    }

    [TomlProperty("project")]
    public ProjectData? Project { get; set; }

    [TomlDoNotInlineObject]
    public class BuildData
    {
        [TomlProperty("icon")]
        public string? Icon { get; set; }

        [TomlProperty("readme")]
        public string? Readme { get; set; }

        [TomlProperty("out-directory")]
        public string? OutDirectory { get; set; }

        [TomlDoNotInlineObject]
        public class CopyPath
        {
            [TomlProperty("source")]
            public string? Source { get; set; }

            [TomlProperty("target")]
            public string? Target { get; set; }
        }

        [TomlProperty("copy")]
        public CopyPath[] CopyPaths { get; set; } = [];
    }
    [TomlProperty("build")]
    public BuildData? Build { get; set; }

    [TomlDoNotInlineObject]
    public class PublishData
    {
        [TomlProperty("repository")]
        public string? Repository { get; set; }
    }
    [TomlProperty("publish")]
    public PublishData? Publish { get; set; }

    [TomlDoNotInlineObject]
    public class InstallData
    {
        [TomlDoNotInlineObject]
        public class InstallerDeclaration
        {
            [TomlProperty("identifier")]
            public string? Identifier { get; set; }
        }

        [TomlProperty("installers")]
        public InstallerDeclaration[] InstallerDeclarations { get; set; } = [];
    }
    [TomlProperty("install")]
    public InstallData? Install { get; set; }

    public ThunderstoreProject() { }

    public ThunderstoreProject(bool initialize)
    {
        if (!initialize)
            return;

        Project = new ProjectData();
        Build = new BuildData();
        Publish = new PublishData();
        Install = new InstallData();
    }

    public ThunderstoreProject(Config config)
    {
        Project = new ProjectData
        {
            Namespace = config.PackageConfig.Namespace,
            Name = config.PackageConfig.Name,
            VersionNumber = config.PackageConfig.VersionNumber,
            Description = config.PackageConfig.Description,
            ProjectUrl = config.PackageConfig.ProjectUrl,
            RepositoryUrl = config.PackageConfig.RepositoryUrl,
            ContainsNsfwContent = config.PackageConfig.ContainsNsfwContent,
            Dependencies = config.PackageConfig.Dependencies,
            DependencyGroups = config.PackageConfig.DependencyGroups,
            Communities = (config.PackageConfig.Communities ?? [])
                .Select(community => new CommunityData
                {
                    Name = community.Name,
                    Project = new CommunitySpecificProjectData
                    {
                        Namespace = community.Project?.Namespace,
                        Name = community.Project?.Name,
                        VersionNumber = community.Project?.VersionNumber,
                        Description = community.Project?.Description,
                        ProjectUrl = community.Project?.ProjectUrl,
                        RepositoryUrl = community.Project?.RepositoryUrl,
                        ContainsNsfwContent = community.Project?.ContainsNsfwContent,
                        Dependencies = community.Project?.Dependencies,
                        DependencyGroups = community.Project?.DependencyGroups,
                        Categories = community.Project?.Categories?.ToArray(),
                    }
                })
                .ToArray(),
        };
        Build = new BuildData
        {
            Icon = config.BuildConfig.IconPath,
            OutDirectory = config.BuildConfig.OutDir,
            Readme = config.BuildConfig.ReadmePath,
            CopyPaths = (config.BuildConfig.CopyPaths ?? [])
                .Select(x => new BuildData.CopyPath { Source = x.From, Target = x.To })
                .ToArray(),
        };
        Publish = new PublishData
        {
            Repository = config.GeneralConfig.Repository,
        };
        Install = new InstallData
        {
            InstallerDeclarations = (config.InstallConfig.InstallerDeclarations ?? [])
                .Select(x => new InstallData.InstallerDeclaration { Identifier = x.Identifier })
                .ToArray(),
        };
    }
}

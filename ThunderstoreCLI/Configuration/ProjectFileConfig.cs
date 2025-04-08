using ThunderstoreCLI.Models;
using static Kokuban.Chalk;

namespace ThunderstoreCLI.Configuration;

internal class ProjectFileConfig : EmptyConfig
{
    private string SourcePath = null!;
    private ThunderstoreProject Project = null!;

    public override void Parse(Config currentConfig)
    {
        SourcePath = currentConfig.GetProjectConfigPath();
        if (!File.Exists(SourcePath))
        {
            Utils.Write.Warn(
                "Unable to find project configuration file",
                $"Looked from {Dim.Render(SourcePath)}"
            );
            Project = new ThunderstoreProject(false);
            return;
        }
        Project = ThunderstoreProject.Deserialize(File.ReadAllText(SourcePath))!;
    }

    public override GeneralConfig GetGeneralConfig()
    {
        return new GeneralConfig
        {
            Repository = Project.Publish?.Repository!
        };
    }

    public override PackageConfig GetPackageMeta()
    {
        var project = Project.Project;
        if (project is null)
            return new PackageConfig
            {
                ProjectConfigPath = SourcePath,
            };

        return new PackageConfig
        {
            ProjectConfigPath = SourcePath,
            Namespace = project.Namespace,
            Name = project.Name,
            VersionNumber = project.VersionNumber,
            Description = project.Description,
            Dependencies = project.Dependencies,
            DependencyGroups = project.DependencyGroups,
            ContainsNsfwContent = project.ContainsNsfwContent,
            ProjectUrl = project.ProjectUrl,
            RepositoryUrl = project.RepositoryUrl,
            Communities = project.Communities
                .Select(community => new CommunityConfig
                {
                    Name = community.Name,
                    Project = new CommunitySpecificProjectConfig
                    {
                        Namespace = community.Project?.Namespace,
                        Name = community.Project?.Name,
                        VersionNumber = community.Project?.VersionNumber,
                        Description = community.Project?.Description,
                        Dependencies = community.Project?.Dependencies,
                        DependencyGroups = community.Project?.DependencyGroups,
                        ContainsNsfwContent = community.Project?.ContainsNsfwContent,
                        ProjectUrl = community.Project?.ProjectUrl,
                        RepositoryUrl = community.Project?.RepositoryUrl,
                        Categories = community.Project?.Categories?.ToList(),
                    }
                })
                .ToList(),
        };
    }

    public override BuildConfig GetBuildConfig()
    {
        return new BuildConfig
        {
            CopyPaths = Project.Build?.CopyPaths
                .Select(static path => new CopyPathMap(path.Source, path.Target))
                .ToList(),
            IconPath = Project.Build?.Icon,
            OutDir = Project.Build?.OutDirectory,
            ReadmePath = Project.Build?.Readme
        };
    }

    public override PublishConfig GetPublishConfig()
    {
        return new PublishConfig();
    }

    public override InstallConfig GetInstallConfig()
    {
        return new InstallConfig
        {
            InstallerDeclarations = Project.Install?.InstallerDeclarations
                .Select(static path => new InstallerDeclaration(path.Identifier))
                .ToList()
        };
    }

    public static void Write(Config config, string path)
    {
        File.WriteAllText(path, new ThunderstoreProject(config).Serialize());
    }
}

using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using StreamBigJson;
using ThunderstoreCLI;

var community = "repo";
{
    await using var context = new PackageIndexContext { DbPath = $"./{community}-index.db" };
    using var http = new HttpClient();
    await context.Database.EnsureDeletedAsync();
    await context.Database.MigrateAsync();

    var stream = await http.GetStreamAsync(new Uri($"{Defaults.REPOSITORY_URL}/c/{community}/api/v1/package/"));

    var enumerable = JsonSerializer.DeserializeAsyncEnumerable<Package>(stream);
    var bufferer = new SaveBufferer(context, 500);
    await foreach (var package in enumerable)
    {
        if (package is null)
            continue;
        foreach (var version in package.Versions.ToArray())
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (version.VersionNumber is null)
                package.Versions.Remove(version);
        }
        context.Add(package);
        await bufferer.BufferedSave();
    }
    await bufferer.Save();
}

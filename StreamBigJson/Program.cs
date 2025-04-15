using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using StreamBigJson;
using ThunderstoreCLI;

var community = "lethal-company";
{
    await using var context = new PackageIndexContext { DbPath = $"./{community}-index.db" };
    using var http = new HttpClient();
    await context.Database.EnsureDeletedAsync();
    await context.Database.MigrateAsync();

    var stream = await http.GetStreamAsync(new Uri($"{Defaults.REPOSITORY_URL}/c/{community}/api/v1/package/"));

    var enumerable = JsonSerializer.DeserializeAsyncEnumerable<Package>(stream);
    var counter = 0;
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
        counter = await BufferedSave(counter, context);
    }
    await Save(context);
}

return;

async Task<int> BufferedSave(int counter, DbContext context)
{
    if (++counter < 500)
        return counter;
    await Save(context);
    return 0;
}

async Task Save(DbContext context)
{
    await context.SaveChangesAsync();
}

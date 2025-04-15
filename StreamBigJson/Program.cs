using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using StreamBigJson;

using (var context = new PackageIndexContext())
using (var fs = File.OpenRead("../lethal-company-index.json"))
{
    await context.Database.EnsureDeletedAsync();
    await context.Database.MigrateAsync();

    var enumerable = JsonSerializer.DeserializeAsyncEnumerable<Package>(fs);
    var counter = 0;
    await foreach (var package in enumerable)
    {
        if (package is null) continue;
        foreach (var version in package.Versions.ToArray())
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (version.VersionNumber is null) package.Versions.Remove(version);
        }
        context.Add(package);
        counter = await BufferedSave(counter, context);
    }
    await Save(context);
}

return;

async Task<int> BufferedSave(int counter, DbContext context)
{
    if (++counter < 500) return counter;
    await Save(context);
    return 0;
}

async Task Save(DbContext context)
{
    await context.SaveChangesAsync();
}

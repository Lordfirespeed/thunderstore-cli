using Microsoft.EntityFrameworkCore;

namespace StreamBigJson;

public class SaveBufferer(DbContext context, int threshold)
{
    private int _counter = 0;

    public async Task BufferedSave()
    {
        if (++_counter < threshold)
            return;
        await Save();
    }

    public async Task Save()
    {
        await context.SaveChangesAsync();
    }
}

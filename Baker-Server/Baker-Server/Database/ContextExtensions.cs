using Baker_Server.Database.Entities;

namespace Baker_Server.Database
{
    public static class ContextExtensions
    {
        public static async Task<int> Log(this AppDbContext context, BakerLog log)
        {
            await context.Logs.AddAsync(log);
            return await context.SaveChangesAsync();
        }

        public static async Task<int> Log(this AppDbContext context, string message)
        {
            await context.Logs.AddAsync(new()
            {
                Description = message
            });

            return await context.SaveChangesAsync();
        }
    }
}

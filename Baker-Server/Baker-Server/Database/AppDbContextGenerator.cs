using Baker_Server.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Baker_Server.Database
{
    public class AppDbContextGenerator
    {
        private readonly AppDbContext _context;

        public AppDbContextGenerator(AppDbContext context)
        {
            _context = context;
        }

        public async Task InitialiseAsync()
        {
            if (_context.Database.IsNpgsql())
            {
                await _context.Database.MigrateAsync();
            }
        }

        public async Task SeedAsync()
        {
            if (!_context.BunTypes.Any())
            {
                await _context.BunTypes.AddRangeAsync(new BunType[]
                {
                    new()
                    {
                        Name = BunNameEnum.Cruassan,
                        DefaultPrice = 66.5,
                        SellTerm = TimeSpan.FromHours(18),
                        ControlTerm = TimeSpan.FromHours(9)
                    },
                    new()
                    {
                        Name = BunNameEnum.Crendel,
                        DefaultPrice = 45.4,
                        SellTerm = TimeSpan.FromHours(24),
                        ControlTerm = TimeSpan.FromHours(12)
                    },
                    new()
                    {
                        Name = BunNameEnum.Baget,
                        DefaultPrice = 87.7,
                        SellTerm = TimeSpan.FromHours(16),
                        ControlTerm = TimeSpan.FromHours(8)
                    },
                    new()
                    {
                        Name = BunNameEnum.Smetanik,
                        DefaultPrice = 56.2,
                        SellTerm = TimeSpan.FromHours(8),
                        ControlTerm = TimeSpan.FromHours(3)
                    },
                    new()
                    {
                        Name = BunNameEnum.Baton,
                        DefaultPrice = 38.9,
                        SellTerm = TimeSpan.FromHours(36),
                        ControlTerm = TimeSpan.FromHours(18)
                    }
                });

                await _context.SaveChangesAsync();
            }
        }
    }
}

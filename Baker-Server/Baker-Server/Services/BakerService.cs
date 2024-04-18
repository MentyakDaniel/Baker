using Baker_Server.Database;
using Baker_Server.Database.Entities;
using Baker_Server.Services.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Baker_Server.Services
{
    public class BakerService : IBakerService
    {
        private readonly AppDbContext _context;

        public BakerService(AppDbContext context)
        {
            _context = context;    
        }

        public async Task<ICollection<BunSaleDto>> GetAllSales() 
            => await _context.SalesBun
                .Include(item => item.BunType)
                .Include(item => item.Monitorings)
                .Select(item => item.ToDto())
                .ToListAsync();

        public async Task StartBake(int count)
        {
            int i = count;

            Random rnd = new();

            int max = Enum.GetNames(typeof(BunNameEnum)).Length;

            while (i > 0)
            {
                BunNameEnum name = (BunNameEnum)rnd.Next(1, max);

                BunType bunType = _context.BunTypes
                    .Where(item => item.Name == name)
                    .First();

                BunSale sale = new()
                {
                    BunTypeId = bunType.Id,
                    BunType = bunType,
                    BakedTime = DateTime.UtcNow,
                    Price = bunType.DefaultPrice
                };

                EntityEntry<BunSale> result = await _context.SalesBun.AddAsync(sale);

                await _context.SaveChangesAsync();

                await sale.CalculateNextPrice(_context);

                i--;
            }

            await _context.Log($"Приготовили новую партию в размере {count} булочек");
        }
    }
}

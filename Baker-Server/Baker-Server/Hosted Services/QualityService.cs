using Baker_Server.Database;
using Baker_Server.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Baker_Server.Services
{
    public class QualityService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private AppDbContext _context;
        private static readonly TimeSpan Interval = new(0, 0, 30);

        public QualityService(IServiceProvider services)
        {
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    ReinitServices();
                    await CalculateQuality(stoppingToken);
                }
                catch (Exception ex)
                {
                    await _context.Log(ex.Message);
                }
                finally
                {
                    await Task.Delay(Interval, stoppingToken);
                }
            }
        }

        private void ReinitServices()
        {
            IServiceScope scope = _services.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        }

        private async Task CalculateQuality(CancellationToken stoppingToken)
        {
            List<BunSale> sales = await _context.SalesBun
                .Include(item => item.BunType)
                .ToListAsync(stoppingToken);

            if(sales.Count == 0) return; 

            sales = sales
                .Except(await RemoveBadBun(sales, stoppingToken))
                .ToList();

            await UpdatePrices(sales, stoppingToken);

            await _context.SaveChangesAsync(stoppingToken);
        }

        private async Task UpdatePrices(List<BunSale> sales, CancellationToken stoppingToken)
        {
            foreach (BunSale sale in sales)
            {
                QualityMonitoring? monitoring = await _context.Monitorings
                    .OrderByDescending(item => item.TimeStamp)
                    .Where(item => item.BunSaleId == sale.Id && item.IsThrow == false)
                    .FirstAsync(stoppingToken) ?? null;

                if (monitoring is not null)
                {
                    DateTime? nextPriceTime = monitoring.TimeStamp + monitoring.ToNextPrice;

                    if (nextPriceTime <= DateTime.UtcNow)
                    {
                        sale.Price = monitoring.NextPrice ?? sale.Price;

                        await sale.CalculateNextPrice(_context);
                    }
                    else
                    {
                        monitoring.ToNextPrice = nextPriceTime - DateTime.UtcNow;
                        monitoring.TimeStamp = DateTime.UtcNow;

                        _context.Monitorings.Update(monitoring);

                        await _context.SaveChangesAsync();
                    }
                }
                else
                    await sale.CalculateNextPrice(_context);
            }

            _context.SalesBun.UpdateRange(sales);
        }
        private async Task<IEnumerable<BunSale>> RemoveBadBun(List<BunSale> sales, CancellationToken stoppingToken)
        {
            IEnumerable<BunSale> trash = sales
                .Where(item => item.BakedTime + item.BunType.SellTerm <= DateTime.UtcNow);

            foreach (BunSale sale in trash)
            {
                await _context.Monitorings.AddAsync(new QualityMonitoring()
                    {
                        IsThrow = true,
                        BunSaleId = sale.Id,
                        TimeStamp = DateTime.UtcNow
                    }, stoppingToken);

                _context.SalesBun.Remove(sale);

                await _context.Log($"Снята с продажи {sale.BunType.Name} - {sale.Id} по причине истечения срока хранения - {sale.BunType.SellTerm}");
            }

            return trash;
        }
    }
}

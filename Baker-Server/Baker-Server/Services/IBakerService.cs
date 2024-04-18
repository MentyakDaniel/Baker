using Baker_Server.Services.Models;

namespace Baker_Server.Services
{
    public interface IBakerService
    {
        public Task<ICollection<BunSaleDto>> GetAllSales();
        public Task StartBake(int count);

    }
}

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Baker_Server.Database.Entities
{
    public class QualityMonitoring
    {
        [Key]
        public int Id { get; set; }
        public Guid BunSaleId { get; set; }
        public double? NextPrice { get; set; }
        public TimeSpan? ToNextPrice { get; set; }
        public bool IsThrow { get; set; } = false;
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public BunSale? BunSale { get; set; }
    }
}

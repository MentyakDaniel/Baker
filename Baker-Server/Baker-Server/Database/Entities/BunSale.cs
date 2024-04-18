using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Baker_Server.Database.Entities
{
    public class BunSale
    {
        [Key]
        public Guid Id { get; set; }
        public Guid BunTypeId { get; set; }
        public double Price { get; set; }
        public DateTime BakedTime { get; set; }

        [JsonIgnore]
        public BunType? BunType { get; set; }
        [JsonIgnore]
        public ICollection<QualityMonitoring>? Monitorings { get; set; } 
    }
}

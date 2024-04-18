using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Baker_Server.Database.Entities
{
    public class BunType
    {
        [Key]
        public Guid Id { get; set; }
        public BunNameEnum Name { get; set; }
        public double DefaultPrice { get; set; }

        public TimeSpan SellTerm { get; set; }
        public TimeSpan ControlTerm { get; set; }

        [JsonIgnore]
        public ICollection<BunSale>? BunForSales { get; set; }
    }
}

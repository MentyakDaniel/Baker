using System.ComponentModel.DataAnnotations;

namespace Baker_Server.Database.Entities
{
    public class BakerLog
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    }
}

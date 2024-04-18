using Baker_Server.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Baker_Server.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<BunType> BunTypes { get; set; }
        public DbSet<BunSale> SalesBun { get; set; }
        public DbSet<BakerLog> Logs { get; set; }
        public DbSet<QualityMonitoring> Monitorings { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BunType>().HasKey(x => x.Id);
            modelBuilder.Entity<BunType>().Property(x => x.Id).HasValueGenerator(typeof(GuidValueGenerator));
            modelBuilder.Entity<BunType>().Property(item => item.Name).HasConversion<int>();

            modelBuilder.Entity<BunSale>().HasKey(x => x.Id);
            modelBuilder.Entity<BunSale>().Property(x => x.Id).HasValueGenerator(typeof(GuidValueGenerator));

            modelBuilder.Entity<BakerLog>().HasKey(x => x.Id);
            modelBuilder.Entity<QualityMonitoring>().HasKey(x => x.Id);

            modelBuilder.Entity<BakerLog>().HasIndex(x => x.TimeStamp);
            modelBuilder.Entity<QualityMonitoring>().HasIndex(x => x.IsThrow);

            modelBuilder.Entity<BunSale>()
                .HasOne(item => item.BunType)
                .WithMany(item => item.BunForSales)
                .HasForeignKey(item => item.BunTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QualityMonitoring>()
                .HasOne(item => item.BunSale)
                .WithMany(item => item.Monitorings)
                .HasForeignKey(item => item.BunSaleId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}

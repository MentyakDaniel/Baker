using Baker_Server.Database;
using Baker_Server.Database.Entities;
using Baker_Server.Services.Dto;
using Baker_Server.Services.Models;
using System.ComponentModel;
using System.Reflection;

namespace Baker_Server.Services
{
    public static class BakerServiceExtension
    {
        public static QualitiMonitoringDto ToDto(this QualityMonitoring monitoring)
            => new (monitoring.Id, monitoring.BunSaleId, monitoring.NextPrice, monitoring.ToNextPrice?.ToString("hh\\:mm\\:ss") ?? "", monitoring.TimeStamp, monitoring.IsThrow);

        public static BunSaleDto ToDto(this BunSale sale) 
            => new (sale.Id, sale.BunTypeId, sale.Price, sale.BakedTime, sale.BunType ?? new(), sale.Monitorings?.OrderByDescending(item => item.TimeStamp).First().ToDto());

        public static BunSale ToModel(this BunSaleDto dto)
            => new()
            {
                Id= dto.Id,
                BunTypeId= dto.BunTypeId,
                BakedTime= dto.BakedTime,
                BunType= dto.BunType,
                Price= dto.Price
            };

        public static async Task CalculateNextPrice(this BunSale sale, AppDbContext context)
        {
            if (sale.BunType is not null)
            {
                switch (sale.BunType.Name)
                {
                    case BunNameEnum.Smetanik:
                        // до сих пор не понимаю до конца формулировку "Цена уменьшается каждой час с двойной скоростью"
                        // и так как контекст "каждого часа" сохраняется, то на время мы не влияем
                        // а значит самый логический путь, который я понял, это уменьшать стоимость сметанника
                        // в два раза больше, чем другой продукции. Раз продукция уменьшается на 2% от первоначальной цены,
                        // то сметанник уменьшается на 4% от первоначальной цены.
                        double minus = sale.BunType.DefaultPrice / 100 * 4;

                        await context.Monitorings.AddAsync(new()
                        {
                            BunSaleId = sale.Id,
                            NextPrice = sale.Price - minus,
                            ToNextPrice = TimeSpan.FromHours(1),
                            TimeStamp = DateTime.UtcNow
                        });

                        break;
                    case BunNameEnum.Crendel:
                        if (sale.Price == sale.BunType.DefaultPrice)
                        {
                            await context.Monitorings.AddAsync(new()
                            {
                                BunSaleId = sale.Id,
                                NextPrice = sale.BunType.DefaultPrice / 2,
                                ToNextPrice = (sale.BakedTime + sale.BunType.ControlTerm) - DateTime.UtcNow,
                                TimeStamp = DateTime.UtcNow
                            });
                        }
                        break;
                    default:
                        double procent = sale.BunType.DefaultPrice / 100 * 2;

                        await context.Monitorings.AddAsync(new()
                        {
                            BunSaleId = sale.Id,
                            NextPrice = sale.Price - procent,
                            ToNextPrice = TimeSpan.FromHours(1),
                            TimeStamp = DateTime.UtcNow
                        });

                        break;
                }

                await context.SaveChangesAsync();
            }
        }

        public static ICollection<EnumValueDto> GetEnumValues(Type enumType)
        {
            Array values = Enum.GetValues(enumType);

            List<EnumValueDto> result = new();

            foreach (Enum val in values)
            {
                string descrpAttr = DescriptionAttr(val);

                EnumValueDto item = new(Convert.ToInt32(val), val.ToString(), descrpAttr ?? val.ToString());

                result.Add(item);
            }

            return result;
        }

        private static string DescriptionAttr(Enum source)
        {
            FieldInfo? fi = source.GetType().GetField(source.ToString());

            if (fi is not null)
            {
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi
                    .GetCustomAttributes(typeof(DescriptionAttribute), false);

                return attributes != null && attributes.Length > 0
                    ? attributes[0].Description
                    : source.ToString();
            }

            return source.ToString();
        }
    }
}

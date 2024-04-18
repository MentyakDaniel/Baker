using Baker_Server.Database.Entities;
using Baker_Server.Services.Dto;

namespace Baker_Server.Services.Models
{
    public record BunSaleDto(Guid Id, Guid BunTypeId, double Price, DateTime BakedTime, BunType BunType, QualitiMonitoringDto? Monitoring);
}

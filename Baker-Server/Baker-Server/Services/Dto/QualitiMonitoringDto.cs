namespace Baker_Server.Services.Dto
{
    public record QualitiMonitoringDto(int Id, Guid BunSaleId, double? NextPrice, string? ToNextPrice, DateTime TimeStamp, bool IsThrow = false);
}

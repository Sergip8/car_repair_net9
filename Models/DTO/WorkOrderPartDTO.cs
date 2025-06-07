namespace car_repair.Models.DTO
{
    public record CreateWorkOrderPartRequest(
        int WorkOrderId,
        int PartId,
        int QuantityUsed,
        decimal UnitPrice,
        string? Notes
    );

    public record WorkOrderPartResponse(
        int Id,
        int WorkOrderId,
        int PartId,
        int QuantityUsed,
        decimal UnitPrice,
        string? Notes
    );
}

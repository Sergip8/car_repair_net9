namespace car_repair.Models.DTO
{
    public record CreateWorkOrderServiceRequest(
        int WorkOrderId,
        int ServiceId,
        decimal Price,
        decimal Quantity,
        string? Notes
    );

    public record WorkOrderServiceResponse(
        int Id,
        int WorkOrderId,
        int ServiceId,
        decimal Price,
        decimal Quantity,
        string? Notes
    );
}

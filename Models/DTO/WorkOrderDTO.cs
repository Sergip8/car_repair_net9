namespace car_repair.Models.DTO
{
    public record CreateWorkOrderRequest(
        string WorkOrderNumber,
        WorkOrderStatus Status,
        string? Description,
        string? DiagnosisNotes,
        string? CompletionNotes,
        DateTime? StartedAt,
        DateTime? CompletedAt,
        decimal LaborHours,
        decimal TotalCost,
        int VehicleId,
        int? AppointmentId,
        int? AssignedEmployeeId
    );

    public record WorkOrderResponse(
        int Id,
        string WorkOrderNumber,
        WorkOrderStatus Status,
        string? Description,
        string? DiagnosisNotes,
        string? CompletionNotes,
        DateTime? StartedAt,
        DateTime? CompletedAt,
        decimal LaborHours,
        decimal TotalCost,
        int VehicleId,
        int? AppointmentId,
        int? AssignedEmployeeId
    );
}

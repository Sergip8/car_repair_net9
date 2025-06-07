namespace car_repair.Models.DTO
{
    public record CreateAppointmentRequest(
        DateTime ScheduledDateTime,
        int EstimatedDurationMinutes,
        int CustomerId,
        int VehicleId,
        int? AssignedEmployeeId,
        string? Description,
        string? CustomerNotes,
        string? InternalNotes
    );

    public record AppointmentResponse(
        int Id,
        DateTime ScheduledDateTime,
        int EstimatedDurationMinutes,
        AppointmentStatus Status,
        string? Description,
        string? CustomerNotes,
        string? InternalNotes,
        int CustomerId,
        int VehicleId,
        int? AssignedEmployeeId
    );
}

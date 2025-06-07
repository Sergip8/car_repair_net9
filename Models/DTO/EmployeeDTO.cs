namespace car_repair.Models.DTO
{
    public record CreateEmployeeRequest(
        string FirstName,
        string LastName,
        string Email,
        string? PhoneNumber,
        string Position,
        decimal? HourlyRate,
        DateTime? HireDate,
        bool IsActive
    );

    public record EmployeeResponse(
        int Id,
        string FirstName,
        string LastName,
        string Email,
        string? PhoneNumber,
        string Position,
        decimal? HourlyRate,
        DateTime? HireDate,
        bool IsActive
    );
}

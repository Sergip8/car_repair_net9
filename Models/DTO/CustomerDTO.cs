namespace car_repair.Models.DTO
{
    public record CreateCustomerRequest(
        string FirstName,
        string LastName,
        string Email,
        string? PhoneNumber,
        string? Address,
        string? City,
        string? PostalCode,
        string? State
    );

    public record CustomerResponse(
        int Id,
        string FirstName,
        string LastName,
        string Email,
        string? PhoneNumber,
        string? Address,
        string? City,
        string? PostalCode,
        string? State
    );
}

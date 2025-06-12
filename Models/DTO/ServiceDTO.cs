namespace car_repair.Models.DTO
{
    public record CreateServiceRequest(
        string Name,
        string? Description,
        decimal BasePrice,
        int EstimatedDurationMinutes,
        bool IsActive,
        int ServiceCategoryId
    );

    public record ServiceResponse(
        int Id,
        string Name,
        string? Description,
        decimal BasePrice,
        int EstimatedDurationMinutes,
        bool IsActive,
        int ServiceCategoryId
    );

    public record ServiceWithCategoryResponse(
        int Id,
        string Name,
        string? Description,
        decimal BasePrice,
        int EstimatedDurationMinutes,
        bool IsActive,
        int ServiceCategoryId,
        ServiceCategoryResponse ServiceCategory
    );
}

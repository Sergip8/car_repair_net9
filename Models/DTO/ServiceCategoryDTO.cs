namespace car_repair.Models.DTO
{
    public record CreateServiceCategoryRequest(
        string Name,
        string? Description
    );

    public record ServiceCategoryResponse(
        int Id,
        string Name,
        string? Description
    );
}

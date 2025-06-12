namespace car_repair.Models.DTO
{
    public record BrandResponse(
        int Id,
        string Title,
        string? ImageUrl
    );
} 
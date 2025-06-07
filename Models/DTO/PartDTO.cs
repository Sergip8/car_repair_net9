namespace car_repair.Models.DTO
{
    public record CreatePartRequest(
        string PartNumber,
        string Name,
        string? Description,
        string? Brand,
        decimal Cost,
        decimal SellPrice,
        int QuantityInStock,
        int MinimumStock,
        string? Location,
        bool IsActive
    );

    public record PartResponse(
        int Id,
        string PartNumber,
        string Name,
        string? Description,
        string? Brand,
        decimal Cost,
        decimal SellPrice,
        int QuantityInStock,
        int MinimumStock,
        string? Location,
        bool IsActive
    );
}

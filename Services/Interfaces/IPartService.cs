using car_repair.Models.DTO;

public interface IPartService
{
    Task<PaginationResponse<Part>> GetPaginatedParts(PaginationRequest pagination);
    Task<PartResponse> CreatePart(CreatePartRequest request);
    Task<PartResponse> UpdatePart(int id, CreatePartRequest request);
    Task<PartResponse> GetPartById(int id);
    Task<PartResponse> DeletePart(int id);
}

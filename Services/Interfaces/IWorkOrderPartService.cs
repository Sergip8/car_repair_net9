using car_repair.Models.DTO;

public interface IWorkOrderPartService
{
    Task<PaginationResponse<WorkOrderPart>> GetPaginatedWorkOrderParts(PaginationRequest pagination);
    Task<WorkOrderPartResponse> CreateWorkOrderPart(CreateWorkOrderPartRequest request);
    Task<WorkOrderPartResponse> UpdateWorkOrderPart(int id, CreateWorkOrderPartRequest request);
    Task<WorkOrderPartResponse> GetWorkOrderPartById(int id);
    Task<WorkOrderPartResponse> DeleteWorkOrderPart(int id);
}

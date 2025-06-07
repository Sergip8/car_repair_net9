using car_repair.Models.DTO;

public interface IWorkOrderServiceService
{
    Task<PaginationResponse<WorkOrderServiceEnt>> GetPaginatedWorkOrderServices(PaginationRequest pagination);
    Task<WorkOrderServiceResponse> CreateWorkOrderService(CreateWorkOrderServiceRequest request);
    Task<WorkOrderServiceResponse> UpdateWorkOrderService(int id, CreateWorkOrderServiceRequest request);
    Task<WorkOrderServiceResponse> GetWorkOrderServiceById(int id);
    Task<WorkOrderServiceResponse> DeleteWorkOrderService(int id);
}

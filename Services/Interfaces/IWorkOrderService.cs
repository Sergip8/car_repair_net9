using car_repair.Models.DTO;

public interface IWorkOrderService
{
    Task<PaginationResponse<WorkOrder>> GetPaginatedWorkOrders(PaginationRequest pagination);
    Task<WorkOrderResponse> CreateWorkOrder(CreateWorkOrderRequest request);
    Task<WorkOrderResponse> UpdateWorkOrder(int id, CreateWorkOrderRequest request);
    Task<WorkOrderResponse> GetWorkOrderById(int id);
    Task<WorkOrderResponse> DeleteWorkOrder(int id);
}

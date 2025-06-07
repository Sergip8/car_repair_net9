using car_repair.Models.DTO;

public interface IServiceService
{
    Task<PaginationResponse<Service>> GetPaginatedServices(PaginationRequest pagination);
    Task<ServiceResponse> CreateService(CreateServiceRequest request);
    Task<ServiceResponse> UpdateService(int id, CreateServiceRequest request);
    Task<ServiceResponse> GetServiceById(int id);
    Task<ServiceResponse> DeleteService(int id);
}

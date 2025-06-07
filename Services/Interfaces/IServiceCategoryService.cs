using car_repair.Models.DTO;

public interface IServiceCategoryService
{
    Task<PaginationResponse<ServiceCategory>> GetPaginatedServiceCategories(PaginationRequest pagination);
    Task<ServiceCategoryResponse> CreateServiceCategory(CreateServiceCategoryRequest request);
    Task<ServiceCategoryResponse> UpdateServiceCategory(int id, CreateServiceCategoryRequest request);
    Task<ServiceCategoryResponse> GetServiceCategoryById(int id);
    Task<ServiceCategoryResponse> DeleteServiceCategory(int id);
}

using car_repair.Models.DTO;

public interface ICustomerService
{
    Task<PaginationResponse<Customer>> GetPaginatedCustomers(PaginationRequest pagination);
    Task<CustomerResponse> CreateCustomer(CreateCustomerRequest request);
    Task<CustomerResponse> UpdateCustomer(int id, CreateCustomerRequest request);
    Task<CustomerResponse> GetCustomerById(int id);
    Task<CustomerResponse> DeleteCustomer(int id);
}

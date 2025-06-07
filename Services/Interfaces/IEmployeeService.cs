using car_repair.Models.DTO;

public interface IEmployeeService
{
    Task<PaginationResponse<Employee>> GetPaginatedEmployees(PaginationRequest pagination);
    Task<EmployeeResponse> CreateEmployee(CreateEmployeeRequest request);
    Task<EmployeeResponse> UpdateEmployee(int id, CreateEmployeeRequest request);
    Task<EmployeeResponse> GetEmployeeById(int id);
    Task<EmployeeResponse> DeleteEmployee(int id);
}

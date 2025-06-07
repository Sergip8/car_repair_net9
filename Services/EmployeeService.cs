using AutoMapper;
using car_repair.Models.DTO;
using Microsoft.EntityFrameworkCore;

class EmployeeService : IEmployeeService
{
    private CarRepairDbContext _context;
    private ILogger<EmployeeService> _logger;
    private readonly IExceptionHandlingService _exceptionHandling;
    private IMapper _mapper;

    public EmployeeService(CarRepairDbContext context, ILogger<EmployeeService> logger, IExceptionHandlingService exceptionHandling, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _exceptionHandling = exceptionHandling;
        _mapper = mapper;
    }

    public async Task<PaginationResponse<Employee>> GetPaginatedEmployees(PaginationRequest pagination)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            pagination.ThrowIfNull(nameof(pagination));
            if (pagination.Page < 1) pagination.Page = 1;
            if (pagination.Size < 1 || pagination.Size > 100) pagination.Size = 10;
            IQueryable<Employee> query = _context.Employees.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(pagination.Query))
            {
                string searchTerm = pagination.Query.Trim().ToLowerInvariant();
                query = query.Where(e => (e.FirstName != null && EF.Functions.Like(e.FirstName.ToLower(), $"%{searchTerm}%")) || (e.LastName != null && EF.Functions.Like(e.LastName.ToLower(), $"%{searchTerm}%")));
            }
            var totalCount = await query.CountAsync();
            if (totalCount == 0)
            {
                return new PaginationResponse<Employee>(new List<Employee>(), pagination.Page, pagination.Size, 0, 0, true);
            }
            var totalPages = (int)Math.Ceiling(totalCount / (double)pagination.Size);
            var isLastPage = pagination.Page >= totalPages;
            var employees = await query.Skip((pagination.Page - 1) * pagination.Size).Take(pagination.Size).ToListAsync();
            return new PaginationResponse<Employee>(employees, pagination.Page, pagination.Size, totalPages, totalCount, isLastPage);
        }, nameof(GetPaginatedEmployees));
    }

    public async Task<EmployeeResponse> GetEmployeeById(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var employee = await _context.Employees.FindAsync(id);
            employee?.ThrowIfNotFound("Employee", id);
            return _mapper.Map<EmployeeResponse>(employee);
        }, nameof(GetEmployeeById));
    }

    public async Task<EmployeeResponse> CreateEmployee(CreateEmployeeRequest request)
    {
        var employee = _mapper.Map<Employee>(request);
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return _mapper.Map<EmployeeResponse>(employee);
        }, nameof(CreateEmployee));
    }

    public async Task<EmployeeResponse> UpdateEmployee(int id, CreateEmployeeRequest request)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var employee = await _context.Employees.FindAsync(id);
            employee.ThrowIfNotFound("Employee", id);
            // Map fields from request to employee
            employee.FirstName = request.FirstName;
            employee.LastName = request.LastName;
            employee.Email = request.Email;
            employee.PhoneNumber = request.PhoneNumber;
            employee.Position = request.Position;
            employee.HourlyRate = request.HourlyRate;
            employee.HireDate = request.HireDate;
            employee.IsActive = request.IsActive;
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
            return _mapper.Map<EmployeeResponse>(employee);
        }, nameof(UpdateEmployee));
    }

    public async Task<EmployeeResponse> DeleteEmployee(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var employee = await _context.Employees.FindAsync(id);
            employee.ThrowIfNotFound("Employee", id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return _mapper.Map<EmployeeResponse>(employee);
        }, nameof(DeleteEmployee));
    }
}

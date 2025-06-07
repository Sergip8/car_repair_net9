using AutoMapper;
using car_repair.Models.DTO;
using Microsoft.EntityFrameworkCore;

class CustomerService : ICustomerService
{
    private CarRepairDbContext _context;
    private ILogger<CustomerService> _logger;
    private readonly IExceptionHandlingService _exceptionHandling;
    private IMapper _mapper;

    public CustomerService(CarRepairDbContext context, ILogger<CustomerService> logger, IExceptionHandlingService exceptionHandling, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _exceptionHandling = exceptionHandling;
        _mapper = mapper;
    }

    public async Task<PaginationResponse<Customer>> GetPaginatedCustomers(PaginationRequest pagination)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            pagination.ThrowIfNull(nameof(pagination));
            if (pagination.Page < 1) pagination.Page = 1;
            if (pagination.Size < 1 || pagination.Size > 100) pagination.Size = 10;
            IQueryable<Customer> query = _context.Customers.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(pagination.Query))
            {
                string searchTerm = pagination.Query.Trim().ToLowerInvariant();
                query = query.Where(c => (c.FirstName != null && EF.Functions.Like(c.FirstName.ToLower(), $"%{searchTerm}%")) || (c.LastName != null && EF.Functions.Like(c.LastName.ToLower(), $"%{searchTerm}%")));
            }
            var totalCount = await query.CountAsync();
            if (totalCount == 0)
            {
                return new PaginationResponse<Customer>(new List<Customer>(), pagination.Page, pagination.Size, 0, 0, true);
            }
            var totalPages = (int)Math.Ceiling(totalCount / (double)pagination.Size);
            var isLastPage = pagination.Page >= totalPages;
            var customers = await query.Skip((pagination.Page - 1) * pagination.Size).Take(pagination.Size).ToListAsync();
            return new PaginationResponse<Customer>(customers, pagination.Page, pagination.Size, totalPages, totalCount, isLastPage);
        }, nameof(GetPaginatedCustomers));
    }

    public async Task<CustomerResponse> GetCustomerById(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var customer = await _context.Customers.FindAsync(id);
            customer?.ThrowIfNotFound("Customer", id);
            return _mapper.Map<CustomerResponse>(customer);
        }, nameof(GetCustomerById));
    }

    public async Task<CustomerResponse> CreateCustomer(CreateCustomerRequest request)
    {
        var customer = _mapper.Map<Customer>(request);
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return _mapper.Map<CustomerResponse>(customer);
        }, nameof(CreateCustomer));
    }

    public async Task<CustomerResponse> UpdateCustomer(int id, CreateCustomerRequest request)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var customer = await _context.Customers.FindAsync(id);
            customer?.ThrowIfNotFound("Customer", id);
            // Map fields from request to customer
            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;
            customer.Email = request.Email;
            customer.PhoneNumber = request.PhoneNumber;
            customer.Address = request.Address;
            customer.City = request.City;
            customer.PostalCode = request.PostalCode;
            customer.State = request.State;
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
            return _mapper.Map<CustomerResponse>(customer);
        }, nameof(UpdateCustomer));
    }

    public async Task<CustomerResponse> DeleteCustomer(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
    {
        var customer = await _context.Customers
            .Include(c => c.Vehicles)
            .FirstOrDefaultAsync(c => c.Id == id);

        customer.ThrowIfNotFound("Customer", id);

        if (customer.Vehicles.Any())
            throw new BusinessLogicException("No se puede eliminar un cliente con vehículos registrados.");

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
        return _mapper.Map<CustomerResponse>(customer);
    }, nameof(DeleteCustomer));
    }
}

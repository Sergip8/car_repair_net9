using AutoMapper;
using car_repair.Models.DTO;
using Microsoft.EntityFrameworkCore;

class ServiceService : IServiceService
{
    private CarRepairDbContext _context;
    private ILogger<ServiceService> _logger;
    private readonly IExceptionHandlingService _exceptionHandling;
    private IMapper _mapper;

    public ServiceService(CarRepairDbContext context, ILogger<ServiceService> logger, IExceptionHandlingService exceptionHandling, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _exceptionHandling = exceptionHandling;
        _mapper = mapper;
    }

    public async Task<PaginationResponse<Service>> GetPaginatedServices(PaginationRequest pagination)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            pagination.ThrowIfNull(nameof(pagination));
            if (pagination.Page < 1) pagination.Page = 1;
            if (pagination.Size < 1 || pagination.Size > 100) pagination.Size = 10;
            IQueryable<Service> query = _context.Services.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(pagination.Query))
            {
                string searchTerm = pagination.Query.Trim().ToLowerInvariant();
                query = query.Where(s => s.Name != null && EF.Functions.Like(s.Name.ToLower(), $"%{searchTerm}%"));
            }
            var totalCount = await query.CountAsync();
            if (totalCount == 0)
            {
                return new PaginationResponse<Service>(new List<Service>(), pagination.Page, pagination.Size, 0, 0, true);
            }
            var totalPages = (int)Math.Ceiling(totalCount / (double)pagination.Size);
            var isLastPage = pagination.Page >= totalPages;
            var services = await query.Skip((pagination.Page - 1) * pagination.Size).Take(pagination.Size).ToListAsync();
            return new PaginationResponse<Service>(services, pagination.Page, pagination.Size, totalPages, totalCount, isLastPage);
        }, nameof(GetPaginatedServices));
    }

    public async Task<ServiceResponse> GetServiceById(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var service = await _context.Services.FindAsync(id);
            service.ThrowIfNotFound("Service", id);
            return _mapper.Map<ServiceResponse>(service);
        }, nameof(GetServiceById));
    }

    public async Task<ServiceResponse> CreateService(CreateServiceRequest request)
    {
        var service = _mapper.Map<Service>(request);
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _context.Services.Add(service);
            await _context.SaveChangesAsync();
            return _mapper.Map<ServiceResponse>(service);
        }, nameof(CreateService));
    }

    public async Task<ServiceResponse> UpdateService(int id, CreateServiceRequest request)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var service = await _context.Services.FindAsync(id);
            service.ThrowIfNotFound("Service", id);
            // Map fields from request to service
            service.Name = request.Name;
            service.Description = request.Description;
            service.BasePrice = request.BasePrice;
            service.EstimatedDurationMinutes = request.EstimatedDurationMinutes;
            service.IsActive = request.IsActive;
            service.ServiceCategoryId = request.ServiceCategoryId;
            _context.Services.Update(service);
            await _context.SaveChangesAsync();
            return _mapper.Map<ServiceResponse>(service);
        }, nameof(UpdateService));
    }

    public async Task<ServiceResponse> DeleteService(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var service = await _context.Services.FindAsync(id);
            service.ThrowIfNotFound("Service", id);
            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return _mapper.Map<ServiceResponse>(service);
        }, nameof(DeleteService));
    }

    public async Task<List<ServiceWithCategoryResponse>> GetServiceHome()
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var services = await _context.Services
                .Include(s => s.ServiceCategory)
                .Where(s => s.IsActive)
                .ToListAsync();
            return services.Select(s => _mapper.Map<ServiceWithCategoryResponse>(s)).ToList();
        }, nameof(GetServiceHome));
    }
}

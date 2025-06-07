using AutoMapper;
using car_repair.Models.DTO;
using Microsoft.EntityFrameworkCore;

class ServiceCategoryService : IServiceCategoryService
{
    private CarRepairDbContext _context;
    private ILogger<ServiceCategoryService> _logger;
    private readonly IExceptionHandlingService _exceptionHandling;
    private IMapper _mapper;

    public ServiceCategoryService(CarRepairDbContext context, ILogger<ServiceCategoryService> logger, IExceptionHandlingService exceptionHandling, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _exceptionHandling = exceptionHandling;
        _mapper = mapper;
    }

    public async Task<PaginationResponse<ServiceCategory>> GetPaginatedServiceCategories(PaginationRequest pagination)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            pagination.ThrowIfNull(nameof(pagination));
            if (pagination.Page < 1) pagination.Page = 1;
            if (pagination.Size < 1 || pagination.Size > 100) pagination.Size = 10;
            IQueryable<ServiceCategory> query = _context.ServiceCategories.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(pagination.Query))
            {
                string searchTerm = pagination.Query.Trim().ToLowerInvariant();
                query = query.Where(sc => (sc.Name != null && EF.Functions.Like(sc.Name.ToLower(), $"%{searchTerm}%")));
            }
            var totalCount = await query.CountAsync();
            if (totalCount == 0)
            {
                return new PaginationResponse<ServiceCategory>(new List<ServiceCategory>(), pagination.Page, pagination.Size, 0, 0, true);
            }
            var totalPages = (int)Math.Ceiling(totalCount / (double)pagination.Size);
            var isLastPage = pagination.Page >= totalPages;
            var serviceCategories = await query.Skip((pagination.Page - 1) * pagination.Size).Take(pagination.Size).ToListAsync();
            return new PaginationResponse<ServiceCategory>(serviceCategories, pagination.Page, pagination.Size, totalPages, totalCount, isLastPage);
        }, nameof(GetPaginatedServiceCategories));
    }

    public async Task<ServiceCategoryResponse> GetServiceCategoryById(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var serviceCategory = await _context.ServiceCategories.FindAsync(id);
            serviceCategory.ThrowIfNotFound("ServiceCategory", id);
            return _mapper.Map<ServiceCategoryResponse>(serviceCategory);
        }, nameof(GetServiceCategoryById));
    }

    public async Task<ServiceCategoryResponse> CreateServiceCategory(CreateServiceCategoryRequest request)
    {
        var serviceCategory = _mapper.Map<ServiceCategory>(request);
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _context.ServiceCategories.Add(serviceCategory);
            await _context.SaveChangesAsync();
            return _mapper.Map<ServiceCategoryResponse>(serviceCategory);
        }, nameof(CreateServiceCategory));
    }

    public async Task<ServiceCategoryResponse> UpdateServiceCategory(int id, CreateServiceCategoryRequest request)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var serviceCategory = await _context.ServiceCategories.FindAsync(id);
            serviceCategory.ThrowIfNotFound("ServiceCategory", id);
            // Map fields from request to serviceCategory
            serviceCategory.Name = request.Name;
            serviceCategory.Description = request.Description;
            _context.ServiceCategories.Update(serviceCategory);
            await _context.SaveChangesAsync();
            return _mapper.Map<ServiceCategoryResponse>(serviceCategory);
        }, nameof(UpdateServiceCategory));
    }

    public async Task<ServiceCategoryResponse> DeleteServiceCategory(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var serviceCategory = await _context.ServiceCategories.FindAsync(id);
            serviceCategory.ThrowIfNotFound("ServiceCategory", id);
            _context.ServiceCategories.Remove(serviceCategory);
            await _context.SaveChangesAsync();
            return _mapper.Map<ServiceCategoryResponse>(serviceCategory);
        }, nameof(DeleteServiceCategory));
    }
}

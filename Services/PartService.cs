using AutoMapper;
using car_repair.Models.DTO;
using Microsoft.EntityFrameworkCore;

class PartService : IPartService
{
    private CarRepairDbContext _context;
    private ILogger<PartService> _logger;
    private readonly IExceptionHandlingService _exceptionHandling;
    private IMapper _mapper;

    public PartService(CarRepairDbContext context, ILogger<PartService> logger, IExceptionHandlingService exceptionHandling, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _exceptionHandling = exceptionHandling;
        _mapper = mapper;
    }

    public async Task<PaginationResponse<Part>> GetPaginatedParts(PaginationRequest pagination)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            pagination.ThrowIfNull(nameof(pagination));
            if (pagination.Page < 1) pagination.Page = 1;
            if (pagination.Size < 1 || pagination.Size > 100) pagination.Size = 10;
            IQueryable<Part> query = _context.Parts.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(pagination.Query))
            {
                string searchTerm = pagination.Query.Trim().ToLowerInvariant();
                query = query.Where(p => (p.Name != null && EF.Functions.Like(p.Name.ToLower(), $"%{searchTerm}%")));
            }
            var totalCount = await query.CountAsync();
            if (totalCount == 0)
            {
                return new PaginationResponse<Part>(new List<Part>(), pagination.Page, pagination.Size, 0, 0, true);
            }
            var totalPages = (int)Math.Ceiling(totalCount / (double)pagination.Size);
            var isLastPage = pagination.Page >= totalPages;
            var parts = await query.Skip((pagination.Page - 1) * pagination.Size).Take(pagination.Size).ToListAsync();
            return new PaginationResponse<Part>(parts, pagination.Page, pagination.Size, totalPages, totalCount, isLastPage);
        }, nameof(GetPaginatedParts));
    }

    public async Task<PartResponse> GetPartById(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var part = await _context.Parts.FindAsync(id);
            part.ThrowIfNotFound("Part", id);
            return _mapper.Map<PartResponse>(part);
        }, nameof(GetPartById));
    }

    public async Task<PartResponse> CreatePart(CreatePartRequest request)
    {
        var part = _mapper.Map<Part>(request);
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _context.Parts.Add(part);
            await _context.SaveChangesAsync();
            return _mapper.Map<PartResponse>(part);
        }, nameof(CreatePart));
    }

    public async Task<PartResponse> UpdatePart(int id, CreatePartRequest request)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var part = await _context.Parts.FindAsync(id);
            part.ThrowIfNotFound("Part", id);
            // Map fields from request to part
            part.PartNumber = request.PartNumber;
            part.Name = request.Name;
            part.Description = request.Description;
            part.Brand = request.Brand;
            part.Cost = request.Cost;
            part.SellPrice = request.SellPrice;
            part.QuantityInStock = request.QuantityInStock;
            part.MinimumStock = request.MinimumStock;
            part.Location = request.Location;
            part.IsActive = request.IsActive;
            _context.Parts.Update(part);
            await _context.SaveChangesAsync();
            return _mapper.Map<PartResponse>(part);
        }, nameof(UpdatePart));
    }

    public async Task<PartResponse> DeletePart(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var part = await _context.Parts.FindAsync(id);
            part.ThrowIfNotFound("Part", id);
            _context.Parts.Remove(part);
            await _context.SaveChangesAsync();
            return _mapper.Map<PartResponse>(part);
        }, nameof(DeletePart));
    }
}

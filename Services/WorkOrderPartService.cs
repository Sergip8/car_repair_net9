using AutoMapper;
using car_repair.Models.DTO;
using Microsoft.EntityFrameworkCore;

class WorkOrderPartService : IWorkOrderPartService
{
    private CarRepairDbContext _context;
    private ILogger<WorkOrderPartService> _logger;
    private readonly IExceptionHandlingService _exceptionHandling;
    private IMapper _mapper;

    public WorkOrderPartService(CarRepairDbContext context, ILogger<WorkOrderPartService> logger, IExceptionHandlingService exceptionHandling, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _exceptionHandling = exceptionHandling;
        _mapper = mapper;
    }

    public async Task<PaginationResponse<WorkOrderPart>> GetPaginatedWorkOrderParts(PaginationRequest pagination)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            pagination.ThrowIfNull(nameof(pagination));
            if (pagination.Page < 1) pagination.Page = 1;
            if (pagination.Size < 1 || pagination.Size > 100) pagination.Size = 10;
            IQueryable<WorkOrderPart> query = _context.WorkOrderParts.AsNoTracking();
            var totalCount = await query.CountAsync();
            if (totalCount == 0)
            {
                return new PaginationResponse<WorkOrderPart>(new List<WorkOrderPart>(), pagination.Page, pagination.Size, 0, 0, true);
            }
            var totalPages = (int)Math.Ceiling(totalCount / (double)pagination.Size);
            var isLastPage = pagination.Page >= totalPages;
            var workOrderParts = await query.Skip((pagination.Page - 1) * pagination.Size).Take(pagination.Size).ToListAsync();
            return new PaginationResponse<WorkOrderPart>(workOrderParts, pagination.Page, pagination.Size, totalPages, totalCount, isLastPage);
        }, nameof(GetPaginatedWorkOrderParts));
    }

    public async Task<WorkOrderPartResponse> GetWorkOrderPartById(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var workOrderPart = await _context.WorkOrderParts.FindAsync(id);
            workOrderPart.ThrowIfNotFound("WorkOrderPart", id);
            return _mapper.Map<WorkOrderPartResponse>(workOrderPart);
        }, nameof(GetWorkOrderPartById));
    }

    public async Task<WorkOrderPartResponse> CreateWorkOrderPart(CreateWorkOrderPartRequest request)
    {
        var workOrderPart = _mapper.Map<WorkOrderPart>(request);
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _context.WorkOrderParts.Add(workOrderPart);
            await _context.SaveChangesAsync();
            return _mapper.Map<WorkOrderPartResponse>(workOrderPart);
        }, nameof(CreateWorkOrderPart));
    }

    public async Task<WorkOrderPartResponse> UpdateWorkOrderPart(int id, CreateWorkOrderPartRequest request)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var workOrderPart = await _context.WorkOrderParts.FindAsync(id);
            workOrderPart.ThrowIfNotFound("WorkOrderPart", id);
            workOrderPart.WorkOrderId = request.WorkOrderId;
            workOrderPart.PartId = request.PartId;
            workOrderPart.QuantityUsed = request.QuantityUsed;
            workOrderPart.UnitPrice = request.UnitPrice;
            workOrderPart.Notes = request.Notes;
            _context.WorkOrderParts.Update(workOrderPart);
            await _context.SaveChangesAsync();
            return _mapper.Map<WorkOrderPartResponse>(workOrderPart);
        }, nameof(UpdateWorkOrderPart));
    }

    public async Task<WorkOrderPartResponse> DeleteWorkOrderPart(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var workOrderPart = await _context.WorkOrderParts.FindAsync(id);
            workOrderPart.ThrowIfNotFound("WorkOrderPart", id);
            _context.WorkOrderParts.Remove(workOrderPart);
            await _context.SaveChangesAsync();
            return _mapper.Map<WorkOrderPartResponse>(workOrderPart);
        }, nameof(DeleteWorkOrderPart));
    }
}

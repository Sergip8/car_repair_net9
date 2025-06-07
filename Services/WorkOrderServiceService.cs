using AutoMapper;
using car_repair.Models.DTO;
using Microsoft.EntityFrameworkCore;

class WorkOrderServiceService : IWorkOrderServiceService
{
    private CarRepairDbContext _context;
    private ILogger<WorkOrderServiceService> _logger;
    private readonly IExceptionHandlingService _exceptionHandling;
    private IMapper _mapper;

    public WorkOrderServiceService(CarRepairDbContext context, ILogger<WorkOrderServiceService> logger, IExceptionHandlingService exceptionHandling, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _exceptionHandling = exceptionHandling;
        _mapper = mapper;
    }

    public async Task<PaginationResponse<WorkOrderServiceEnt>> GetPaginatedWorkOrderServices(PaginationRequest pagination)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            pagination.ThrowIfNull(nameof(pagination));
            if (pagination.Page < 1) pagination.Page = 1;
            if (pagination.Size < 1 || pagination.Size > 100) pagination.Size = 10;
            IQueryable<WorkOrderServiceEnt> query = _context.WorkOrderServices.AsNoTracking();
            var totalCount = await query.CountAsync();
            if (totalCount == 0)
            {
                return new PaginationResponse<WorkOrderServiceEnt>(new List<WorkOrderServiceEnt>(), pagination.Page, pagination.Size, 0, 0, true);
            }
            var totalPages = (int)Math.Ceiling(totalCount / (double)pagination.Size);
            var isLastPage = pagination.Page >= totalPages;
            var workOrderServices = await query.Skip((pagination.Page - 1) * pagination.Size).Take(pagination.Size).ToListAsync();
            return new PaginationResponse<WorkOrderServiceEnt>(workOrderServices, pagination.Page, pagination.Size, totalPages, totalCount, isLastPage);
        }, nameof(GetPaginatedWorkOrderServices));
    }

    public async Task<WorkOrderServiceResponse> GetWorkOrderServiceById(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var workOrderService = await _context.WorkOrderServices.FindAsync(id);
            workOrderService.ThrowIfNotFound("WorkOrderService", id);
            return _mapper.Map<WorkOrderServiceResponse>(workOrderService);
        }, nameof(GetWorkOrderServiceById));
    }

    public async Task<WorkOrderServiceResponse> CreateWorkOrderService(CreateWorkOrderServiceRequest request)
    {
        var workOrderService = _mapper.Map<WorkOrderServiceEnt>(request);
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _context.WorkOrderServices.Add(workOrderService);
            await _context.SaveChangesAsync();
            return _mapper.Map<WorkOrderServiceResponse>(workOrderService);
        }, nameof(CreateWorkOrderService));
    }

    public async Task<WorkOrderServiceResponse> UpdateWorkOrderService(int id, CreateWorkOrderServiceRequest request)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var workOrderService = await _context.WorkOrderServices.FindAsync(id);
            workOrderService.ThrowIfNotFound("WorkOrderService", id);
            workOrderService.WorkOrderId = request.WorkOrderId;
            workOrderService.ServiceId = request.ServiceId;
            workOrderService.Price = request.Price;
            workOrderService.Quantity = request.Quantity;
            workOrderService.Notes = request.Notes;
            _context.WorkOrderServices.Update(workOrderService);
            await _context.SaveChangesAsync();
            return _mapper.Map<WorkOrderServiceResponse>(workOrderService);
        }, nameof(UpdateWorkOrderService));
    }

    public async Task<WorkOrderServiceResponse> DeleteWorkOrderService(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var workOrderService = await _context.WorkOrderServices.FindAsync(id);
            workOrderService.ThrowIfNotFound("WorkOrderService", id);
            _context.WorkOrderServices.Remove(workOrderService);
            await _context.SaveChangesAsync();
            return _mapper.Map<WorkOrderServiceResponse>(workOrderService);
        }, nameof(DeleteWorkOrderService));
    }
}

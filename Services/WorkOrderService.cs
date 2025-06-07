using AutoMapper;
using car_repair.Models.DTO;
using Microsoft.EntityFrameworkCore;

class WorkOrderService: IWorkOrderService
{
    private CarRepairDbContext _context;
    private ILogger<WorkOrderServiceEnt> _logger;
    private readonly IExceptionHandlingService _exceptionHandling;
    private IMapper _mapper;

    public WorkOrderService(CarRepairDbContext context, ILogger<WorkOrderServiceEnt> logger, IExceptionHandlingService exceptionHandling, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _exceptionHandling = exceptionHandling;
        _mapper = mapper;
    }

    public async Task<PaginationResponse<WorkOrder>> GetPaginatedWorkOrders(PaginationRequest pagination)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            pagination.ThrowIfNull(nameof(pagination));
            if (pagination.Page < 1) pagination.Page = 1;
            if (pagination.Size < 1 || pagination.Size > 100) pagination.Size = 10;
            IQueryable<WorkOrder> query = _context.WorkOrders.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(pagination.Query))
            {
                string searchTerm = pagination.Query.Trim().ToLowerInvariant();
                query = query.Where(w => (w.WorkOrderNumber != null && EF.Functions.Like(w.WorkOrderNumber.ToLower(), $"%{searchTerm}%")));
            }
            var totalCount = await query.CountAsync();
            if (totalCount == 0)
            {
                return new PaginationResponse<WorkOrder>(new List<WorkOrder>(), pagination.Page, pagination.Size, 0, 0, true);
            }
            var totalPages = (int)Math.Ceiling(totalCount / (double)pagination.Size);
            var isLastPage = pagination.Page >= totalPages;
            var workOrders = await query.Skip((pagination.Page - 1) * pagination.Size).Take(pagination.Size).ToListAsync();
            return new PaginationResponse<WorkOrder>(workOrders, pagination.Page, pagination.Size, totalPages, totalCount, isLastPage);
        }, nameof(GetPaginatedWorkOrders));
    }

    public async Task<WorkOrderResponse> GetWorkOrderById(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var workOrder = await _context.WorkOrders.FindAsync(id);
            workOrder.ThrowIfNotFound("WorkOrder", id);
            return _mapper.Map<WorkOrderResponse>(workOrder);
        }, nameof(GetWorkOrderById));
    }

    public async Task<WorkOrderResponse> CreateWorkOrder(CreateWorkOrderRequest request)
    {
        
        var workOrder = _mapper.Map<WorkOrder>(request);
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _context.WorkOrders.Add(workOrder);
            await _context.SaveChangesAsync();
            return _mapper.Map<WorkOrderResponse>(workOrder);
        }, nameof(CreateWorkOrder));
    }

    public async Task<WorkOrderResponse> UpdateWorkOrder(int id, CreateWorkOrderRequest request)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            
            var workOrder = await _context.WorkOrders.FindAsync(id);
            workOrder.ThrowIfNotFound("WorkOrder", id);
            // Map fields from request to workOrder
            workOrder.WorkOrderNumber = request.WorkOrderNumber;
            workOrder.Status = request.Status;
            workOrder.Description = request.Description;
            workOrder.DiagnosisNotes = request.DiagnosisNotes;
            workOrder.CompletionNotes = request.CompletionNotes;
            workOrder.StartedAt = request.StartedAt;
            workOrder.CompletedAt = request.CompletedAt;
            workOrder.LaborHours = request.LaborHours;
            workOrder.TotalCost = request.TotalCost;
            workOrder.VehicleId = request.VehicleId;
            workOrder.AppointmentId = request.AppointmentId;
            workOrder.AssignedEmployeeId = request.AssignedEmployeeId;
            _context.WorkOrders.Update(workOrder);
            await _context.SaveChangesAsync();
            return _mapper.Map<WorkOrderResponse>(workOrder);
        }, nameof(UpdateWorkOrder));
    }

    public async Task<WorkOrderResponse> DeleteWorkOrder(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var workOrder = await _context.WorkOrders.FindAsync(id);
            workOrder.ThrowIfNotFound("WorkOrder", id);
            _context.WorkOrders.Remove(workOrder);
            await _context.SaveChangesAsync();
            return _mapper.Map<WorkOrderResponse>(workOrder);
        }, nameof(DeleteWorkOrder));
    }
}

using car_repair.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class WorkOrderController : ControllerBase
{
    private readonly IWorkOrderService _workOrderService;
    private readonly IExceptionHandlingService _exceptionHandling;
    private readonly ILogger<WorkOrderController> _logger;
    public WorkOrderController(IWorkOrderService workOrderService, IExceptionHandlingService exceptionHandling, ILogger<WorkOrderController> logger)
    {
        _workOrderService = workOrderService;
        _exceptionHandling = exceptionHandling;
        _logger = logger;
    }

    [HttpPost("Paginated")]
    [ProducesResponseType(typeof(PaginationResponse<WorkOrder>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllWorkOrders([FromBody] PaginationRequest pagination)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Getting paginated work orders with parameters: {@Pagination}", pagination);
            var result = await _workOrderService.GetPaginatedWorkOrders(pagination);
            _logger.LogInformation("Successfully retrieved {Pages} pages of work orders out of {Total} total", result.TotalPages, result.TotalElements);
            return Ok(result);
        }, nameof(GetAllWorkOrders));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(WorkOrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetWorkOrderById(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Getting work order with ID: {WorkOrderId}", id);
            var workOrder = await _workOrderService.GetWorkOrderById(id);
            _logger.LogInformation("Successfully retrieved work order: {WorkOrderId}", workOrder.Id);
            return Ok(workOrder);
        }, nameof(GetWorkOrderById));
    }

    [HttpPost]
    [ProducesResponseType(typeof(WorkOrderResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateWorkOrder([FromBody] CreateWorkOrderRequest createWorkOrderRequest)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Creating new work order: {@CreateWorkOrderRequest}", createWorkOrderRequest);
            var workOrder = await _workOrderService.CreateWorkOrder(createWorkOrderRequest);
            _logger.LogInformation("Successfully created work order with ID: {WorkOrderId}", workOrder.Id);
            return CreatedAtAction(nameof(GetWorkOrderById), new { id = workOrder.Id }, workOrder);
        }, nameof(CreateWorkOrder));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(WorkOrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateWorkOrder(int id, [FromBody] CreateWorkOrderRequest updateWorkOrderRequest)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Updating work order {WorkOrderId} with data: {@UpdateWorkOrderRequest}", id, updateWorkOrderRequest);
            var workOrder = await _workOrderService.UpdateWorkOrder(id, updateWorkOrderRequest);
            _logger.LogInformation("Successfully updated work order: {WorkOrderId}", workOrder.Id);
            return Ok(workOrder);
        }, nameof(UpdateWorkOrder));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteWorkOrder(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Deleting work order with ID: {WorkOrderId}", id);
            await _workOrderService.DeleteWorkOrder(id);
            _logger.LogInformation("Successfully deleted work order: {WorkOrderId}", id);
            return NoContent();
        }, nameof(DeleteWorkOrder));
    }
}

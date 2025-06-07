using car_repair.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class WorkOrderServiceController : ControllerBase
{
    private readonly IWorkOrderServiceService _workOrderServiceService;
    private readonly IExceptionHandlingService _exceptionHandling;
    private readonly ILogger<WorkOrderServiceController> _logger;
    public WorkOrderServiceController(IWorkOrderServiceService workOrderServiceService, IExceptionHandlingService exceptionHandling, ILogger<WorkOrderServiceController> logger)
    {
        _workOrderServiceService = workOrderServiceService;
        _exceptionHandling = exceptionHandling;
        _logger = logger;
    }

    [HttpPost("Paginated")]
    [ProducesResponseType(typeof(PaginationResponse<WorkOrderService>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllWorkOrderServices([FromBody] PaginationRequest pagination)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Getting paginated work order services with parameters: {@Pagination}", pagination);
            var result = await _workOrderServiceService.GetPaginatedWorkOrderServices(pagination);
            _logger.LogInformation("Successfully retrieved {Pages} pages of work order services out of {Total} total", result.TotalPages, result.TotalElements);
            return Ok(result);
        }, nameof(GetAllWorkOrderServices));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(WorkOrderServiceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetWorkOrderServiceById(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Getting work order service with ID: {WorkOrderServiceId}", id);
            var workOrderService = await _workOrderServiceService.GetWorkOrderServiceById(id);
            _logger.LogInformation("Successfully retrieved work order service: {WorkOrderServiceId}", workOrderService.Id);
            return Ok(workOrderService);
        }, nameof(GetWorkOrderServiceById));
    }

    [HttpPost]
    [ProducesResponseType(typeof(WorkOrderServiceResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateWorkOrderService([FromBody] CreateWorkOrderServiceRequest createWorkOrderServiceRequest)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Creating new work order service: {@CreateWorkOrderServiceRequest}", createWorkOrderServiceRequest);
            var workOrderService = await _workOrderServiceService.CreateWorkOrderService(createWorkOrderServiceRequest);
            _logger.LogInformation("Successfully created work order service with ID: {WorkOrderServiceId}", workOrderService.Id);
            return CreatedAtAction(nameof(GetWorkOrderServiceById), new { id = workOrderService.Id }, workOrderService);
        }, nameof(CreateWorkOrderService));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(WorkOrderServiceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateWorkOrderService(int id, [FromBody] CreateWorkOrderServiceRequest updateWorkOrderServiceRequest)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Updating work order service {WorkOrderServiceId} with data: {@UpdateWorkOrderServiceRequest}", id, updateWorkOrderServiceRequest);
            var workOrderService = await _workOrderServiceService.UpdateWorkOrderService(id, updateWorkOrderServiceRequest);
            _logger.LogInformation("Successfully updated work order service: {WorkOrderServiceId}", workOrderService.Id);
            return Ok(workOrderService);
        }, nameof(UpdateWorkOrderService));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteWorkOrderService(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Deleting work order service with ID: {WorkOrderServiceId}", id);
            await _workOrderServiceService.DeleteWorkOrderService(id);
            _logger.LogInformation("Successfully deleted work order service: {WorkOrderServiceId}", id);
            return NoContent();
        }, nameof(DeleteWorkOrderService));
    }
}

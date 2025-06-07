using car_repair.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class WorkOrderPartController : ControllerBase
{
    private readonly IWorkOrderPartService _workOrderPartService;
    private readonly IExceptionHandlingService _exceptionHandling;
    private readonly ILogger<WorkOrderPartController> _logger;
    public WorkOrderPartController(IWorkOrderPartService workOrderPartService, IExceptionHandlingService exceptionHandling, ILogger<WorkOrderPartController> logger)
    {
        _workOrderPartService = workOrderPartService;
        _exceptionHandling = exceptionHandling;
        _logger = logger;
    }

    [HttpPost("Paginated")]
    [ProducesResponseType(typeof(PaginationResponse<WorkOrderPart>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllWorkOrderParts([FromBody] PaginationRequest pagination)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Getting paginated work order parts with parameters: {@Pagination}", pagination);
            var result = await _workOrderPartService.GetPaginatedWorkOrderParts(pagination);
            _logger.LogInformation("Successfully retrieved {Pages} pages of work order parts out of {Total} total", result.TotalPages, result.TotalElements);
            return Ok(result);
        }, nameof(GetAllWorkOrderParts));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(WorkOrderPartResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetWorkOrderPartById(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Getting work order part with ID: {WorkOrderPartId}", id);
            var workOrderPart = await _workOrderPartService.GetWorkOrderPartById(id);
            _logger.LogInformation("Successfully retrieved work order part: {WorkOrderPartId}", workOrderPart.Id);
            return Ok(workOrderPart);
        }, nameof(GetWorkOrderPartById));
    }

    [HttpPost]
    [ProducesResponseType(typeof(WorkOrderPartResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateWorkOrderPart([FromBody] CreateWorkOrderPartRequest createWorkOrderPartRequest)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Creating new work order part: {@CreateWorkOrderPartRequest}", createWorkOrderPartRequest);
            var workOrderPart = await _workOrderPartService.CreateWorkOrderPart(createWorkOrderPartRequest);
            _logger.LogInformation("Successfully created work order part with ID: {WorkOrderPartId}", workOrderPart.Id);
            return CreatedAtAction(nameof(GetWorkOrderPartById), new { id = workOrderPart.Id }, workOrderPart);
        }, nameof(CreateWorkOrderPart));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(WorkOrderPartResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateWorkOrderPart(int id, [FromBody] CreateWorkOrderPartRequest updateWorkOrderPartRequest)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Updating work order part {WorkOrderPartId} with data: {@UpdateWorkOrderPartRequest}", id, updateWorkOrderPartRequest);
            var workOrderPart = await _workOrderPartService.UpdateWorkOrderPart(id, updateWorkOrderPartRequest);
            _logger.LogInformation("Successfully updated work order part: {WorkOrderPartId}", workOrderPart.Id);
            return Ok(workOrderPart);
        }, nameof(UpdateWorkOrderPart));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteWorkOrderPart(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Deleting work order part with ID: {WorkOrderPartId}", id);
            await _workOrderPartService.DeleteWorkOrderPart(id);
            _logger.LogInformation("Successfully deleted work order part: {WorkOrderPartId}", id);
            return NoContent();
        }, nameof(DeleteWorkOrderPart));
    }
}

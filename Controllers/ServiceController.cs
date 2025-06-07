using car_repair.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ServiceController : ControllerBase
{
    private readonly IServiceService _serviceService;
    private readonly IExceptionHandlingService _exceptionHandling;
    private readonly ILogger<ServiceController> _logger;
    public ServiceController(IServiceService serviceService, IExceptionHandlingService exceptionHandling, ILogger<ServiceController> logger)
    {
        _serviceService = serviceService;
        _exceptionHandling = exceptionHandling;
        _logger = logger;
    }

    [HttpPost("Paginated")]
    [ProducesResponseType(typeof(PaginationResponse<Service>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllServices([FromBody] PaginationRequest pagination)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Getting paginated services with parameters: {@Pagination}", pagination);
            var result = await _serviceService.GetPaginatedServices(pagination);
            _logger.LogInformation("Successfully retrieved {Pages} pages of services out of {Total} total", result.TotalPages, result.TotalElements);
            return Ok(result);
        }, nameof(GetAllServices));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetServiceById(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Getting service with ID: {ServiceId}", id);
            var service = await _serviceService.GetServiceById(id);
            _logger.LogInformation("Successfully retrieved service: {ServiceId}", service.Id);
            return Ok(service);
        }, nameof(GetServiceById));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateService([FromBody] CreateServiceRequest createServiceRequest)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Creating new service: {@CreateServiceRequest}", createServiceRequest);
            var service = await _serviceService.CreateService(createServiceRequest);
            _logger.LogInformation("Successfully created service with ID: {ServiceId}", service.Id);
            return CreatedAtAction(nameof(GetServiceById), new { id = service.Id }, service);
        }, nameof(CreateService));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateService(int id, [FromBody] CreateServiceRequest updateServiceRequest)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Updating service {ServiceId} with data: {@UpdateServiceRequest}", id, updateServiceRequest);
            var service = await _serviceService.UpdateService(id, updateServiceRequest);
            _logger.LogInformation("Successfully updated service: {ServiceId}", service.Id);
            return Ok(service);
        }, nameof(UpdateService));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteService(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Deleting service with ID: {ServiceId}", id);
            await _serviceService.DeleteService(id);
            _logger.LogInformation("Successfully deleted service: {ServiceId}", id);
            return NoContent();
        }, nameof(DeleteService));
    }
}

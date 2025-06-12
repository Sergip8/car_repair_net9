using car_repair.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class BrandController : ControllerBase
{
    private readonly IBrandService _brandService;
    private readonly IExceptionHandlingService _exceptionHandling;
    private readonly ILogger<ServiceController> _logger;
    public BrandController(IBrandService brandService, IExceptionHandlingService exceptionHandling, ILogger<ServiceController> logger)
    {
        _brandService = brandService;
        _exceptionHandling = exceptionHandling;
        _logger = logger;
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
            var brand = await _brandService.GetByIdAsync(id);
            _logger.LogInformation("Successfully retrieved service: {ServiceId}", brand.Id);
            return Ok(brand);
        }, nameof(GetServiceById));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateService([FromBody] Brand brandDto)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Creating new service: {@CreateServiceRequest}", brandDto);
            var service = await _brandService.CreateAsync(brandDto);
            _logger.LogInformation("Successfully created service with ID: {ServiceId}", service.Id);
            return CreatedAtAction(nameof(GetServiceById), new { id = service.Id }, service);
        }, nameof(CreateService));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateService(int id, [FromBody] Brand updateBrand)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Updating service {ServiceId} with data: {@UpdateServiceRequest}", id, updateBrand);
            var service = await _brandService.UpdateAsync(id, updateBrand);
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
            await _brandService.DeleteAsync(id);
            _logger.LogInformation("Successfully deleted service: {ServiceId}", id);
            return NoContent();
        }, nameof(DeleteService));
    }

     [HttpGet("Home")]
    [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetServiceHome()
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Get Home Services");
            var service = await _brandService.GetBrandsHome();
            Console.WriteLine(JsonSerializer.Serialize(service));
            _logger.LogInformation("services retrieved");
            return Ok(service);
        }, nameof(GetServiceHome));
    }
}

using car_repair.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ServiceCategoryController : ControllerBase
{
    private readonly IServiceCategoryService _serviceCategoryService;
    private readonly IExceptionHandlingService _exceptionHandling;
    private readonly ILogger<ServiceCategoryController> _logger;
    public ServiceCategoryController(IServiceCategoryService serviceCategoryService, IExceptionHandlingService exceptionHandling, ILogger<ServiceCategoryController> logger)
    {
        _serviceCategoryService = serviceCategoryService;
        _exceptionHandling = exceptionHandling;
        _logger = logger;
    }

    [HttpPost("Paginated")]
    [ProducesResponseType(typeof(PaginationResponse<ServiceCategory>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllServiceCategories([FromBody] PaginationRequest pagination)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Getting paginated service categories with parameters: {@Pagination}", pagination);
            var result = await _serviceCategoryService.GetPaginatedServiceCategories(pagination);
            _logger.LogInformation("Successfully retrieved {Pages} pages of service categories out of {Total} total", result.TotalPages, result.TotalElements);
            return Ok(result);
        }, nameof(GetAllServiceCategories));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ServiceCategoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetServiceCategoryById(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Getting service category with ID: {ServiceCategoryId}", id);
            var serviceCategory = await _serviceCategoryService.GetServiceCategoryById(id);
            _logger.LogInformation("Successfully retrieved service category: {ServiceCategoryId}", serviceCategory.Id);
            return Ok(serviceCategory);
        }, nameof(GetServiceCategoryById));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ServiceCategoryResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateServiceCategory([FromBody] CreateServiceCategoryRequest createServiceCategoryRequest)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Creating new service category: {@CreateServiceCategoryRequest}", createServiceCategoryRequest);
            var serviceCategory = await _serviceCategoryService.CreateServiceCategory(createServiceCategoryRequest);
            _logger.LogInformation("Successfully created service category with ID: {ServiceCategoryId}", serviceCategory.Id);
            return CreatedAtAction(nameof(GetServiceCategoryById), new { id = serviceCategory.Id }, serviceCategory);
        }, nameof(CreateServiceCategory));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ServiceCategoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateServiceCategory(int id, [FromBody] CreateServiceCategoryRequest updateServiceCategoryRequest)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Updating service category {ServiceCategoryId} with data: {@UpdateServiceCategoryRequest}", id, updateServiceCategoryRequest);
            var serviceCategory = await _serviceCategoryService.UpdateServiceCategory(id, updateServiceCategoryRequest);
            _logger.LogInformation("Successfully updated service category: {ServiceCategoryId}", serviceCategory.Id);
            return Ok(serviceCategory);
        }, nameof(UpdateServiceCategory));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteServiceCategory(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Deleting service category with ID: {ServiceCategoryId}", id);
            await _serviceCategoryService.DeleteServiceCategory(id);
            _logger.LogInformation("Successfully deleted service category: {ServiceCategoryId}", id);
            return NoContent();
        }, nameof(DeleteServiceCategory));
    }
}

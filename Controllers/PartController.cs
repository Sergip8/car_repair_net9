using car_repair.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class PartController : ControllerBase
{
    private readonly IPartService _partService;
    private readonly IExceptionHandlingService _exceptionHandling;
    private readonly ILogger<PartController> _logger;
    public PartController(IPartService partService, IExceptionHandlingService exceptionHandling, ILogger<PartController> logger)
    {
        _partService = partService;
        _exceptionHandling = exceptionHandling;
        _logger = logger;
    }

    [HttpPost("Paginated")]
    [ProducesResponseType(typeof(PaginationResponse<Part>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllParts([FromBody] PaginationRequest pagination)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Getting paginated parts with parameters: {@Pagination}", pagination);
            var result = await _partService.GetPaginatedParts(pagination);
            _logger.LogInformation("Successfully retrieved {Pages} pages of parts out of {Total} total", result.TotalPages, result.TotalElements);
            return Ok(result);
        }, nameof(GetAllParts));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PartResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPartById(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Getting part with ID: {PartId}", id);
            var part = await _partService.GetPartById(id);
            _logger.LogInformation("Successfully retrieved part: {PartId}", part.Id);
            return Ok(part);
        }, nameof(GetPartById));
    }

    [HttpPost]
    [ProducesResponseType(typeof(PartResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreatePart([FromBody] CreatePartRequest createPartRequest)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Creating new part: {@CreatePartRequest}", createPartRequest);
            var part = await _partService.CreatePart(createPartRequest);
            _logger.LogInformation("Successfully created part with ID: {PartId}", part.Id);
            return CreatedAtAction(nameof(GetPartById), new { id = part.Id }, part);
        }, nameof(CreatePart));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(PartResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdatePart(int id, [FromBody] CreatePartRequest updatePartRequest)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Updating part {PartId} with data: {@UpdatePartRequest}", id, updatePartRequest);
            var part = await _partService.UpdatePart(id, updatePartRequest);
            _logger.LogInformation("Successfully updated part: {PartId}", part.Id);
            return Ok(part);
        }, nameof(UpdatePart));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeletePart(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Deleting part with ID: {PartId}", id);
            await _partService.DeletePart(id);
            _logger.LogInformation("Successfully deleted part: {PartId}", id);
            return NoContent();
        }, nameof(DeletePart));
    }
}

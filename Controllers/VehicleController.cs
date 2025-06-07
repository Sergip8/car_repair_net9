using car_repair.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class VehicleController : ControllerBase
{
    private readonly IVehicleService _vehicleService;
    private readonly IExceptionHandlingService _exceptionHandling;
    private readonly ILogger<VehicleController> _logger;
    public VehicleController(IVehicleService vehicleService, IExceptionHandlingService exceptionHandling, ILogger<VehicleController> logger)
    {
        _vehicleService = vehicleService;
        _exceptionHandling = exceptionHandling;
        _logger = logger;
    }

     /// <summary>
    /// Gets paginated list of vehicles with optional filtering and sorting
    /// </summary>
    /// <param name="pagination">Pagination parameters including page, size, query, sort, and direction</param>
    /// <returns>Paginated response containing vehicles and pagination metadata</returns>
    [HttpPost]
    [Route("Paginated")]
    [ProducesResponseType(typeof(PaginationResponse<Vehicle>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllVehicles([FromBody] PaginationRequest pagination)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Getting paginated vehicles with parameters: {@Pagination}", pagination);
            
            var result = await _vehicleService.GetPaginatedVehicles(pagination);
            
            _logger.LogInformation("Successfully retrieved {Pages} pages of vehicles out of {Total} total", 
                result.TotalPages, result.TotalElements);
            
            return Ok(result);
        }, nameof(GetAllVehicles));
    }

    /// <summary>
    /// Gets a vehicle by its ID
    /// </summary>
    /// <param name="id">Vehicle ID</param>
    /// <returns>Vehicle details</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Vehicle), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetVehicleById(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Getting vehicle with ID: {VehicleId}", id);
            
            var vehicle = await _vehicleService.GetVehicleById(id);
            
            _logger.LogInformation("Successfully retrieved vehicle: {VehicleId}", vehicle.Id);
            
            return Ok(vehicle);
        }, nameof(GetVehicleById));
    }

    /// <summary>
    /// Creates a new vehicle
    /// </summary>
    /// <param name="createVehicleRequest">Vehicle creation data</param>
    /// <returns>Created vehicle</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Vehicle), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateVehicle([FromForm] VehicleRequestForm vehicleRequestForm)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            VehicleRequest vehicleRequest = new VehicleRequest(vehicleRequestForm.Data, vehicleRequestForm.Image);
            _logger.LogInformation("Creating new vehicle: {@CreateVehicleRequest}", vehicleRequestForm.Data);
            
            var vehicle = await _vehicleService.CreateVehicle(vehicleRequest);
            
            _logger.LogInformation("Successfully created vehicle with ID: {VehicleId}", vehicle.Id);
            
            return CreatedAtAction(nameof(GetVehicleById), new { id = vehicle.Id }, vehicle);
        }, nameof(CreateVehicle));
    }

    /// <summary>
    /// Updates an existing vehicle
    /// </summary>
    /// <param name="id">Vehicle ID</param>
    /// <param name="updateVehicleRequest">Vehicle update data</param>
    /// <returns>Updated vehicle</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Vehicle), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateVehicle(int id, [FromBody] VehicleRequest updateVehicleRequest)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Updating vehicle {VehicleId} with data: {@UpdateVehicleRequest}", 
                id, updateVehicleRequest);
            
            var vehicle = await _vehicleService.UpdateVehicle(id, updateVehicleRequest);
            
            _logger.LogInformation("Successfully updated vehicle: {VehicleId}", vehicle.Id);
            
            return Ok(vehicle);
        }, nameof(UpdateVehicle));
    }

    /// <summary>
    /// Deletes a vehicle
    /// </summary>
    /// <param name="id">Vehicle ID</param>
    /// <returns>No content if successful</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteVehicle(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Deleting vehicle with ID: {VehicleId}", id);
            
            await _vehicleService.DeleteVehicle(id);
            
            _logger.LogInformation("Successfully deleted vehicle: {VehicleId}", id);
            
            return NoContent();
        }, nameof(DeleteVehicle));
    // }

    /// <summary>
    /// Gets vehicle statistics
    /// </summary>
    /// <returns>Vehicle statistics</returns>
    // [HttpGet("statistics")]
    // [ProducesResponseType(typeof(VehicleStatistics), StatusCodes.Status200OK)]
    // [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    // public async Task<IActionResult> GetVehicleStatistics()
    // {
    //     return await _exceptionHandling.ExecuteAsync(async () =>
    //     {
    //         _logger.LogInformation("Getting vehicle statistics");
            
    //         var statistics = await _vehicleService.GetVehicleStatistics();
            
    //         _logger.LogInformation("Successfully retrieved vehicle statistics");
            
    //         return Ok(statistics);
    //     }, nameof(GetVehicleStatistics));
     }
}
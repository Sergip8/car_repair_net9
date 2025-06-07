using car_repair.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;
    private readonly IExceptionHandlingService _exceptionHandling;
    private readonly ILogger<AppointmentController> _logger;
    public AppointmentController(IAppointmentService appointmentService, IExceptionHandlingService exceptionHandling, ILogger<AppointmentController> logger)
    {
        _appointmentService = appointmentService;
        _exceptionHandling = exceptionHandling;
        _logger = logger;
    }

    [HttpPost("Paginated")]
    [ProducesResponseType(typeof(PaginationResponse<Appointment>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllAppointments([FromBody] PaginationRequest pagination)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Getting paginated appointments with parameters: {@Pagination}", pagination);
            var result = await _appointmentService.GetPaginatedAppointments(pagination);
            _logger.LogInformation("Successfully retrieved {Pages} pages of appointments out of {Total} total", result.TotalPages, result.TotalElements);
            return Ok(result);
        }, nameof(GetAllAppointments));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AppointmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAppointmentById(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Getting appointment with ID: {AppointmentId}", id);
            var appointment = await _appointmentService.GetAppointmentById(id);
            _logger.LogInformation("Successfully retrieved appointment: {AppointmentId}", appointment.Id);
            return Ok(appointment);
        }, nameof(GetAppointmentById));
    }

    [HttpPost]
    [ProducesResponseType(typeof(AppointmentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentRequest createAppointmentRequest)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Creating new appointment: {@CreateAppointmentRequest}", createAppointmentRequest);
            var appointment = await _appointmentService.CreateAppointment(createAppointmentRequest);
            _logger.LogInformation("Successfully created appointment with ID: {AppointmentId}", appointment.Id);
            return CreatedAtAction(nameof(GetAppointmentById), new { id = appointment.Id }, appointment);
        }, nameof(CreateAppointment));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(AppointmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAppointment(int id, [FromBody] CreateAppointmentRequest updateAppointmentRequest)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Updating appointment {AppointmentId} with data: {@UpdateAppointmentRequest}", id, updateAppointmentRequest);
            var appointment = await _appointmentService.UpdateAppointment(id, updateAppointmentRequest);
            _logger.LogInformation("Successfully updated appointment: {AppointmentId}", appointment.Id);
            return Ok(appointment);
        }, nameof(UpdateAppointment));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAppointment(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Deleting appointment with ID: {AppointmentId}", id);
            await _appointmentService.DeleteAppointment(id);
            _logger.LogInformation("Successfully deleted appointment: {AppointmentId}", id);
            return NoContent();
        }, nameof(DeleteAppointment));
    }
}

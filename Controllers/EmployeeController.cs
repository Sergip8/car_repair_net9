using car_repair.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IExceptionHandlingService _exceptionHandling;
    private readonly ILogger<EmployeeController> _logger;
    public EmployeeController(IEmployeeService employeeService, IExceptionHandlingService exceptionHandling, ILogger<EmployeeController> logger)
    {
        _employeeService = employeeService;
        _exceptionHandling = exceptionHandling;
        _logger = logger;
    }

    [HttpPost("Paginated")]
    [ProducesResponseType(typeof(PaginationResponse<Employee>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllEmployees([FromBody] PaginationRequest pagination)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Getting paginated employees with parameters: {@Pagination}", pagination);
            var result = await _employeeService.GetPaginatedEmployees(pagination);
            _logger.LogInformation("Successfully retrieved {Pages} pages of employees out of {Total} total", result.TotalPages, result.TotalElements);
            return Ok(result);
        }, nameof(GetAllEmployees));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(EmployeeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetEmployeeById(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Getting employee with ID: {EmployeeId}", id);
            var employee = await _employeeService.GetEmployeeById(id);
            _logger.LogInformation("Successfully retrieved employee: {EmployeeId}", employee.Id);
            return Ok(employee);
        }, nameof(GetEmployeeById));
    }

    [HttpPost]
    [ProducesResponseType(typeof(EmployeeResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeRequest createEmployeeRequest)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Creating new employee: {@CreateEmployeeRequest}", createEmployeeRequest);
            var employee = await _employeeService.CreateEmployee(createEmployeeRequest);
            _logger.LogInformation("Successfully created employee with ID: {EmployeeId}", employee.Id);
            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, employee);
        }, nameof(CreateEmployee));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(EmployeeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateEmployee(int id, [FromBody] CreateEmployeeRequest updateEmployeeRequest)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Updating employee {EmployeeId} with data: {@UpdateEmployeeRequest}", id, updateEmployeeRequest);
            var employee = await _employeeService.UpdateEmployee(id, updateEmployeeRequest);
            _logger.LogInformation("Successfully updated employee: {EmployeeId}", employee.Id);
            return Ok(employee);
        }, nameof(UpdateEmployee));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Deleting employee with ID: {EmployeeId}", id);
            await _employeeService.DeleteEmployee(id);
            _logger.LogInformation("Successfully deleted employee: {EmployeeId}", id);
            return NoContent();
        }, nameof(DeleteEmployee));
    }
}

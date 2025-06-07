using car_repair.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly IExceptionHandlingService _exceptionHandling;
    private readonly ILogger<CustomerController> _logger;
    public CustomerController(ICustomerService customerService, IExceptionHandlingService exceptionHandling, ILogger<CustomerController> logger)
    {
        _customerService = customerService;
        _exceptionHandling = exceptionHandling;
        _logger = logger;
    }

    [HttpPost("Paginated")]
    [ProducesResponseType(typeof(PaginationResponse<Customer>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllCustomers([FromBody] PaginationRequest pagination)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Getting paginated customers with parameters: {@Pagination}", pagination);
            var result = await _customerService.GetPaginatedCustomers(pagination);
            _logger.LogInformation("Successfully retrieved {Pages} pages of customers out of {Total} total", result.TotalPages, result.TotalElements);
            return Ok(result);
        }, nameof(GetAllCustomers));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCustomerById(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Getting customer with ID: {CustomerId}", id);
            var customer = await _customerService.GetCustomerById(id);
            _logger.LogInformation("Successfully retrieved customer: {CustomerId}", customer.Id);
            return Ok(customer);
        }, nameof(GetCustomerById));
    }

    [HttpPost]
    [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest createCustomerRequest)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Creating new customer: {@CreateCustomerRequest}", createCustomerRequest);
            var customer = await _customerService.CreateCustomer(createCustomerRequest);
            _logger.LogInformation("Successfully created customer with ID: {CustomerId}", customer.Id);
            return CreatedAtAction(nameof(GetCustomerById), new { id = customer.Id }, customer);
        }, nameof(CreateCustomer));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CreateCustomerRequest updateCustomerRequest)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Updating customer {CustomerId} with data: {@UpdateCustomerRequest}", id, updateCustomerRequest);
            var customer = await _customerService.UpdateCustomer(id, updateCustomerRequest);
            _logger.LogInformation("Successfully updated customer: {CustomerId}", customer.Id);
            return Ok(customer);
        }, nameof(UpdateCustomer));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Deleting customer with ID: {CustomerId}", id);
            await _customerService.DeleteCustomer(id);
            _logger.LogInformation("Successfully deleted customer: {CustomerId}", id);
            return NoContent();
        }, nameof(DeleteCustomer));
    }
}

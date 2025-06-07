using car_repair.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly IExceptionHandlingService _exceptionHandling;
    private readonly ILogger<PaymentController> _logger;
    public PaymentController(IPaymentService paymentService, IExceptionHandlingService exceptionHandling, ILogger<PaymentController> logger)
    {
        _paymentService = paymentService;
        _exceptionHandling = exceptionHandling;
        _logger = logger;
    }

    [HttpPost("Paginated")]
    [ProducesResponseType(typeof(PaginationResponse<Payment>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllPayments([FromBody] PaginationRequest pagination)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Getting paginated payments with parameters: {@Pagination}", pagination);
            var result = await _paymentService.GetPaginatedPayments(pagination);
            _logger.LogInformation("Successfully retrieved {Pages} pages of payments out of {Total} total", result.TotalPages, result.TotalElements);
            return Ok(result);
        }, nameof(GetAllPayments));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PaymentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPaymentById(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Getting payment with ID: {PaymentId}", id);
            var payment = await _paymentService.GetPaymentById(id);
            _logger.LogInformation("Successfully retrieved payment: {PaymentId}", payment.Id);
            return Ok(payment);
        }, nameof(GetPaymentById));
    }

    [HttpPost]
    [ProducesResponseType(typeof(PaymentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequest createPaymentRequest)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Creating new payment: {@CreatePaymentRequest}", createPaymentRequest);
            var payment = await _paymentService.CreatePayment(createPaymentRequest);
            _logger.LogInformation("Successfully created payment with ID: {PaymentId}", payment.Id);
            return CreatedAtAction(nameof(GetPaymentById), new { id = payment.Id }, payment);
        }, nameof(CreatePayment));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(PaymentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdatePayment(int id, [FromBody] CreatePaymentRequest updatePaymentRequest)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Updating payment {PaymentId} with data: {@UpdatePaymentRequest}", id, updatePaymentRequest);
            var payment = await _paymentService.UpdatePayment(id, updatePaymentRequest);
            _logger.LogInformation("Successfully updated payment: {PaymentId}", payment.Id);
            return Ok(payment);
        }, nameof(UpdatePayment));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeletePayment(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Deleting payment with ID: {PaymentId}", id);
            await _paymentService.DeletePayment(id);
            _logger.LogInformation("Successfully deleted payment: {PaymentId}", id);
            return NoContent();
        }, nameof(DeletePayment));
    }
}

using car_repair.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;
    private readonly IExceptionHandlingService _exceptionHandling;
    private readonly ILogger<InvoiceController> _logger;
    public InvoiceController(IInvoiceService invoiceService, IExceptionHandlingService exceptionHandling, ILogger<InvoiceController> logger)
    {
        _invoiceService = invoiceService;
        _exceptionHandling = exceptionHandling;
        _logger = logger;
    }

    [HttpPost("Paginated")]
    [ProducesResponseType(typeof(PaginationResponse<Invoice>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllInvoices([FromBody] PaginationRequest pagination)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Getting paginated invoices with parameters: {@Pagination}", pagination);
            var result = await _invoiceService.GetPaginatedInvoices(pagination);
            _logger.LogInformation("Successfully retrieved {Pages} pages of invoices out of {Total} total", result.TotalPages, result.TotalElements);
            return Ok(result);
        }, nameof(GetAllInvoices));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(InvoiceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetInvoiceById(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Getting invoice with ID: {InvoiceId}", id);
            var invoice = await _invoiceService.GetInvoiceById(id);
            _logger.LogInformation("Successfully retrieved invoice: {InvoiceId}", invoice.Id);
            return Ok(invoice);
        }, nameof(GetInvoiceById));
    }

    [HttpPost]
    [ProducesResponseType(typeof(InvoiceResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceRequest createInvoiceRequest)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Creating new invoice: {@CreateInvoiceRequest}", createInvoiceRequest);
            var invoice = await _invoiceService.CreateInvoice(createInvoiceRequest);
            _logger.LogInformation("Successfully created invoice with ID: {InvoiceId}", invoice.Id);
            return CreatedAtAction(nameof(GetInvoiceById), new { id = invoice.Id }, invoice);
        }, nameof(CreateInvoice));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(InvoiceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateInvoice(int id, [FromBody] CreateInvoiceRequest updateInvoiceRequest)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Updating invoice {InvoiceId} with data: {@UpdateInvoiceRequest}", id, updateInvoiceRequest);
            var invoice = await _invoiceService.UpdateInvoice(id, updateInvoiceRequest);
            _logger.LogInformation("Successfully updated invoice: {InvoiceId}", invoice.Id);
            return Ok(invoice);
        }, nameof(UpdateInvoice));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteInvoice(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _logger.LogInformation("Deleting invoice with ID: {InvoiceId}", id);
            await _invoiceService.DeleteInvoice(id);
            _logger.LogInformation("Successfully deleted invoice: {InvoiceId}", id);
            return NoContent();
        }, nameof(DeleteInvoice));
    }
}

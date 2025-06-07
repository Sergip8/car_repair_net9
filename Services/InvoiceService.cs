using AutoMapper;
using car_repair.Models.DTO;
using Microsoft.EntityFrameworkCore;

class InvoiceService : IInvoiceService
{
    private CarRepairDbContext _context;
    private ILogger<InvoiceService> _logger;
    private readonly IExceptionHandlingService _exceptionHandling;
    private IMapper _mapper;

    public InvoiceService(CarRepairDbContext context, ILogger<InvoiceService> logger, IExceptionHandlingService exceptionHandling, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _exceptionHandling = exceptionHandling;
        _mapper = mapper;
    }

    public async Task<PaginationResponse<Invoice>> GetPaginatedInvoices(PaginationRequest pagination)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            pagination.ThrowIfNull(nameof(pagination));
            if (pagination.Page < 1) pagination.Page = 1;
            if (pagination.Size < 1 || pagination.Size > 100) pagination.Size = 10;
            IQueryable<Invoice> query = _context.Invoices.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(pagination.Query))
            {
                string searchTerm = pagination.Query.Trim().ToLowerInvariant();
                query = query.Where(i => (i.InvoiceNumber != null && EF.Functions.Like(i.InvoiceNumber.ToLower(), $"%{searchTerm}%")));
            }
            var totalCount = await query.CountAsync();
            if (totalCount == 0)
            {
                return new PaginationResponse<Invoice>(new List<Invoice>(), pagination.Page, pagination.Size, 0, 0, true);
            }
            var totalPages = (int)Math.Ceiling(totalCount / (double)pagination.Size);
            var isLastPage = pagination.Page >= totalPages;
            var invoices = await query.Skip((pagination.Page - 1) * pagination.Size).Take(pagination.Size).ToListAsync();
            return new PaginationResponse<Invoice>(invoices, pagination.Page, pagination.Size, totalPages, totalCount, isLastPage);
        }, nameof(GetPaginatedInvoices));
    }

    public async Task<InvoiceResponse> GetInvoiceById(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var invoice = await _context.Invoices.FindAsync(id);
            invoice.ThrowIfNotFound("Invoice", id);
            return _mapper.Map<InvoiceResponse>(invoice);
        }, nameof(GetInvoiceById));
    }

    public async Task<InvoiceResponse> CreateInvoice(CreateInvoiceRequest request)
    {
        
        var invoice = _mapper.Map<Invoice>(request);
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
            return _mapper.Map<InvoiceResponse>(invoice);
        }, nameof(CreateInvoice));
    }

    public async Task<InvoiceResponse> UpdateInvoice(int id, CreateInvoiceRequest request)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
                throw new BusinessLogicException("La factura no existe.");
            if (invoice.Status != InvoiceStatus.Pending)
                throw new BusinessLogicException("Solo se pueden registrar pagos en facturas pendientes.");
            invoice.ThrowIfNotFound("Invoice", id);
            // Map fields from request to invoice
            invoice.InvoiceNumber = request.InvoiceNumber;
            invoice.InvoiceDate = request.InvoiceDate;
            invoice.DueDate = request.DueDate;
            invoice.Status = request.Status;
            invoice.SubTotal = request.SubTotal;
            invoice.TaxRate = request.TaxRate;
            invoice.TaxAmount = request.TaxAmount;
            invoice.TotalAmount = request.TotalAmount;
            invoice.AmountPaid = request.AmountPaid;
            invoice.Notes = request.Notes;
            invoice.CustomerId = request.CustomerId;
            invoice.WorkOrderId = request.WorkOrderId;
            _context.Invoices.Update(invoice);
            await _context.SaveChangesAsync();
            return _mapper.Map<InvoiceResponse>(invoice);
        }, nameof(UpdateInvoice));
    }

    public async Task<InvoiceResponse> DeleteInvoice(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var invoice = await _context.Invoices.FindAsync(id);
            invoice.ThrowIfNotFound("Invoice", id);
            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();
            return _mapper.Map<InvoiceResponse>(invoice);
        }, nameof(DeleteInvoice));
    }
}

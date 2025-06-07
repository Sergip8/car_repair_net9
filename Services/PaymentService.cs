using AutoMapper;
using car_repair.Models.DTO;
using Microsoft.EntityFrameworkCore;

class PaymentService : IPaymentService
{
    private CarRepairDbContext _context;
    private ILogger<PaymentService> _logger;
    private readonly IExceptionHandlingService _exceptionHandling;
    private IMapper _mapper;

    public PaymentService(CarRepairDbContext context, ILogger<PaymentService> logger, IExceptionHandlingService exceptionHandling, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _exceptionHandling = exceptionHandling;
        _mapper = mapper;
    }

    public async Task<PaginationResponse<Payment>> GetPaginatedPayments(PaginationRequest pagination)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            pagination.ThrowIfNull(nameof(pagination));
            if (pagination.Page < 1) pagination.Page = 1;
            if (pagination.Size < 1 || pagination.Size > 100) pagination.Size = 10;
            IQueryable<Payment> query = _context.Payments.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(pagination.Query))
            {
                string searchTerm = pagination.Query.Trim().ToLowerInvariant();
                query = query.Where(p => (p.ReferenceNumber != null && EF.Functions.Like(p.ReferenceNumber.ToLower(), $"%{searchTerm}%")));
            }
            var totalCount = await query.CountAsync();
            if (totalCount == 0)
            {
                return new PaginationResponse<Payment>(new List<Payment>(), pagination.Page, pagination.Size, 0, 0, true);
            }
            var totalPages = (int)Math.Ceiling(totalCount / (double)pagination.Size);
            var isLastPage = pagination.Page >= totalPages;
            var payments = await query.Skip((pagination.Page - 1) * pagination.Size).Take(pagination.Size).ToListAsync();
            return new PaginationResponse<Payment>(payments, pagination.Page, pagination.Size, totalPages, totalCount, isLastPage);
        }, nameof(GetPaginatedPayments));
    }

    public async Task<PaymentResponse> GetPaymentById(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var payment = await _context.Payments.FindAsync(id);
            payment.ThrowIfNotFound("Payment", id);
            return _mapper.Map<PaymentResponse>(payment);
        }, nameof(GetPaymentById));
    }

    public async Task<PaymentResponse> CreatePayment(CreatePaymentRequest request)
    {
        var payment = _mapper.Map<Payment>(request);
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return _mapper.Map<PaymentResponse>(payment);
        }, nameof(CreatePayment));
    }

    public async Task<PaymentResponse> UpdatePayment(int id, CreatePaymentRequest request)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var payment = await _context.Payments.FindAsync(id);
            payment.ThrowIfNotFound("Payment", id);
            // Map fields from request to payment
            payment.Amount = request.Amount;
            payment.PaymentDate = request.PaymentDate;
            payment.PaymentMethod = request.PaymentMethod;
            payment.ReferenceNumber = request.ReferenceNumber;
            payment.Notes = request.Notes;
            payment.InvoiceId = request.InvoiceId;
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
            return _mapper.Map<PaymentResponse>(payment);
        }, nameof(UpdatePayment));
    }

    public async Task<PaymentResponse> DeletePayment(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var payment = await _context.Payments.FindAsync(id);
            payment.ThrowIfNotFound("Payment", id);
            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return _mapper.Map<PaymentResponse>(payment);
        }, nameof(DeletePayment));
    }
}

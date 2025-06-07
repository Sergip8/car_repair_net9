namespace car_repair.Models.DTO
{
    public record CreatePaymentRequest(
        decimal Amount,
        DateTime PaymentDate,
        PaymentMethod PaymentMethod,
        string? ReferenceNumber,
        string? Notes,
        int InvoiceId
    );

    public record PaymentResponse(
        int Id,
        decimal Amount,
        DateTime PaymentDate,
        PaymentMethod PaymentMethod,
        string? ReferenceNumber,
        string? Notes,
        int InvoiceId
    );
}

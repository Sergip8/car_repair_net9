namespace car_repair.Models.DTO
{
    public record CreateInvoiceRequest(
        string InvoiceNumber,
        DateTime InvoiceDate,
        DateTime? DueDate,
        InvoiceStatus Status,
        decimal SubTotal,
        decimal TaxRate,
        decimal TaxAmount,
        decimal TotalAmount,
        decimal AmountPaid,
        string? Notes,
        int CustomerId,
        int? WorkOrderId
    );

    public record InvoiceResponse(
        int Id,
        string InvoiceNumber,
        DateTime InvoiceDate,
        DateTime? DueDate,
        InvoiceStatus Status,
        decimal SubTotal,
        decimal TaxRate,
        decimal TaxAmount,
        decimal TotalAmount,
        decimal AmountPaid,
        string? Notes,
        int CustomerId,
        int? WorkOrderId
    );
}

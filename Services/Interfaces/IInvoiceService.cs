using car_repair.Models.DTO;

public interface IInvoiceService
{
    Task<PaginationResponse<Invoice>> GetPaginatedInvoices(PaginationRequest pagination);
    Task<InvoiceResponse> CreateInvoice(CreateInvoiceRequest request);
    Task<InvoiceResponse> UpdateInvoice(int id, CreateInvoiceRequest request);
    Task<InvoiceResponse> GetInvoiceById(int id);
    Task<InvoiceResponse> DeleteInvoice(int id);
}

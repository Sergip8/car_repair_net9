using car_repair.Models.DTO;

public interface IPaymentService
{
    Task<PaginationResponse<Payment>> GetPaginatedPayments(PaginationRequest pagination);
    Task<PaymentResponse> CreatePayment(CreatePaymentRequest request);
    Task<PaymentResponse> UpdatePayment(int id, CreatePaymentRequest request);
    Task<PaymentResponse> GetPaymentById(int id);
    Task<PaymentResponse> DeletePayment(int id);
}

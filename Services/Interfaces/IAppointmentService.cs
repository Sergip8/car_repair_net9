using car_repair.Models.DTO;

public interface IAppointmentService
{
    Task<PaginationResponse<Appointment>> GetPaginatedAppointments(PaginationRequest pagination);
    Task<AppointmentResponse> CreateAppointment(CreateAppointmentRequest request);
    Task<AppointmentResponse> UpdateAppointment(int id, CreateAppointmentRequest request);
    Task<AppointmentResponse> GetAppointmentById(int id);
    Task<AppointmentResponse> DeleteAppointment(int id);
}

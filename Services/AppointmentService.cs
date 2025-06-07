using AutoMapper;
using car_repair.Models.DTO;
using Microsoft.EntityFrameworkCore;

class AppointmentService : IAppointmentService
{
    private CarRepairDbContext _context;
    private ILogger<AppointmentService> _logger;
    private readonly IExceptionHandlingService _exceptionHandling;
    private IMapper _mapper;

    public AppointmentService(CarRepairDbContext context, ILogger<AppointmentService> logger, IExceptionHandlingService exceptionHandling, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _exceptionHandling = exceptionHandling;
        _mapper = mapper;
    }

    public async Task<PaginationResponse<Appointment>> GetPaginatedAppointments(PaginationRequest pagination)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            pagination.ThrowIfNull(nameof(pagination));
            if (pagination.Page < 1) pagination.Page = 1;
            if (pagination.Size < 1 || pagination.Size > 100) pagination.Size = 10;
            IQueryable<Appointment> query = _context.Appointments.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(pagination.Query))
            {
                string searchTerm = pagination.Query.Trim().ToLowerInvariant();
                query = query.Where(a => (a.Description != null && EF.Functions.Like(a.Description.ToLower(), $"%{searchTerm}%")));
            }
            var totalCount = await query.CountAsync();
            if (totalCount == 0)
            {
                return new PaginationResponse<Appointment>(new List<Appointment>(), pagination.Page, pagination.Size, 0, 0, true);
            }
            var totalPages = (int)Math.Ceiling(totalCount / (double)pagination.Size);
            var isLastPage = pagination.Page >= totalPages;
            var appointments = await query.Skip((pagination.Page - 1) * pagination.Size).Take(pagination.Size).ToListAsync();
            return new PaginationResponse<Appointment>(appointments, pagination.Page, pagination.Size, totalPages, totalCount, isLastPage);
        }, nameof(GetPaginatedAppointments));
    }

    public async Task<AppointmentResponse> GetAppointmentById(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var appointment = await _context.Appointments.FindAsync(id);
            appointment.ThrowIfNotFound("Appointment", id);
            return _mapper.Map<AppointmentResponse>(appointment);
        }, nameof(GetAppointmentById));
    }

    public async Task<AppointmentResponse> CreateAppointment(CreateAppointmentRequest request)
    {
        var appointment = _mapper.Map<Appointment>(request);
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return _mapper.Map<AppointmentResponse>(appointment);
        }, nameof(CreateAppointment));
    }

    public async Task<AppointmentResponse> UpdateAppointment(int id, CreateAppointmentRequest request)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var appointment = await _context.Appointments.FindAsync(id);
            appointment.ThrowIfNotFound("Appointment", id);
            // Map fields from request to appointment
            appointment.ScheduledDateTime = request.ScheduledDateTime;
            appointment.EstimatedDurationMinutes = request.EstimatedDurationMinutes;
    
            appointment.Description = request.Description;
            appointment.CustomerNotes = request.CustomerNotes;
            appointment.InternalNotes = request.InternalNotes;
            appointment.CustomerId = request.CustomerId;
            appointment.VehicleId = request.VehicleId;
            appointment.AssignedEmployeeId = request.AssignedEmployeeId;
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
            return _mapper.Map<AppointmentResponse>(appointment);
        }, nameof(UpdateAppointment));
    }

    public async Task<AppointmentResponse> DeleteAppointment(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var appointment = await _context.Appointments.FindAsync(id);
            appointment.ThrowIfNotFound("Appointment", id);
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return _mapper.Map<AppointmentResponse>(appointment);
        }, nameof(DeleteAppointment));
    }
}

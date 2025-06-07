
using System.Net;
using AutoMapper;
using car_repair.Models.DTO;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

class VehicleService : IVehicleService
{
    private CarRepairDbContext _context;
    private ILogger<VehicleService> _logger;
    private readonly IExceptionHandlingService _exceptionHandling;
    private IMapper _mapper;

    private Cloudinary _cloudinary;
    private CloudinarySettings _cloudinarySettings;

    public VehicleService(CarRepairDbContext context, ILogger<VehicleService> logger, IExceptionHandlingService exceptionHandling, IMapper mapper, IOptions<CloudinarySettings> cloudinarySettings)
    {
        _cloudinarySettings = cloudinarySettings.Value;
        _context = context;
        _logger = logger;
        _exceptionHandling = exceptionHandling;
        _mapper = mapper;
        _cloudinary = new Cloudinary(_cloudinarySettings.CloudinaryUrl);
        _cloudinary.Api.Secure = true;
    }

    public async Task<PaginationResponse<Vehicle>> GetPaginatedVehicles(PaginationRequest pagination)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            // Input validation using extension methods
            pagination.ThrowIfNull(nameof(pagination));

            if (pagination.Page < 1)
                pagination.Page = 1;

            if (pagination.Size < 1 || pagination.Size > 100)
                pagination.Size = 10;

            // Base query
            IQueryable<Vehicle> query = _context.Vehicles.AsNoTracking();

            // Apply filtering
            if (!string.IsNullOrWhiteSpace(pagination.Query))
            {
                string searchTerm = pagination.Query.Trim().ToLowerInvariant();
                query = query.Where(v =>
                    (v.Model != null && EF.Functions.Like(v.Model.ToLower(), $"%{searchTerm}%")) ||
                    (v.Make != null && EF.Functions.Like(v.Make.ToLower(), $"%{searchTerm}%"))
                );
            }

            // Apply sorting
            query = ApplySorting(query, pagination.Sort, pagination.Direction);

            // Get total count
            var totalCount = await query.CountAsync();

            if (totalCount == 0)
            {
                return new PaginationResponse<Vehicle>(
                    new List<Vehicle>(),
                    pagination.Page,
                    pagination.Size,
                    0,
                    0,
                    true
                );
            }

            // Apply pagination
            var totalPages = (int)Math.Ceiling(totalCount / (double)pagination.Size);
            var isLastPage = pagination.Page >= totalPages;

            var vehicles = await query
                .Skip((pagination.Page - 1) * pagination.Size)
                .Take(pagination.Size)
                .ToListAsync();

            return new PaginationResponse<Vehicle>(
                vehicles,
                pagination.Page,
                pagination.Size,
                totalPages,
                totalCount,
                isLastPage
            );
        }, nameof(GetPaginatedVehicles));
    }

    public async Task<VehicleResponse> GetVehicleById(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            vehicle.ThrowIfNotFound("Vehicle", id);
            return _mapper.Map<VehicleResponse>(vehicle);
        }, nameof(GetVehicleById));
    }

    private static IQueryable<Vehicle> ApplySorting(IQueryable<Vehicle> query, string sortField, string direction)
    {
        if (string.IsNullOrWhiteSpace(sortField))
            sortField = "id";

        if (string.IsNullOrWhiteSpace(direction))
            direction = "asc";

        var isAscending = direction.Equals("asc", StringComparison.OrdinalIgnoreCase);

        return sortField.ToLowerInvariant() switch
        {
            "model" => isAscending
                ? query.OrderBy(v => v.Model ?? string.Empty)
                : query.OrderByDescending(v => v.Model ?? string.Empty),
            "make" or "brand" => isAscending
                ? query.OrderBy(v => v.Make ?? string.Empty)
                : query.OrderByDescending(v => v.Make ?? string.Empty),
            _ => isAscending
                ? query.OrderBy(v => v.Id)
                : query.OrderByDescending(v => v.Id)
        };
    }


    // Alternative: Use extension method for better reusability
    public async Task<VehicleResponse> CreateVehicle(VehicleRequest createVehicleRequest)
    {
        var urlImage = Helpers.UploadImage(createVehicleRequest.Image, _cloudinary);
        var exists = await _context.Vehicles.AnyAsync(v => v.VIN == createVehicleRequest.VIN);
    if (exists)
        throw new BusinessLogicException("Ya existe un vehículo con ese VIN.");
        var vehicle = _mapper.Map<Vehicle>(createVehicleRequest);
        vehicle.ImageUrl = urlImage;
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();
            return _mapper.Map<VehicleResponse>(vehicle);
        }, nameof(CreateVehicle));
    }

    public async Task<VehicleResponse> UpdateVehicle(int id, VehicleRequest updateVehicleRequest)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {

            var vehicle = await _context.Vehicles.FindAsync(id);
            vehicle.ThrowIfNotFound("Vehicle", id);


            vehicle.VIN = updateVehicleRequest.VIN;
            vehicle.Make = updateVehicleRequest.Make;
            vehicle.Model = updateVehicleRequest.Model;
            vehicle.Year = updateVehicleRequest.Year;
            // Add other properties here

            _context.Vehicles.Update(vehicle);
            await _context.SaveChangesAsync();
            return _mapper.Map<VehicleResponse>(vehicle);
        }, nameof(UpdateVehicle));
    }
    public async Task<VehicleResponse> DeleteVehicle(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            vehicle.ThrowIfNotFound("Vehicle", id);

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            return _mapper.Map<VehicleResponse>(vehicle);
        }, nameof(DeleteVehicle));
    }
    
    
}
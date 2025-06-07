
using car_repair.Models.DTO;

public interface IVehicleService
{
    Task<PaginationResponse<Vehicle>> GetPaginatedVehicles(PaginationRequest pagination);
    Task<VehicleResponse> CreateVehicle(VehicleRequest createVehicleRequest);
    Task<VehicleResponse> UpdateVehicle(int id, VehicleRequest updateVehicleRequest);


    Task<VehicleResponse> GetVehicleById(int id);

    Task<VehicleResponse> DeleteVehicle(int id);
}
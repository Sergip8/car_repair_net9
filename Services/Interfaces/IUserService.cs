using car_repair.Models.DTO;
using System.Threading.Tasks;

public interface IUserService
{
    Task<PaginationResponse<UserResponse>> GetPaginatedUsers(PaginationRequest pagination);
    Task<UserResponse> GetUserById(int id);
    Task<UserResponse> CreateUser(CreateUserRequest request);
    Task<UserResponse> UpdateUser(int id, CreateUserRequest request);
    Task<UserResponse> DeleteUser(int id);
    Task<AuthResponse> Register(RegisterRequest request);
    Task<AuthResponse> Login(LoginRequest request);
}

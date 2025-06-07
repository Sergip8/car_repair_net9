using AutoMapper;
using car_repair.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


public class UserService : IUserService
{
    private readonly CarRepairDbContext _context;
    private readonly ILogger<UserService> _logger;
    private readonly IExceptionHandlingService _exceptionHandling;
    private readonly IMapper _mapper;
    private readonly SendGridSettings _sendGridSettings;
    private readonly IJwtService _jwtService;


    public UserService(CarRepairDbContext context, ILogger<UserService> logger, IExceptionHandlingService exceptionHandling, IMapper mapper, IOptions<SendGridSettings> sendGridSettings, IJwtService jwtService)
    {
        _context = context;
        _logger = logger;
        _exceptionHandling = exceptionHandling;
        _mapper = mapper;
        _jwtService = jwtService;
        _sendGridSettings = sendGridSettings.Value;
    }

    public async Task<PaginationResponse<UserResponse>> GetPaginatedUsers(PaginationRequest pagination)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            pagination.ThrowIfNull(nameof(pagination));
            if (pagination.Page < 1) pagination.Page = 1;
            if (pagination.Size < 1 || pagination.Size > 100) pagination.Size = 10;
            IQueryable<User> query = _context.Users.Include(u => u.Role).AsNoTracking();
            if (!string.IsNullOrWhiteSpace(pagination.Query))
            {
                string searchTerm = pagination.Query.Trim().ToLowerInvariant();
                query = query.Where(u => u.Username.ToLower().Contains(searchTerm) || u.Email.ToLower().Contains(searchTerm));
            }
            var totalCount = await query.CountAsync();
            if (totalCount == 0)
            {
                return new PaginationResponse<UserResponse>(new List<UserResponse>(), pagination.Page, pagination.Size, 0, 0, true);
            }
            var totalPages = (int)Math.Ceiling(totalCount / (double)pagination.Size);
            var isLastPage = pagination.Page >= totalPages;
            var users = await query
    .Select(u => new UserResponse {
        Id = u.Id,
        Username = u.Username,
        Email = u.Email,
        RoleId = u.RoleId,
        RoleName = u.Role.Name,
        CustomerId = u.CustomerId,
        EmployeeId = u.EmployeeId
    })
    .Skip((pagination.Page - 1) * pagination.Size)
    .Take(pagination.Size)
    .ToListAsync();
            
            return new PaginationResponse<UserResponse>(users, pagination.Page, pagination.Size, totalPages, totalCount, isLastPage);
        }, nameof(GetPaginatedUsers));
    }

    public async Task<UserResponse> GetUserById(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var user = await _context.Users
    .Where(u => u.Id == id)
    .Select(u => new UserResponse {
        Id = u.Id,
        Username = u.Username,
        Email = u.Email,
        RoleId = u.RoleId,
        RoleName = u.Role.Name,
        CustomerId = u.CustomerId,
        EmployeeId = u.EmployeeId
    })
    .FirstOrDefaultAsync();
            user.ThrowIfNotFound("User", id);
            return _mapper.Map<UserResponse>(user);
        }, nameof(GetUserById));
    }

    public async Task<UserResponse> CreateUser(CreateUserRequest request)
    {
        var user = _mapper.Map<User>(request);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return _mapper.Map<UserResponse>(user);
        }, nameof(CreateUser));
    }

    public async Task<UserResponse> UpdateUser(int id, CreateUserRequest request)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var user = await _context.Users.FindAsync(id);
            user.ThrowIfNotFound("User", id);
            user.Username = request.Username;
            user.Email = request.Email;
            user.RoleId = request.RoleId;
            if (!string.IsNullOrEmpty(request.Password))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.CustomerId = request.CustomerId;
            user.EmployeeId = request.EmployeeId;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return _mapper.Map<UserResponse>(user);
        }, nameof(UpdateUser));
    }

    public async Task<UserResponse> DeleteUser(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var user = await _context.Users.FindAsync(id);
            user.ThrowIfNotFound("User", id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return _mapper.Map<UserResponse>(user);
        }, nameof(DeleteUser));
    }

    public async Task<AuthResponse> Register(RegisterRequest request)
    {
        var exists = await _context.Users.AnyAsync(u => u.Email == request.Email || u.Username == request.Username);
        if (exists)
            throw new Exception("User already exists with this email or username");
        var user = _mapper.Map<User>(request);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        user.RoleId = request.RoleId;
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        var template = await _context.EmailTemplate.FirstOrDefaultAsync(e => e.Category == "Onboarding");
        Helpers.SendMail(_sendGridSettings, template, user.Email);
        user.Role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == user.RoleId);
        var token = _jwtService.GenerateToken(user.Id, request.Email, user.Role.Name);

        return new AuthResponse { Token = token, User = _mapper.Map<UserResponse>(user) };
    }

    public async Task<AuthResponse> Login(LoginRequest request)
    {
        var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new Exception("Invalid credentials");
        var token = _jwtService.GenerateToken(user.Id, request.Email, user.Role.Name);
        return new AuthResponse { Token = token, User = _mapper.Map<UserResponse>(user) };
    }

   
}

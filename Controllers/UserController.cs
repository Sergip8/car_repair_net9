using car_repair.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IExceptionHandlingService _exceptionHandling;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, IExceptionHandlingService exceptionHandling, ILogger<UserController> logger)
    {
        _userService = userService;
        _exceptionHandling = exceptionHandling;
        _logger = logger;
    }

    [HttpPost("Paginated")]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllUsers([FromBody] PaginationRequest pagination)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var result = await _userService.GetPaginatedUsers(pagination);
            return Ok(result);
        }, nameof(GetAllUsers));
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUserById(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var user = await _userService.GetUserById(id);
            return Ok(user);
        }, nameof(GetUserById));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var user = await _userService.CreateUser(request);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }, nameof(CreateUser));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] CreateUserRequest request)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var user = await _userService.UpdateUser(id, request);
            return Ok(user);
        }, nameof(UpdateUser));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            await _userService.DeleteUser(id);
            return NoContent();
        }, nameof(DeleteUser));
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var result = await _userService.Register(request);
            return Ok(result);
        }, nameof(Register));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        return await _exceptionHandling.ExecuteAsync(async () =>
        {
            var result = await _userService.Login(request);
            return Ok(result);
        }, nameof(Login));
    }
}

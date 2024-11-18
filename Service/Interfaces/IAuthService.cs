using Backend.Models.DTOs.Auth;

namespace Backend.Service.Interfaces;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterDto model);
    Task<string> LoginAsync(LoginDto model);
}
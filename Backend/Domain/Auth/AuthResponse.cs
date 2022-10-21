namespace Backend.Domain.Auth
{
    public record AuthResponse(bool Success, string[] Errors);
}
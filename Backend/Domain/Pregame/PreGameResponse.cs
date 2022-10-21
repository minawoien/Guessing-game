namespace Backend.Domain.Pregame
{
    public record PregameResponse<T>(bool Success, string[] Errors, T Data);
}
namespace Backend.Domain.Game
{
    public record GameServiceDataResponse<T>(bool Success, T Data, string[] Errors);
}
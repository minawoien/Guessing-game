namespace Backend.Domain.Result
{
    public record ResultServiceResponse<T>(bool Success, string[] Errors, T Result);
}
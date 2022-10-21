namespace Backend.Controllers
{
    public record RouteResponse<T>(T Data, string[] Errors);
}
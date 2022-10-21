using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Backend.Domain.Auth.Functions
{
    public static class UserDataFromCookie
    {
        public static int GetUserId(this HttpContext context)
        {
            var uid = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return string.IsNullOrWhiteSpace(uid) ? 0 : int.Parse(uid);
        }

        public static string GetUserName(this HttpContext context)
        {
            return context.User.FindFirstValue(ClaimTypes.Name);
        }
    }
}
using System;
using Microsoft.AspNetCore.Http;

namespace Backend.Controllers
{
    public static class HttpLogger
    {
        //Used for debugging the communication between the frontend and backend.
        public static void Log(this HttpContext context)
        {
            Console.WriteLine($"{context.Request.Method} " +
                              $"{context.Connection.LocalIpAddress} " +
                              $"{context.Connection.LocalPort} " +
                              $"{context.Request.Path} " +
                              $"{context.Connection.RemoteIpAddress} " +
                              $"{context.Connection.RemotePort} " +
                              $"{DateTime.Now}");
        }
    }
}
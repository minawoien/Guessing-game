using System;
using System.Collections.Generic;
using Backend.Domain.Auth.Functions;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Auth
{
    [ApiController]
    [Route("[controller]")]
    public class UserNameController : Controller
    {
        [HttpGet]
        public IActionResult GetAsync()
        {
            //HttpContext.Log();
            var userName = HttpContext.GetUserName();
            if (string.IsNullOrWhiteSpace(userName))
            {
                List<string> err = new();
                err.Add("Not logged in");
                return Ok(new RouteResponse<string>("", err.ToArray()));
            }

            return Ok(new RouteResponse<string>(userName, Array.Empty<string>()));
        }
    }
}
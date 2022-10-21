using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Backend.Controllers.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Backend.Domain.Auth.Pipelines
{
    public class LoginUser
    {
        public record Request(LogRegData LoginData) : IRequest<AuthResponse>;

        public class Handler : IRequestHandler<Request, AuthResponse>
        {
            private readonly UserManager<User> _userManager;
            private readonly SignInManager<User> _signInManager;

            public Handler(UserManager<User> userManager, SignInManager<User> signInManager)
            {
                _userManager = userManager;
                _signInManager = signInManager;
            }

            public async Task<AuthResponse> Handle(Request request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(request.LoginData.UserName);
                var err = new List<string>();
                err.Add("Username or password is wrong");
                if (user is null)
                {
                    return new AuthResponse(false, err.ToArray());
                }

                var result = await _signInManager.PasswordSignInAsync(user, request.LoginData.Password, false, false);
                if (!result.Succeeded)
                {
                    return new AuthResponse(false, err.ToArray());
                }

                return new AuthResponse(result.Succeeded, Array.Empty<string>());
            }
        }
    }
}
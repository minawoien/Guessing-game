using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Backend.Controllers.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Backend.Domain.Auth.Pipelines
{
    public class RegisterUser
    {
        public record Request(LogRegData RegisterData) : IRequest<AuthResponse>;

        public class Handler : IRequestHandler<Request, AuthResponse>
        {
            private readonly UserManager<User> _userManager;

            public Handler(UserManager<User> userManager)
            {
                _userManager = userManager;
            }

            public async Task<AuthResponse> Handle(Request request, CancellationToken cancellationToken)
            {
                var user = new User();
                user.UserName = request.RegisterData.UserName;
                var result = await _userManager.CreateAsync(user, request.RegisterData.Password);
                var errList = new List<string>();
                foreach (var err in result.Errors)
                {
                    errList.Add(err.Description);
                }

                return new AuthResponse(result.Succeeded, errList.ToArray());
            }
        }
    }
}
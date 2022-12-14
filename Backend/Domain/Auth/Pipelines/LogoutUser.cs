using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Backend.Domain.Auth.Pipelines
{
    public class LogoutUser
    {
        public record Request() : IRequest;

        public class Handler : IRequestHandler<Request, Unit>
        {
            private readonly SignInManager<User> _signInManager;

            public Handler(SignInManager<User> signInManager)
            {
                _signInManager = signInManager;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                await _signInManager.SignOutAsync();
                return Unit.Value;
            }
        }
    }
}
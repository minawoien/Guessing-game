using System.Threading;
using System.Threading.Tasks;
using Backend.Domain.Game.Services;
using Backend.Domain.Images.Events;
using MediatR;

namespace Backend.Domain.Game.Handlers
{
    public class FragmentListCreatedHandler : INotificationHandler<FragmentsListCreated>
    {
        private readonly IGameService _gameService;

        public FragmentListCreatedHandler(IGameService gameService)
        {
            _gameService = gameService;
        }

        public async Task Handle(FragmentsListCreated notification, CancellationToken cancellationToken)
        {
            await _gameService.InsertFragmentList(notification.GameId, notification.ImageId, notification.Label,
                notification.FragmentList);
        }
    }
}
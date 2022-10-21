using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Domain.Game.Events;
using Backend.Domain.Images.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Domain.Images.Handlers
{
    public class GameCreatedHandler : INotificationHandler<GameCreated>
    {
        private readonly GameContext _gameContext;

        public GameCreatedHandler(GameContext gameContext)
        {
            _gameContext = gameContext;
        }

        public async Task Handle(GameCreated notification, CancellationToken cancellationToken)
        {
            var max = _gameContext.Images.Count();
            var rnd = new Random();
            int id = rnd.Next(1, max);
            var img = await _gameContext.Images
                .Include(i => i.Fragments)
                .Include(i => i.Label)
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
            if (img is null)
            {
                img = await _gameContext.Images
                    .Include(i => i.Fragments)
                    .Include(i => i.Label)
                    .Where(i => i.Id == id)
                    .FirstOrDefaultAsync(cancellationToken);
            }

            var fileList = new List<string>();
            foreach (var file in img.Fragments)
            {
                fileList.Add($"/ImageFragment/{img.Id}/{file.FileName}");
            }

            img.Events.Add(new FragmentsListCreated(notification.GameId, img.Id, img.Label.Value, fileList));
            await _gameContext.SaveChangesAsync(cancellationToken);
            await _gameContext.RunEvents();
        }
    }
}
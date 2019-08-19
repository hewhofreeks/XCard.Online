using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XCard.Client.Features.Game.GameSessionUpdated
{
    public class GameSessionUpdatedAction: IRequest<GameState>
    {
        public Guid GameID { get; set; }
    }
}

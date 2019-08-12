using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XCard.Client.Features.Game.LoadGame
{
    public class LoadGameAction : IRequest<GameState>
    {
        public Guid UserID { get; set; }
        public Guid GameID { get; set; }
    }
}

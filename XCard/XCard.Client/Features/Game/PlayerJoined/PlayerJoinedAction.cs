using BlazorState;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace XCard.Client.Features.Game.PlayerJoined
{
    public class PlayerJoinedAction : IRequest<GameState>
    {
        public PlayerJoinedAction()
        {
            Id = Guid.NewGuid();
        }

        public string Username { get; set; }

        public Guid Id { get; }
    }

}

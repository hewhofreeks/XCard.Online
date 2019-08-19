using BlazorState;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XCard.Client.Features.Game;
using XCard.Client.Features.Game.GameSessionUpdated;
using XCard.Client.Features.Game.PlayerJoined;
using XCard.Shared;

namespace XCard.Client.Features.Game
{
    public partial class GameState
    {
        public class PlayerJoinedHandler : BlazorState.RequestHandler<PlayerJoinedAction, GameState>
        {
            private readonly IGameHubClient _gameHubClient;
            private readonly IMediator _mediator;

            public PlayerJoinedHandler(IStore aStore, IGameHubClient gameHubClient, IMediator mediator) : base(aStore)
            {
                _gameHubClient = gameHubClient;
                _mediator = mediator;
            }

            public override async Task<GameState> Handle(PlayerJoinedAction request, CancellationToken aCancellationToken)
            {
                var state = this.Store.GetState<GameState>();

                var connection = await _gameHubClient.GetHubConnection();
                await connection.InvokeAsync<GameSession>("JoinGame", request.Username, request.GameID.ToString());

                await this._mediator.Send(new GameSessionUpdatedAction { GameID = request.GameID });

                state.HasJoinedGame = true;

                return state;
            }

        }
    }
}

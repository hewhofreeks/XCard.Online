using BlazorState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XCard.Client.Features.Game.GameSessionUpdated;
using XCard.Shared;

namespace XCard.Client.Features.Game
{
    public partial class GameState
    {
        public class GameSessionUpdatedHandler : RequestHandler<GameSessionUpdatedAction, GameState>
        {
            private readonly IGameHubClient _gameHubClient;

            public GameSessionUpdatedHandler(IStore aStore, IGameHubClient gameHubClient) : base(aStore)
            {
                _gameHubClient = gameHubClient;
            }

            public override async Task<GameState> Handle(GameSessionUpdatedAction request, CancellationToken aCancellationToken)
            {
                var state = Store.GetState<GameState>();

                var connection = await _gameHubClient.GetHubConnection();
                state.Session = await connection.InvokeAsync<GameSession>("FindSession", request.GameID);

                return state;
            }
        }
    }
}

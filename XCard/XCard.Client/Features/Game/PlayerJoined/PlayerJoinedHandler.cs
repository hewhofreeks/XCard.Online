using BlazorState;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XCard.Client.Features.Game;
using XCard.Client.Features.Game.PlayerJoined;

namespace XCard.Client.Features.Game
{
    public partial class GameState
    {
        public class PlayerJoinedHandler : RequestHandler<PlayerJoinedAction, GameState>
        {
            public PlayerJoinedHandler(IStore aStore) : base(aStore)
            {
            }

            public override Task<GameState> Handle(PlayerJoinedAction request, CancellationToken aCancellationToken)
            {
                var state = this.Store.GetState<GameState>();

                state.Users.Add(request.Username);
                state.ButtonPushed = false;

                Console.WriteLine("Updating State:");
                Console.WriteLine(JsonConvert.SerializeObject(state));

                return Task.FromResult(state);
            }

        }
    }
}

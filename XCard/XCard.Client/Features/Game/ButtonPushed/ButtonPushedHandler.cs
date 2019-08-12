using BlazorState;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XCard.Client.Features.Game.ButtonPushed;

namespace XCard.Client.Features.Game
{
    public partial class GameState
    {
        public class ButtonPushedHandler : RequestHandler<ButtonPushedAction, GameState>
        {
            public ButtonPushedHandler(IStore aStore) : base(aStore)
            {
            }

            public override Task<GameState> Handle(ButtonPushedAction aRequest, CancellationToken aCancellationToken)
            {
                var state = this.Store.GetState<GameState>();

                state.ButtonPushed = true;

                Console.WriteLine("Updating State:");
                Console.WriteLine(JsonConvert.SerializeObject(state));

                return Task.FromResult(state);
            }
        }
    }
    
}

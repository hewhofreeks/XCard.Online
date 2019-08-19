using BlazorState;
using BlazorState.Pipeline.ReduxDevTools;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCard.Client.Components;
using XCard.Client.Features.Game.ButtonPushed;
using XCard.Client.Features.Game.GameSessionUpdated;
using XCard.Client.Features.Game.LoadGame;
using XCard.Client.Features.Game.PlayerJoined;
using XCard.Client.Models;
using XCard.Client.Services;
using XCard.Shared;
using XCard.Shared.ClientHandlers;

namespace XCard.Client.Features.Game
{
    public class GameModel : BlazorStateComponent
    {
        public GameState State => this.GetState<GameState>();

        [Inject] public IMediator Mediator { get; set; }
        [Inject] public IStore Store { get; set; }



        [Inject]
        protected ILocalGameStorage _LocalGameStorage { get; set; }

        [Inject]
        protected IUriHelper UriHelper { get; set; }

        [Inject] IGameHubClient GameHubClient { get; set; }

        [Parameter]
        string GameID { get; set; }


        protected override async Task OnInitializedAsync()
        {
            // Try to establish a pre-existing connection
            var connection = await GameHubClient.GetHubConnection();

            connection.On<string>(GameHandlers.SESSION_UPDATED,
                (s) =>
                {
                    return Mediator.Send(new GameSessionUpdatedAction { GameID = new Guid(GameID) });
                });

            await Mediator.Send(new LoadGameAction { GameID = new Guid(GameID) });

            await base.OnInitializedAsync();
        }

        protected async Task JoinGame()
        {
            await Mediator.Send(new PlayerJoinedAction { GameID = new Guid(this.GameID), Username = this.State.CurrentUser.Username });
        }

        internal void ButtonClick() =>
            Mediator.Send(new ButtonPushedAction());

    }
}

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

        internal void ButtonClick() =>
            Mediator.Send(new ButtonPushedAction());

        protected LocalUserData CurrentUser { get; set; }

        [Inject]
        protected ILocalGameStorage _LocalGameStorage { get; set; }

        [Inject]
        protected IUriHelper UriHelper { get; set; }

        [Inject] IGameHubClient GameHubClient { get; set; }

        [Parameter]
        string GameID { get; set; }

        protected override async Task OnInitAsync()
        {
            // Try to establish a pre-existing connection
            var connection = await GameHubClient.GetHubConnection();

            connection.On<string>(GameHandlers.PLAYER_JOINED,
                (s) =>
                {
                    return Mediator.Send(new PlayerJoinedAction { Username = s });
                });

            await Mediator.Send(new LoadGameAction { GameID = new Guid(GameID) });

            //if (CurrentUser == null)
            //{
            //    CurrentUser = await _LocalGameStorage.FindUser();

            //    if (CurrentUser == null)
            //    {
            //        var redirectTo = UriHelper.GetAbsoluteUri().Replace(UriHelper.GetBaseUri(), "");

            //        UriHelper.NavigateTo($"?redirectTo={redirectTo}");

            //    }
            //}



            //var foundSession = await connection.InvokeAsync<GameSession>("FindSession", GameID);
            //if (foundSession == null)
            //{
            //    UriHelper.NavigateTo("/Home");
            //}

            //// Join Session
            //await connection.InvokeAsync<GameSession>("JoinGame", CurrentUser.Username, GameID);



            //await base.OnInitAsync();
            await base.OnInitAsync();
        }

        //public async Task LogOut()
        //{
        //    await _LocalGameStorage.LogOut();

        //    _UriHelper.NavigateTo("/");
        //}


    }
}

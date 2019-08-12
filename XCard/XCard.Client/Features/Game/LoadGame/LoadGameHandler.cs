using BlazorState;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XCard.Client.Services;
using XCard.Shared;

namespace XCard.Client.Features.Game.LoadGame
{
    public class LoadGameHandler : RequestHandler<LoadGameAction, GameState>
    {
        private readonly IGameHubClient _gameHubClient;
        private readonly IUriHelper _uriHelper;
        private readonly ILocalGameStorage _localGameStorage;

        public LoadGameHandler(IStore aStore, IGameHubClient gameHubClient, IUriHelper uriHelper, ILocalGameStorage localGameStorage) : base(aStore)
        {
            _gameHubClient = gameHubClient;
            _uriHelper = uriHelper;
            _localGameStorage = localGameStorage;
        }

        public override async Task<GameState> Handle(LoadGameAction request, CancellationToken aCancellationToken)
        {
            var state = Store.GetState<GameState>();

            var connection = await _gameHubClient.GetHubConnection();
            var currentUser = await _localGameStorage.FindUser();

            var foundSession = await connection.InvokeAsync<GameSession>("FindSession", request.GameID);
            if (foundSession == null)
            {
                _uriHelper.NavigateTo("/Home");
            }

            //foundSession = await connection.InvokeAsync<GameSession>("JoinGame", currentUser.Username, request.GameID);

            state.Session = foundSession;
            state.IsGameLoaded = true;
            state.Users.AddRange(foundSession.CurrentUsers.Select(s => s.Username));
            state.CurrentUser = currentUser;
            Console.WriteLine("Updating State:");
            Console.WriteLine(JsonConvert.SerializeObject(state));
            return state;
        }
    }
}

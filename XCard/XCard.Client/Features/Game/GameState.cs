using BlazorState;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCard.Client.Models;
using XCard.Client.Services;
using XCard.Shared;

namespace XCard.Client.Features.Game
{
    public partial class GameState : State<GameState>
    {
        public GameState() { }

        public LocalUserData CurrentUser { get; set; }
        public GameSession Session { get; set; }
        public bool IsGameLoaded { get; set; }
        public bool HasJoinedGame { get; set; }
        public bool ButtonPushed { get; set; }

        protected override void Initialize()
        {
            IsGameLoaded = false;
            Session = null;
            HasJoinedGame = false;
        }
    }
}

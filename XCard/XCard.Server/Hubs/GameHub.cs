using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCard.Server.Stores;
using XCard.Shared;

namespace XCard.Server.Hubs
{
    public class GameHub : Hub
    {
        private readonly IGameSessionStore _gameSessionStore;

        public GameHub(IGameSessionStore gameSessionStore)
        {
            _gameSessionStore = gameSessionStore;
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _gameSessionStore.QuitSessions(this.Context.ConnectionId);
        }

        public async Task<GameSession> FindSession(string sessionId)
        {
            return _gameSessionStore.FindSession(Guid.Parse(sessionId));
        }

        public async Task<GameSession> JoinGame(string username, string sessionId)
        {
            var sessionForUser = _gameSessionStore.FindSession(this.Context.ConnectionId);

            if(sessionForUser == null)
            {
                var user = new GameUser { Username = username, ConnectionId = this.Context.ConnectionId };

                sessionForUser = _gameSessionStore.JoinSession(Guid.Parse(sessionId), user);

                await this.Groups.AddToGroupAsync(this.Context.ConnectionId, sessionForUser.SessionID.ToString());
            }

            return sessionForUser;
        }

        public async Task<GameSession> StartGame(string username, string sessionName)
        {
            var admin = new GameUser { Username = username, ConnectionId = this.Context.ConnectionId };

            var session = _gameSessionStore.StartSession(admin, sessionName);

            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, session.SessionID.ToString());

            return session;
        }

        public async Task EndSession(string sessionID)
        {
            _gameSessionStore.EndSession(Guid.Parse(sessionID));
        }

    }
}

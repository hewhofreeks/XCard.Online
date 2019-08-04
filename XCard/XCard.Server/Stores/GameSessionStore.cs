using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCard.Shared;

namespace XCard.Server.Stores
{
    public interface IGameSessionStore
    {
        GameSession FindSession(Guid id);

        GameSession FindSession(string connectionID);

        GameSession JoinSession(Guid id, GameUser user);

        GameSession StartSession(GameUser host, string sessionName);

        IEnumerable<GameSession> QuitSessions(string connectionID);

        void EndSession(Guid id);
    }

    public class GameSessionStore : IGameSessionStore
    {
        private Dictionary<Guid, GameSession> _gameSessionStore { get; set; } = new Dictionary<Guid, GameSession>();

        public void EndSession(Guid id)
        {
            if (!_gameSessionStore.ContainsKey(id))
                throw new KeyNotFoundException();

            _gameSessionStore.Remove(id);
        }

        public GameSession FindSession(Guid id)
        {
            if (!_gameSessionStore.ContainsKey(id))
                throw new KeyNotFoundException();

            return _gameSessionStore[id];
        }

        public GameSession FindSession(string connectionID)
        {
            var sessions = _gameSessionStore.Where(s => s.Value.CurrentUsers.Any(c => c.ConnectionId == connectionID));

            if (sessions.Any())
                return sessions.FirstOrDefault().Value;

            return null;
        }

        public GameSession JoinSession(Guid id, GameUser user)
        {
            if (!_gameSessionStore.ContainsKey(id))
                throw new KeyNotFoundException();

            var session = _gameSessionStore[id];

            session.CurrentUsers.Add(user);

            return session;
        }

        public IEnumerable<GameSession> QuitSessions(string connectionID)
        {
            var sessions = _gameSessionStore.Where(s => s.Value.CurrentUsers.Any(c => c.ConnectionId == connectionID));

            foreach(var session in sessions)
            {
                var user = session.Value.CurrentUsers.FirstOrDefault(s => s.ConnectionId == connectionID);

                if (user != null)
                    session.Value.CurrentUsers.Remove(user);
            }

            return sessions.Select(s => s.Value);
        }

        public GameSession StartSession(GameUser host, string sessionName)
        {
            Guid id = Guid.NewGuid();

            var session = new GameSession { SessionID = id, CurrentUsers = new List<GameUser>(), Host = host, SessionName = sessionName };
            _gameSessionStore.Add(id, session);

            return session;
        }
    }
}

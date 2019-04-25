using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCard.Shared;

namespace XCard.Server.Stores
{
    public interface IGameSessionStore
    {
        GameSession JoinSession(Guid id, GameUser user);

        GameSession StartSession(GameUser host);

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

        public GameSession JoinSession(Guid id, GameUser user)
        {
            if (!_gameSessionStore.ContainsKey(id))
                throw new KeyNotFoundException();

            var session = _gameSessionStore[id];

            session.CurrentUsers.Add(user);

            return session;
        }

        public GameSession StartSession(GameUser host)
        {
            Guid id = Guid.NewGuid();

            var session = new GameSession { SessionID = id, CurrentUsers = new List<GameUser>(), Host = host };
            _gameSessionStore.Add(id, session);

            return session;
        }
    }
}

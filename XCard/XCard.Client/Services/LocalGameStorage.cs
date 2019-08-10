using Blazor.Extensions.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCard.Client.Models;
using XCard.Shared;

namespace XCard.Client.Services
{
    public interface ILocalGameStorage
    {
        Task<LocalUserData> FindUser();

        Task AddOrUpdateUser(LocalUserData user);

        Task LogOut();

        Task<LocalGameData[]> GetPastGameConnections();

        Task AddGameConnection(string gameName, Guid sessionID);

    }

    public class LocalGameStorage: ILocalGameStorage
    {
        private readonly SessionStorage _localStorage;
        private readonly string USER_KEY = "XCARD_USER";
        private readonly string PAST_GAMES_KEY = "XCARD_GAMES";


        public LocalGameStorage(SessionStorage localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task AddGameConnection(string gameName, Guid sessionID)
        {
            var pastGames = await GetPastGameConnections();

            var gameList = pastGames.ToList();

            gameList.Add(new LocalGameData { AddedOn = DateTime.Now, Name = gameName, SessionID = sessionID });

            if(gameList.Count > 5)
            {
                var toRemove = gameList.OrderByDescending(g => g.AddedOn).Last();
                gameList.Remove(toRemove);
            }

            await _localStorage.SetItem(PAST_GAMES_KEY, gameList.ToArray());
        }

        public async Task AddOrUpdateUser(LocalUserData user)
        {
            await _localStorage.SetItem<LocalUserData>(USER_KEY, user);
        }

        public async Task<LocalUserData> FindUser()
        {
            return await _localStorage.GetItem<LocalUserData>(USER_KEY);
        }

        public async Task<LocalGameData[]> GetPastGameConnections()
        {
            var pastGames = await _localStorage.GetItem<LocalGameData[]>(PAST_GAMES_KEY);

            if (pastGames == null) {
                pastGames = Enumerable.Empty<LocalGameData>().ToArray();
                await _localStorage.SetItem<LocalGameData[]>(PAST_GAMES_KEY, pastGames);
            }

            return pastGames;
        }

        public async Task LogOut()
        {
            await _localStorage.Clear();
        }
    }
}

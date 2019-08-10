using Blazor.Extensions;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCard.Shared;

namespace XCard.Client
{
    public interface IGameHubClient
    {
        Task<HubConnection> GetHubConnection();

        void StartConnection();
    }

    public class GameHubClient : IGameHubClient
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly HubConnection _hubConnection;
        private Task _isHubConnected;

        public GameHubClient(IJSRuntime jSRuntime)
        {
            _jsRuntime = jSRuntime;
            _hubConnection = new HubConnectionBuilder(_jsRuntime)
                .WithUrl("/gameHub", // The hub URL. If the Hub is hosted on the server where the blazor is hosted, you can just use the relative path.
                    opt =>
                    {
                        opt.LogMessageContent = true;
                        opt.LogLevel = SignalRLogLevel.Trace; // Client log level
                        opt.Transport = HttpTransportType.WebSockets; // Which transport you want to use for this connection
                    })
                    .Build(); // Build the HubConnection
        }

        public void StartConnection()
        {
            _isHubConnected = _hubConnection.StartAsync();
        }

        public async Task<HubConnection> GetHubConnection()
        {
            await _isHubConnected;

            return _hubConnection;
        }
    }
}

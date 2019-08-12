using Blazor.Extensions;
using Blazor.Extensions.Storage;
using BlazorState;
using MediatR;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using XCard.Client.Features.Game;
using XCard.Client.Services;

namespace XCard.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IGameHubClient, GameHubClient>();
            services.AddSingleton<ILocalGameStorage, LocalGameStorage>();
            services.AddStorage();
            services.AddTransient<GameState>();
            services.AddBlazorState((aOptions) => aOptions.Assemblies =
              new Assembly[]
              {
                typeof(Client.Startup).GetTypeInfo().Assembly
              });
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}

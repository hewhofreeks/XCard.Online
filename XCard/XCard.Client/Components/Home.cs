using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCard.Client.Models;
using XCard.Client.Services;

namespace XCard.Client.Components
{
    public class Home : BaseAuthComponent
    {
        protected LocalGameData[] Games { get; set; }

        public Home() 
        {
        }

        protected override async Task OnInitAsync()
        {
            await base.OnInitAsync();

            Games = await _LocalGameStorage.GetPastGameConnections();
        }

    }
}

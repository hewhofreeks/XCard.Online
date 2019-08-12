using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorState;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Components;
using XCard.Client.Models;
using XCard.Client.Services;
using XCard.Shared;

namespace XCard.Client.Components
{
    public class BaseAuthComponent : BlazorStateComponent
    {
        protected LocalUserData CurrentUser { get; set; }

        [Inject]
        protected ILocalGameStorage _LocalGameStorage { get; set; }

        [Inject]
        protected IUriHelper _UriHelper { get; set; }

        public BaseAuthComponent()
        {

        }

        protected override async Task OnInitAsync()
        {
            if (CurrentUser == null)
            {
                CurrentUser = await _LocalGameStorage.FindUser();

                if (CurrentUser == null)
                {
                    var redirectTo = _UriHelper.GetAbsoluteUri().Replace(_UriHelper.GetBaseUri(), "");

                    _UriHelper.NavigateTo($"?redirectTo={redirectTo}");

                }
            }

            await base.OnInitAsync();
        }

        public async Task LogOut()
        {
            await _LocalGameStorage.LogOut();

            _UriHelper.NavigateTo("/");
        }
    }
}

using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCard.Client.Models;
using XCard.Client.Services;

namespace XCard.Client.Components
{
    public class Index : ComponentBase
    {

        [Inject]
        private ILocalGameStorage _localGameStorage { get; set; }

        [Inject]
        private IUriHelper _uriHelper { get; set; }

        public Index()
        {
           
        }

        public async Task CreateUserAndGoHome(string username)
        {
            await _localGameStorage.AddOrUpdateUser(new LocalUserData { Username = username, UserID = Guid.NewGuid() });

            var uri = new Uri(_uriHelper.GetAbsoluteUri());
            var redirectTo = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query).TryGetValue("redirectTo", out var type) ? type.First() : "";

            if (!String.IsNullOrEmpty(redirectTo))
                _uriHelper.NavigateTo(redirectTo);
            else
                _uriHelper.NavigateTo("/Home");
        }

        protected override async Task OnInitAsync()
        {

            var user = await _localGameStorage.FindUser();

            if (user != null)
                _uriHelper.NavigateTo("/Home");


            await base.OnInitAsync();
        }
    }
}

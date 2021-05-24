using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TheOxbridgeApp.Models;
using TheOxbridgeApp.Services;
using Xamarin.Forms;

namespace TheOxbridgeApp.ViewModels
{   
    public class TeamViewModel : BaseViewModel
    {
        #region -- Commands -- 
        public ICommand NavigateToEditTeamCMD { get; set; }
        #endregion

        private ServerClient serverClient;
        private SingletonSharedData sharedData;

        public TeamViewModel()
        {
            serverClient = new ServerClient();
            sharedData = SingletonSharedData.GetInstance();

            NavigateToEditTeamCMD = new Command(NavigateToEditTeam);
        }

        private async void NavigateToEditTeam()
        {
            await NavigationService.NavigateToAsync(typeof(EditTeamViewModel));
        }
        
    }
}

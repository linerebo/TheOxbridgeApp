using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TheOxbridgeApp.Models;
using TheOxbridgeApp.Services;
using Xamarin.Forms;

namespace TheOxbridgeApp.ViewModels
{   
    public class TeamViewModel : BaseViewModel
    {
        #region --Local variables--
        private ServerClient serverClient;
        public SingletonSharedData sharedData { get; set; }
        private List<Ship> unHandledShips;
        #endregion


        #region --Binding values--
        private Ship selectedShip;
        public Ship SelectedShip
        {
            get { return selectedShip; }
            set { selectedShip = value; OnPropertyChanged(); }
        }

        private String searchText;
        public String SearchText
        {
            get { return searchText; }
            set { searchText = value; OnPropertyChanged(); UpdateListShips(); }
        }
        #endregion


        #region -- Commands -- 
        public ICommand NavigateToEditTeamCMD { get; set; }
        #endregion


        public TeamViewModel()
        {
            serverClient = new ServerClient();
            sharedData = SingletonSharedData.GetInstance();
            
            NavigateToEditTeamCMD = new Command(NavigateToEditTeam);

            unHandledShips = serverClient.GetAllShips();
            sharedData.Ships = new ObservableCollection<Ship>(unHandledShips);
        }

        // Navigates to View with selected Ship
        private async void NavigateToEditTeam()
        {
            sharedData.SelectedShip.teamImage = null;
            await NavigationService.NavigateToAsync(typeof(EditTeamViewModel));
        }

        // Updates the list of ships according to searchText
        private void UpdateListShips()
        {
            sharedData.Ships = new ObservableCollection<Ship>(unHandledShips.Where(e => e.Name.ToLower().Contains(searchText.ToLower())));
        }  
    }
}

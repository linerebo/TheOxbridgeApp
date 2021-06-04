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
        private ObservableCollection<Ship> ships;
        public ObservableCollection<Ship> Ships
        {
            get { return ships; }
            set { ships = value; OnPropertyChanged(); }
        }

        private Ship selectedShip;
        public Ship SelectedShip
        {
            get { return selectedShip; }
            set { selectedShip = value; OnPropertyChanged(); NavigateToShip(); }
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
            Ships = new ObservableCollection<Ship>(unHandledShips);
            //sharedData.Ships = new ObservableCollection<Ship>(serverClient.GetAllShips());
        }

        // Navigates to View with selected Ship
        private async void NavigateToEditTeam()
        {
            await NavigationService.NavigateToAsync(typeof(EditTeamViewModel));
        }

        
        private async void NavigateToShip()
        {

        }

        // Updates the list of ships according to searchText
        private void UpdateListShips()
        {
            Ships = new ObservableCollection<Ship>(unHandledShips.Where(e => e.Name.ToLower().Contains(searchText.ToLower()) || e.TeamName.ToLower().Contains(searchText.ToLower())));
        }  
    }
}

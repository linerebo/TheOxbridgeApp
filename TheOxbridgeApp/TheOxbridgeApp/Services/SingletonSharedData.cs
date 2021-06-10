using System;
using System.Collections.ObjectModel;
using TheOxbridgeApp.ViewModels.Popups;
using Xamarin.Forms;

namespace TheOxbridgeApp.Models
{
    public sealed class SingletonSharedData : BindableObject
    {
        #region -- Local variables --

        private static SingletonSharedData instance = null;

        private static readonly object padlock = new object();
        #endregion

        #region -- Public variables --

        public float Direction { get; set; }

        public int SelectedShipId { get; set; }

        public Event SelectedEvent { get; set; }


        public Ship SelectedShip { get; set; }
        public TeamImage teamImage { get; set; }

        private ObservableCollection<Ship> ships;
        public ObservableCollection<Ship> Ships
        {
            get { return ships; }
            set { ships = value; OnPropertyChanged(); }
        }

        public bool isMapDisplayed { get; set; }

        public bool HasSelectedDifferentEvent { get; set; }


        public TrackingPoupViewModel TrackingPoupViewModel;
        #endregion

        private SingletonSharedData()
        {
        }


        /// <summary>
        /// The method creates an instance of the SingletonSharedData class or returns the existing instance
        /// if it has already been created. 
        /// </summary>
        /// <returns>Returns the SingletonSharedData object</returns>
        public static SingletonSharedData GetInstance()
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new SingletonSharedData();

                }
                return instance;
            }
        }
    }
}

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TheOxbridgeApp.Models
{
    public class Ship : INotifyPropertyChanged
    {
        private int shipId;
        public int ShipId 
        {
            get { return shipId; }
            set { shipId = value; this.PropertyIsChanged(); }
        }

        private String name;
        public String Name 
        {
            get { return name; }
            set { name = value; this.PropertyIsChanged(); }
        }

        private String username;
        public String Username 
        { 
            get { return username; }
            set { username = value; this.PropertyIsChanged(); }
        }

        private String teamName;
        public String TeamName 
        {
            get { return teamName; } 
            set { teamName = value; this.PropertyIsChanged(); } 
        }

        private TeamImage _teamImage;
        public TeamImage teamImage 
        { 
            get { return _teamImage; }
            set { _teamImage = value; this.PropertyIsChanged(); }
        }

        
        public event PropertyChangedEventHandler PropertyChanged;
        public void PropertyIsChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TheOxbridgeApp.Models
{
    public class Ship : INotifyPropertyChanged
    {
        public int ShipId { get; set; }

        public String Name { get; set; }

        public String Username { get; set; }

        public String TeamName { get; set; }
        public TeamImage teamImage { get; set; }

        
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

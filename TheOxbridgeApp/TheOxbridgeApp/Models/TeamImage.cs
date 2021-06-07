using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace TheOxbridgeApp.Models
{
    public class TeamImage: INotifyPropertyChanged
    {
        private byte[] picture;
        public byte[] Picture
        {
            get => picture;
            set { picture = value; PictureSource = ImageSource.FromStream(() => new MemoryStream(picture)); }
        }

        private ImageSource pictureSource = ImageSource.FromFile("trackingBoatIcon.png");
        public ImageSource PictureSource
        {
            set { pictureSource = value; RaisePropertyChanged(); }
            get { return pictureSource; }
        }

        public String Filename { get; set; } //= "trackingBoatIcon.png";

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Input;
using TheOxbridgeApp.Models;
using TheOxbridgeApp.Services;
using Xamarin.Forms;
using System.Runtime.CompilerServices;


namespace TheOxbridgeApp.ViewModels
{
    public class EditTeamViewModel : BaseViewModel
    {
        private ServerClient serverClient;
        private SingletonSharedData sharedData;
        public String ErrorMsg { get; set; }
        public TeamImage TeamPicture { get; set; }

        
        #region --Commands--
        public ICommand TakePhotoCMD { get; set; }
        #endregion

        public EditTeamViewModel()
        {
            serverClient = new ServerClient();
            sharedData = SingletonSharedData.GetInstance();

            TakePhotoCMD = new Command(TakePhoto);
            TeamPicture = new TeamImage();
        }

        private async void TakePhoto()
        {
            if (isBusy)
                return;
            isBusy = true;
            ErrorMsg = "";

            TeamImage teamImageTmp = new TeamImage();

            try
            {
                if (!Plugin.Media.CrossMedia.Current.IsCameraAvailable ||
                    !Plugin.Media.CrossMedia.Current.IsTakePhotoSupported)
                {
                    System.Diagnostics.Debug.WriteLine("No camera available. ");
                    ErrorMsg = "No camera available. ";
                    isBusy = false;
                    return;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ex.Message: " + ex.Message);
                isBusy = false;
                return;
            }
            DateTime dt = DateTime.Now;
            string createdDate = dt.Year.ToString("d4") + dt.Month.ToString("d2") + dt.Day.ToString("d2");
            string photoName = String.Format("teamphoto_{0}.jpg", createdDate);

            var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions()
            { SaveToAlbum = true, Name = photoName, PhotoSize = PhotoSize.Medium, AllowCropping = false, SaveMetaData = false });

            if (photo != null)
            {
                using (photo)
                {
                    using (var memoryStreamHandler = new MemoryStream())
                    {
                        photo.GetStreamWithImageRotatedForExternalStorage().CopyTo(memoryStreamHandler);

                        teamImageTmp.Picture = memoryStreamHandler.ToArray();
                        TeamPicture = teamImageTmp;
                    }
                }
            }
            isBusy = false;
        }
    }
}

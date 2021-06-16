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
using Xamarin.Essentials;
using System.Collections.ObjectModel;

namespace TheOxbridgeApp.ViewModels
{
    public class EditTeamViewModel : BaseViewModel
    {
        #region -- local variables
        private ServerClient serverClient;
        public SingletonSharedData sharedData { get; set; }
        public String ErrorMsg { get; set; }
        public Ship SelectedShip { get; set; }
        private List<Ship> unHandledShips;
        #endregion

        #region --Commands--
        public ICommand TakePhotoCMD { get; set; }
        public ICommand ChoosePhotoCMD { get; set; }
        public ICommand PickPhotoFromGalleryCMD { get; set; }
        public ICommand GoToTeamsCMD { get; set; }
        #endregion

        #region -- Binding values--
        private TeamImage _teamPicture;
        public TeamImage TeamPicture
        {
            get { return _teamPicture; }
            set { _teamPicture = value; OnPropertyChanged(); }
        }
        private String photoPath;
        public String PhotoPath
        {
            get => photoPath;
            set
            {
                photoPath = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public EditTeamViewModel()
        {
            serverClient = new ServerClient();
            sharedData = SingletonSharedData.GetInstance();
            
            TakePhotoCMD = new Command(TakePhoto);
            ChoosePhotoCMD = new Command(ChoosePhoto);
            PickPhotoFromGalleryCMD = new Command(PickPhotoFromGallery);
            GoToTeamsCMD = new Command(GoToTeams);
            TeamPicture = new TeamImage();
            SelectedShip = new Ship();
        }

        //use device camera to take new photo and store on device. Photo is shown in EditTeamView
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
                        teamImageTmp.Filename = photoName; //jpeg file
                        TeamPicture = teamImageTmp;
                    }
                }
            }
            isBusy = false;
        }

        // Pick photo from gallery on device
        public async void PickPhotoFromGallery()
        {
            var photo = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "please pick a photo"
            });

            if (photo != null)
            {
                var stream = await photo.OpenReadAsync();

                TeamPicture.PictureSource = ImageSource.FromStream(() => stream);
                TeamPicture.Filename = photo.FullPath;
                TeamPicture.Picture = System.IO.File.ReadAllBytes(TeamPicture.Filename);  //converting to byte array
            }
        }

        // selecting an TeamImage to be stored on the object SelectedShip
        public void ChoosePhoto()
        {
            sharedData.SelectedShip.teamImage = TeamPicture;
            serverClient.SaveImageToDB(sharedData.SelectedShip.ShipId, sharedData.SelectedShip.teamImage);
            
        }

        public async void GoToTeams()
        {
            TeamPicture.PictureSource = ImageSource.FromFile("trackingBoatIcon.png"); //unselecting image shown in EditTeamView
            unHandledShips = serverClient.GetAllShips();
            sharedData.Ships = new ObservableCollection<Ship>(unHandledShips);
            await NavigationService.NavigateToAsync(typeof(TeamViewModel));
        }
    }
}

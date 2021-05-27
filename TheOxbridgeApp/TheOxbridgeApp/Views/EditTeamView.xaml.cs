using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TheOxbridgeApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditTeamView : ContentPage
    {
        public EditTeamView()
        {
            InitializeComponent();
            //btnTakePhoto.Clicked += btnTakePhoto_Clicked;
            btnPickPhoto.Clicked += btnPickPhoto_Clicked;
        }

        private async void btnPickPhoto_Clicked(object sender, EventArgs e)
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "please pick a photo"
            });

            if (result != null)
            {
                var stream = await result.OpenReadAsync();

                resultImage.Source = ImageSource.FromStream(() => stream);
            }
        }

        //method for take photo, the photo is shown in resultImage
        /*private async void btnTakePhoto_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Plugin.Media.CrossMedia.Current.Initialize();
                if (!Plugin.Media.CrossMedia.Current.IsCameraAvailable ||
                    !Plugin.Media.CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("No Camera ", "No camera available. ", "OK");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions()
            { SaveToAlbum = true, Name = "Team.jpg" });//, DefaultCamera= Plugin.Media.Abstractions.CameraDevice.Rear

            if (photo != null)
            {
                using (photo)
                {
                    resultImage.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
                    //photo.Dispose();
                }
            }
        }*/
    }
}
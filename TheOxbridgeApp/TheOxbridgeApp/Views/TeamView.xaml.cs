using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.IO;

namespace TheOxbridgeApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TeamView : ContentPage
    {
        public TeamView()
        {
            InitializeComponent();

            btnTakePhoto.Clicked += async (sender, args) =>
            {
                var result = await MediaPicker.CapturePhotoAsync();
                if (result != null)
                {
                    var stream = await result.OpenReadAsync(); // the stream to the file

                    resultImage.Source = ImageSource.FromStream(() => stream);

                    //save the file into local storage
                    var newFile = Path.Combine(FileSystem.CacheDirectory, result.FileName);
                    Console.WriteLine("file name: " + result.FileName);
                    Console.WriteLine("file path: " + result.FullPath);
                    var newStream = File.OpenWrite(newFile);
                    await stream.CopyToAsync(newStream);
                }
            };

            btnPickPhoto.Clicked += async (sender, args) =>
            {

                var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "please pick a photo"
                }); // abstract file result

                if (result != null)
                {
                    var stream = await result.OpenReadAsync(); // the stream to th file

                    resultImage.Source = ImageSource.FromStream(() => stream);
                }
            };
        }
    }
}
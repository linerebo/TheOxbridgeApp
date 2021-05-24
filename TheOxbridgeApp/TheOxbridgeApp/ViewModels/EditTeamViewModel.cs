using System;
using System.Collections.Generic;
using System.Text;
using TheOxbridgeApp.Models;
using TheOxbridgeApp.Services;

namespace TheOxbridgeApp.ViewModels
{
    public class EditTeamViewModel : BaseViewModel
    {
        private ServerClient serverClient;
        private SingletonSharedData sharedData;

        public EditTeamViewModel()
        {
            serverClient = new ServerClient();
            sharedData = SingletonSharedData.GetInstance();
        }
    }
}

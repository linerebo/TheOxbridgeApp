using System;
using System.Collections.Generic;
using System.Text;
using TheOxbridgeApp.Models;
using TheOxbridgeApp.Services;

namespace TheOxbridgeApp.ViewModels
{
    public class TeamViewModel : BaseViewModel
    {
        private ServerClient serverClient;
        private SingletonSharedData sharedData;

        public TeamViewModel()
        {
            serverClient = new ServerClient();
            sharedData = SingletonSharedData.GetInstance();

        }
        
    }
}

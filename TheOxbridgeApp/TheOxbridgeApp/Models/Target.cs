using System;

namespace TheOxbridgeApp.Models
{
    public static class Target
    {
        //private const String azure = "https://oxbridgecloud.azurewebsites.net/";

        //private const String azure = "http://localhost:3000/"; // for emulator use 10.0.2.2 instead of localhost

        //private const String azure = "http://10.0.2.2:3000/";  // for emulator

        private const String azure = "http://192.168.178.46:3000/";  // for device



        private const String StandardAdress = azure;


        public const String Authenticate = StandardAdress + "users/login";
        public const String Events = StandardAdress + "events/";
        public const String EventsFromUsername = StandardAdress + "events/myevents/findfromusername";
        public const String EventRegistrations = StandardAdress + "eventRegistrations/findEventRegFromUsername/";
        public const String Locations = StandardAdress + "locationRegistrations/";
        public const String StartAndFinishPoints = StandardAdress + "racepoints/findStartAndFinish/";
        public const String LiveLocations = StandardAdress + "locationRegistrations/getlive/";
        public const String ReplayLocations = StandardAdress + "locationRegistrations/getReplay/";
        public const String Ships = StandardAdress + "ships/";
        public const String ShipFromEventId = StandardAdress + "ships/fromeventid/";
    }
}

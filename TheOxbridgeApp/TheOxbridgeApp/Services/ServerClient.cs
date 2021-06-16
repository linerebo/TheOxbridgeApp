using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TheOxbridgeApp.Data;
using TheOxbridgeApp.Models;

namespace TheOxbridgeApp.Services
{
    public class ServerClient
    {
        #region -- Local variables -- 
        private DataController dataController;
        private ServerClient client;
        
        #endregion

        public ServerClient()
        {
            dataController = new DataController();
            
        }


        /// <summary>
        /// Contacts the backend in order to login and get a token from the backend 
        /// </summary>
        /// <param name="username">The username of the user</param>
        /// <param name="password">The password of the user</param>
        /// <returns>A user with a token</returns>
        public User Login(String username, String password)
        {
            String target = Target.Authenticate;
            String jsonData = "{\"emailUsername\": \"" + username + "\", \"password\": \"" + password + "\" }";
            WebRequest request = WebRequest.Create(target);
            request.Method = "POST";
            request.ContentType = "application/json";

            try 
            { 
                using (Stream requestStream = request.GetRequestStream())
                {
                    using (StreamWriter streamWriter = new StreamWriter(requestStream))
                    {
                        streamWriter.Write(jsonData);
                    }
                }
            }
            catch 
            {
                Console.WriteLine("There is no connection to the server! ");
            }
            User foundUser = null;
            try
            {
                String responseFromServer = GetResponse(request);
                foundUser = JsonConvert.DeserializeObject<User>(responseFromServer);
            }
            catch (WebException)
            {
            }
            return foundUser;
        }


        /// <summary>
        /// Gets the last 20 locations for each boat in a specific event from the backend 
        /// </summary>
        /// <param name="eventId">The eventId for the event in question</param>
        /// <returns>Returns a List of ShipLocations</returns>
        public List<ShipLocation> GetLiveLocations(int eventId)
        {
            WebRequest request = WebRequest.Create(Target.LiveLocations + eventId);
            request.Method = "GET";
            request.ContentType = "application/json";

            String responseFromServer = GetResponse(request);
            List<ShipLocation> locations = JsonConvert.DeserializeObject<List<ShipLocation>>(responseFromServer);
            return locations;
        }

        /// <summary>
        /// Gets all the events that the logged in user is signed up for from the backend 
        /// </summary>
        /// <returns>A task with a List of TrackingEvents</returns>
        public async Task<List<TrackingEvent>> GetTrackingEvents()
        {
            WebRequest request = WebRequest.Create(Target.EventsFromUsername);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Headers.Add("x-access-token", (await dataController.GetUser()).Token);

            String responseFromServer = GetResponse(request);
            List<TrackingEvent> events = JsonConvert.DeserializeObject<List<TrackingEvent>>(responseFromServer);

            return events;
        }


        /// <summary>
        /// Gets all events from the backend
        /// </summary>
        /// <returns>A list of Events</returns>
        public List<Event> GetEvents()
        {

            WebRequest request = WebRequest.Create(Target.Events);
            request.Method = "GET";

            String responseFromServer = GetResponse(request);

            List<Event> events = JsonConvert.DeserializeObject<List<Event>>(responseFromServer);
            return events;

        }


        /// <summary>
        /// Gets all locations from all boats in a specific event from the backend
        /// </summary>
        /// <param name="eventId">The eventId for the event in question</param>
        /// <returns>A list of ShipLocations</returns>
        public List<ShipLocation> GetReplayLocations(int eventId)
        {
            try
            {
                WebRequest request = WebRequest.Create(Target.ReplayLocations + eventId);
                request.Method = "GET";
                request.ContentType = "application/json";

                String responseFromServer = GetResponse(request);
                List<ShipLocation> locations = JsonConvert.DeserializeObject<List<ShipLocation>>(responseFromServer);

                return locations;
            }
            catch (Exception)
            {
            }
            return null;
        }

        /// <summary>
        /// Gets a specific Ship from the backend 
        /// </summary>
        /// <param name="ShipId">The ShipId for the ship in question</param>
        /// <returns>A Ship</returns>
        public Ship GetShip(int ShipId)
        {
            WebRequest request = WebRequest.Create(Target.Ships + ShipId);
            request.Method = "GET";
            request.ContentType = "application/json";

            String responseFromServer = GetResponse(request);
            Ship ship = JsonConvert.DeserializeObject<Ship>(responseFromServer);
            return ship;
        }

        /// <summary>
        /// Gets the start and the fininish locations from the backend
        /// </summary>
        /// <param name="eventId">The eventId of the event in question</param>
        /// <returns>A List of RacePoints</returns>
        public List<RacePoint> GetStartAndFinish(int eventId)
        {
            WebRequest request = WebRequest.Create(Target.StartAndFinishPoints + eventId);
            request.Method = "GET";
            request.ContentType = "application/json";

            String responseFromServer = GetResponse(request);
            List<RacePoint> racePoints = JsonConvert.DeserializeObject<List<RacePoint>>(responseFromServer);
            return racePoints;
        }


        /// <summary>
        /// Get all Ships from a specific event from the backend
        /// </summary>
        /// <param name="eventId">The eventId of the event in question</param>
        /// <returns>A List of Ships</returns>
        public List<Ship> GetShipsFromEventId(int eventId)
        {
            WebRequest request = WebRequest.Create(Target.ShipFromEventId + eventId);
            request.Method = "GET";
            request.ContentType = "application/json";

            String responseFromServer = GetResponse(request);
            List<Ship> ships = null;
            if (!responseFromServer.Equals("{}"))
            {
                ships = JsonConvert.DeserializeObject<List<Ship>>(responseFromServer);
            }
            return ships;
        }

        /// <summary>
        /// Gets the response from a request from the backend
        /// </summary>
        /// <param name="request">The request from which you want a response</param>
        /// <returns>A string with the response</returns>
        private String GetResponse(WebRequest request)
        {
            String responseFromServer = "";
            
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(responseStream))

                        responseFromServer = reader.ReadToEnd();
                }
                return responseFromServer;
            }
        }

        /// <summary>
        /// Posts object with the ISerializable interface to a specific target url to the backend
        /// </summary>
        /// <param name="serializable">The ISerializable object that needs to be posted</param>
        /// <param name="target">The target url</param>
        /// <returns>A task with a boolean, true if succes and false if not</returns>
        public async Task<bool> PostData(ISerializable serializable, String target)
        {
            String statusCode = "";
            String jsonData = JsonConvert.SerializeObject(serializable);
            WebRequest request = WebRequest.Create(target);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("x-access-token", (await dataController.GetUser()).Token);

            using (Stream requestStream = request.GetRequestStream())
            {
                using (StreamWriter streamWriter = new StreamWriter(requestStream))
                {
                    streamWriter.Write(jsonData);
                }
            }
            try
            {
                statusCode = GetStatusCode(request);
            }
            catch (Exception)
            {
            }
            if (statusCode.ToLower().Equals("created"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the statuscode for a request from the backend
        /// </summary>
        /// <param name="request">The request from which a status code is needed</param>
        /// <returns>A string of the statuscode</returns>
        private String GetStatusCode(WebRequest request)
        {
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                return response.StatusCode.ToString();
            }
        }

        /// <summary>
        /// Get all ships from the backend
        /// </summary>
        /// <returns>A List of Ships</returns>
        public List<Ship> GetAllShips()
        {
            WebRequest request = WebRequest.Create(Target.Ships);
            request.Method = "GET";
            request.ContentType = "application/json";
            String responseFromServer = GetResponse(request);
            List<Ship> ships = JsonConvert.DeserializeObject<List<Ship>>(responseFromServer);

            WebRequest requestImages = WebRequest.Create(Target.Images);
            requestImages.Method = "GET";
            requestImages.ContentType = "application/json";
            responseFromServer = GetResponse(requestImages);
            List<ServerImage> images = JsonConvert.DeserializeObject<List<ServerImage>>(responseFromServer);

            foreach (ServerImage i in images)
            {
                Predicate<Ship> Match = s => s.ShipId == i.ShipId_img;  //lamda expression that tests if shipId of s equals shipId of image i
                Ship x = ships.Find(Match);
                x.teamImage = new TeamImage();
                x.teamImage.Picture = Convert.FromBase64String(i.ImageBase64);  //converting Image String to byte[]
                x.teamImage.Filename = i.Filename;
            }
            return ships;
        }


        /// <summary>
        /// Post an Image with shipId to the backend
        /// </summary>
        /// <param name="shipId"></param>
        /// <param name="teamImage"></param>
        public async void SaveImageToDB(int shipId, TeamImage teamImage)
        {
            try
            {
                HttpClient client = new HttpClient();
                MultipartFormDataContent content = new MultipartFormDataContent();
                ByteArrayContent baContent = new ByteArrayContent(teamImage.Picture);
                StringContent shipIdContent = new StringContent(shipId.ToString());

                content.Add(baContent, "image", teamImage.Filename);
                content.Add(shipIdContent, "shipId_img");

                var response = await client.PostAsync(Target.Images, content);
                var responsestr = response.Content.ReadAsStringAsync().Result;

                Debug.WriteLine(responsestr);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception Caught: " + e.ToString());
                return;
            }
        }
    }
}
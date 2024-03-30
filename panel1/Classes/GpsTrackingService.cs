//using System;
//using System.Net.Http;
//using System.Threading.Tasks;
//using Newtonsoft.Json;

//namespace panel1.Classes
//{
//    public interface IGpsService
//    {
//        Task<(double Latitude, double Longitude)> GetCoordinatesAsync();
//    }

//    public class GpsTrackingService : IGpsService
//    {
//        private readonly HttpClient _httpClient;

//        public GpsTrackingService(HttpClient httpClient)
//        {
//            _httpClient = httpClient;
//        }

//        public async Task<(double Latitude, double Longitude)> GetCoordinatesAsync()
//        {
//            try
//            {
//                var response = await _httpClient.GetAsync("https://nominatim.openstreetmap.org/");
//                response.EnsureSuccessStatusCode();

//                var content = await response.Content.ReadAsStringAsync();
//                var location = JsonConvert.DeserializeObject<LocationResponse>(content);

//                return (location.Latitude, location.Longitude);
//            }
//            catch (Exception ex)
//            {
//                // Handle exceptions
//                Console.WriteLine($"Error getting coordinates: {ex.Message}");
//                return (0, 0); // Return default coordinates or handle error differently
//            }
//        }
//    }

//    public class LocationResponse
//    {
//        public double Latitude { get; set; }
//        public double Longitude { get; set; }
//    }
//}


//using System;
//using System.Device.Location;

//namespace panel1.Classes
//{
//    public class LocationServices : IGpsService
//    {
//        private GeoCoordinateWatcher myWatcher;
//        private bool fgWatcherStarted = false;

//        public LocationServices()
//        {
//            myWatcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
//            fgWatcherStarted = myWatcher.TryStart(true, TimeSpan.FromMilliseconds(1000));
//        }

//        public (double Latitude, double Longitude) GetCoordinates()
//        {
//            var myPosition = new GeoCoordinate();

//            try
//            {
//                if (!fgWatcherStarted)
//                {
//                    fgWatcherStarted = myWatcher.TryStart(true, TimeSpan.FromMilliseconds(1000));
//                }

//                myPosition = myWatcher.Position.Location;

//                if (myPosition.IsUnknown)
//                {
//                    return (0, 0);
//                }
//                else
//                {
//                    return (myPosition.Latitude, myPosition.Longitude);
//                }

//            }
//            catch (Exception ex)
//            {
               
//                Console.WriteLine(ex.Message);
//                return (0, 0);
//            }
//        }
//    }
//}

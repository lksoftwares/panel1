

//using System;
//using System.Device.Location;

//namespace panel1.Classes
//{
//    public class GetLocationProperty
//    {
//        static void Main(string[] args)
//        {
//            GetLocationProperty();
//        }

//        static void GetLocationProperty()
//        {
//            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();

//            // Do not suppress prompt, and wait 1000 milliseconds to start.
//            watcher.TryStart(false, TimeSpan.FromMilliseconds(1000));

//            watcher.PositionChanged += (sender, e) =>
//            {
//                var coord = e.Position.Location;

//                if (coord.IsUnknown != true)
//                {
//                    Console.WriteLine("Lat: {0}, Long: {1}",
//                        coord.Latitude,
//                        coord.Longitude);
//                }
//                else
//                {
//                    Console.WriteLine("Unknown latitude and longitude.");
//                }
//            };

//            // Keep the console application running
//            Console.ReadLine();
//        }
//    }
//}

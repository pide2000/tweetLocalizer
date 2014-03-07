using Lucuma;
using Lucuma.Libs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipleGeoCoderTestApp
{
     
    class Program
    {

        
        static void Main(string[] args)
        {  

            //DEFAULT, GoogleMaps
          
           // GeoCoder gc = new GeoCoder();
           // IGeoCodeResult Target = new GeoCodeResult();
            //Target = g.GetCoordinates("Austin, TX");  // defaults to Google's GMap Service
           

            //BING MAPS

            GeoCoder gc = new GeoCoder();
            BingMapConfig bmc = new BingMapConfig();
            bmc.Key = "AjgNhLFwXI1zRKHwTyZYQSXQZwTlWdHrjSZ6DD-Pcp6rihIEeIoG7ywMGmb_TiRs";
            gc.AddProvider(new BingMap(bmc));
            IGeoCodeResult Target = new GeoCodeResult();
           
           
            //MAP QUEST 

            //GeoCoder gc = new GeoCoder();
            //MapQuestConfig mqc = new MapQuestConfig();
            //mqc.Key = "Fmjtd%7Cluubn1ut2d%2Cbw%3Do5-90b09y";
            //gc.AddProvider(new MapQuestMap(mqc));
            //IGeoCodeResult Target = new GeoCodeResult();
            //Target = gc.GetCoordinates("Abstatt");


            //OSM

            //IGeoCodeResult Target = new GeoCodeResult();
            //GeoCoder gc = new GeoCoder();
            //gc.AddProvider(new OpenStreetMap());
            //Target = gc.GetCoordinates("Austin, TX");

            //CLOUD MATE

            //GeoCoder gc = new GeoCoder();
            //CloudMadeConfig cmc = new CloudMadeConfig();
            //cmc.Key = "66ac5b1df21848ecafa38a53d92dabd1";

            //gc.AddProvider(new CloudMade(cmc));
            //IGeoCodeResult Target = new GeoCodeResult();
            //Target = gc.GetCoordinates("Austin, TX");

            Target = gc.GetCoordinates("");
            double lat;
            double lng;
            JObject response = (JObject)JsonConvert.DeserializeObject(Target.response);
            System.Console.WriteLine("JSON Response: " + response.ToString());
            System.Console.WriteLine(response.ToString());
            if (Target.HasValue)
            {
                lat = Target.Latitude;
                lng = Target.Longitude;
                System.Console.WriteLine("latitude: " + lat + "--- longitude: " + lng);
                System.Console.WriteLine("country" + Target.Locations.First().Country);
                
            }
            Console.ReadLine();
           
           
            
        }
    }
}

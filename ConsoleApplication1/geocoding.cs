using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace GeoCoderJson
{
    class GeoCoder
    {
        public class GeoLocation
        {
            public decimal Lat { get; set; }
            public decimal Lng { get; set; }
        }

        public class GeoGeometry
        {
            public GeoLocation Location { get; set; }
        }

        public class GeoResult
        {
            public GeoGeometry Geometry { get; set; }
        }

        public class GeoResponse
        {
            public string Status { get; set; }
            public GeoResult[] Results { get; set; }
        }



        static void Main(string[] args)
        {
            

            //REVERSE GEOCODING
            //string url = "http://maps.googleapis.com/maps/api/geocode/" +
            //   "json?latlng=40.714224,-73.961452&sensor=false";

            //ANFRAGE FUNKTION
            string url = "http://maps.googleapis.com/maps/api/geocode/" +
                "json?address=1600+Amphitheatre+Parkway,+Mountain+View,+CA&sensor=false";

            WebResponse response = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                response = request.GetResponse();
                if (response != null)
                {
                    string str = null;
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader streamReader = new StreamReader(stream))
                        {
                            str = streamReader.ReadToEnd();
                        }
                    }

                    GeoResponse geoResponse = JsonConvert.DeserializeObject<GeoResponse>(str);
                    if (geoResponse.Status == "OK")
                    {
                        int count = geoResponse.Results.Length;
                        for (int i = 0; i < count; i++)
                        {
                            Console.WriteLine("Lat: {0}", geoResponse.Results[i].Geometry.Location.Lat);
                            Console.WriteLine("Lng: {0}", geoResponse.Results[i].Geometry.Location.Lng);
                        }
                    }
                    else
                    {
                        Console.WriteLine("JSON response failed, status is '{0}'", geoResponse.Status);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("Clean up");
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
            }

            Console.WriteLine("Done.");
            Console.ReadLine();
        }
    }
}
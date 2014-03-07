using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Lucuma.Libs.Config;
using Lucuma.Helper;

namespace Lucuma.Libs
{
    public class MapQuestConfig : GeoProviderConfig, IGeoProviderConfig
    {

        public override void SetDefaults()
        {
            UrlPattern = "http://www.mapquestapi.com/geocoding/v1/address?inFormat=kvp&outFormat=json"; // default
            this.SetKeyQuery("key");
            this.SetSearchQuery("location");
        }

    }

    public class MapQuestMap : GeoProvider,IGeoProvider
    {
        
        public MapQuestMap()
        {
            Config = new MapQuestConfig();
        }
        public MapQuestMap(IGeoProviderConfig bconfig)
        {
            Config = bconfig;
        }

        public IGeoCodeResult GetCoordinates(string search)
        {
            GeoCodeResult gResult = new GeoCodeResult();
            gResult.Library = this.GetType().ToString();
            if (!String.IsNullOrEmpty(Config.Key))
            {
                try
                {
                    string url = BuildSearchQuery(search); //135+pilkington+avenue,+birmingham
                    string json = new WebRequester(url).GetData();
                    if (!String.IsNullOrEmpty(json))
                    {
                        JObject result = (JObject)JsonConvert.DeserializeObject(json);
                        if (result["results"][0]["locations"].Count() > 0)
                        {
                            foreach (var loc in result["results"][0]["locations"])
                            {
                                try
                                {
                                    Location Location = new Location();
                                    var latLng = loc["latLng"];
                                    if (latLng != null)
                                    {
                                        Location.Latitude = latLng["lat"].GetDoubleValue();
                                        Location.Longitude = latLng["lng"].GetDoubleValue();

                                    }
                                    
                                    Location.Accuracy = loc["geocodeQuality"].GetStringValue();

                                    String[] AddressTemp = new string[]
                                    {
                                        loc["street"].GetStringValue(),
                                        loc["adminArea5"].GetStringValue(),
                                        loc["adminArea4"].GetStringValue(),
                                        loc["postalCode"].GetStringValue(),
                                        loc["adminArea1"].GetStringValue()

                                    };

                                    Location.Type = loc["type"].GetStringValue();
                                    Location.Address = String.Join(",", AddressTemp.Where(s => !String.IsNullOrEmpty(s)));
                                    Location.Country = loc["adminArea1"].GetStringValue();
                                    Location.Name = Location.Address;

                                    gResult.Locations.Add(Location);
                                }
                                catch
                                {

                                }
                            }
                         
                        }
                        //gResult.Count = 1;

                    }

                }
                catch
                {

                }
            }
            return gResult;
        }
    }
}


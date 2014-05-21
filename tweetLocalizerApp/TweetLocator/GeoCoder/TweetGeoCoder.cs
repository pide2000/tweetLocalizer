using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tweetLocalizerApp.TweetLocator.GeoCoder;
using System.Data.Entity.Spatial;
using System.Diagnostics;
using tweetLocalizerApp.Libs;

namespace tweetLocalizerApp.TweetLocator
{

    public class TweetGeoCoder : IGeoCoder
    {
        public IGeographyData locate(float longitude, float latitude)
        {
            throw new NotImplementedException();
        }
        // todo: Return value shouldnt be a list of int's instead a new geoobject should be sufficient. Be careful: There shouldnt be a dependency to the ef classes! 

        public List<int> locateGeonames(double longitude, double latitude, GeonamesDataEntities geoNamesDB, GeographyData geographyData)
        {

            //geoNamesDB.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);

            var nearestCitiesList = (from geoNamesEntry in geoNamesDB.getNearestCities5000(longitude, latitude, 1)
                               select geoNamesEntry).Take(1).ToList();

            //var sql = nearestCity.ToString();
            //System.Console.WriteLine(sql);
            var country_code = nearestCitiesList[0].country_code.Trim();
            var admin1_code = country_code + "." + nearestCitiesList[0].admin1_code;
            var admin2_code = admin1_code + "." + nearestCitiesList[0].admin2_code;

            var countryId = (from countryinf in geoNamesDB.countryinfo
                             where countryinf.iso_alpha2.Equals(country_code)
                             select countryinf.geonameId).Take(1).ToList();

            var admin1Id = (from admin1code in geoNamesDB.admin1Codes
                             where admin1code.ID.Equals(admin1_code)
                            select admin1code.geonamesId).Take(1).ToList();

            var admin2Id = (from admin2code in geoNamesDB.admin2Codes
                            where admin2code.ID.Equals(admin2_code)
                            select admin2code.geonamesId).Take(1).ToList();

            List<int> idListe = new List<int>();
            
            idListe.Add(nearestCitiesList[0].geonameID.Value);

            geographyData.geonamesId = nearestCitiesList[0].geonameID.Value;
            geographyData.distance = nearestCitiesList[0].distance.Value;

            if (countryId.Count>0)
            {
                idListe.Add(countryId[0].Value);
                geographyData.countryId = countryId[0].Value;
            }
            else { 
                idListe.Add(0);
                geographyData.countryId = null;
            }
            if (admin1Id.Count>0)
            {
                idListe.Add(admin1Id[0].Value);
                geographyData.admin1Id = admin1Id[0].Value;
            }
            else
            {
                idListe.Add(0);
                geographyData.admin1Id = null;
            }
            if (admin2Id.Count>0)
            {
                idListe.Add(admin2Id[0].Value);
                geographyData.admin2Id = admin2Id[0].Value;
            }
            else
            {
                idListe.Add(0);
                geographyData.admin2Id = null;
            }

            return idListe;
        }
            
    }
}

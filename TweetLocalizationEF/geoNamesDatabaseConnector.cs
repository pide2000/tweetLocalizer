using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TweetLocalizationEF;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
namespace TweetLocalizationEF
{

    class searchResultAlternateNames
    {
        public String alternateName { get; set; }
        public int? geoNameId { get; set; }
    }

    class searchResultGeoNamesSmall
    {
        public String alternateName { get; set; }
        public int? geoNameId { get; set; }
        public String country { get; set; }
    }

    class GeoNameLocal : GeoNames
    {

    }

    class Tweet : tweetRandomSample
    {
    
    }


    class alternateNamesLocal : alternateNames {
    
    
    }

    class geoNamesDatabaseConnection
    {

        public String getLocationFieldById(int id)
        {
            String location;
            using (GeonamesDataEntities1 DB = new GeonamesDataEntities1())
            {
                var query = from tweet in DB.tweetRandomSample
                            where tweet.id == id
                            select tweet;
                tweetRandomSample tw = query.First<tweetRandomSample>();
                location = tw.place;
            }
            return location;
        }



        public List<Tweet> getTweetById(int id) {
            using (GeonamesDataEntities1 DB = new GeonamesDataEntities1()) {
                var query = (from tweet in DB.tweetRandomSample
                            where tweet.id.Equals(id)
                            select new Tweet
                            {
                                id = tweet.id,
                                tweetid = tweet.id,
                                country_code = tweet.country_code,
                                geotagged = tweet.geotagged,
                                lat = tweet.lat,
                                lon = tweet.lon,
                                place = tweet.place,
                                status = tweet.status,
                                processed = tweet.processed,
                                skipped = tweet.skipped,
                                time = tweet.time,
                                tweetTime = tweet.time,
                                username = tweet.username

                            });
                List<Tweet> tweetList = query.ToList();
                return tweetList;
            }
        
        }

        public void addNode(string parentNodeId, string nodeText) {
            using (GeonamesDataEntities1 DB = new GeonamesDataEntities1()) {
                DB.insertNode_TokenTreeGeoNames(nodeText, parentNodeId);
            }
        }

        public void addNodeId_GeoNamesId(int geonamesId, string nodeId) {
            try
            {
                using (GeonamesDataEntities1 DB = new GeonamesDataEntities1())
                {
                    DB.addNodeToGeonames(geonamesId, nodeId);
                }
            }
            catch (Exception ex) {
                System.Console.WriteLine("ERROR: " + ex.Message);
                if (ex.InnerException!=null) {
                    System.Console.WriteLine("Inner Exception Message: "+ ex.InnerException.Message);
                
                }
            }

        }

        public List<String> getNodeIdString(string tokenText, string parentId) {
            using (GeonamesDataEntities1 DB = new GeonamesDataEntities1())
            {
                var query = DB.getNodeIdString(parentId,tokenText);
                List<getNodeIdString_Result> liste = query.ToList();
                List<string> resultList = new List<string>();
                foreach(var str in liste){
                    resultList.Add(str.HierarchyString);
                }
                return resultList;
            }

           
        }


        /**
         * As value for pattern, this function accepts the same pattern as the LIKE Operator of transact-sql 
         * see: http://technet.microsoft.com/en-us/library/ms179859.aspx
         * 
         **/

        public List<Tweet> getTweetsWhereCharacterIsLike(String pattern)
        {
            using (GeonamesDataEntities1 DB = new GeonamesDataEntities1())
            {
                var query = (from tweet in DB.getTweetsWhereLocationLike(pattern)
                             select new Tweet
                             {
                                 id = tweet.id,
                                 tweetid = tweet.id,
                                 country_code = tweet.country_code,
                                 geotagged = tweet.geotagged,
                                 lat = tweet.lat,
                                 lon = tweet.lon,
                                 place = tweet.place,
                                 status = tweet.status,
                                 processed = tweet.processed,
                                 skipped = tweet.skipped,
                                 time = tweet.time,
                                 tweetTime = tweet.time,
                                 username = tweet.username

                             });
                List<Tweet> tweetList = query.ToList();
                return tweetList;
            }

        }

        public String getCountryCodeById(int id)
        {
            String country_code;
            using (GeonamesDataEntities1 DB = new GeonamesDataEntities1())
            {
                var query = from tweet in DB.tweetRandomSample
                            where tweet.id == id
                            select tweet;
                tweetRandomSample tw = query.First<tweetRandomSample>();
                country_code = tw.country_code;
            }
            return country_code;
        }

        public void freeTextSearch_AlternateNames_alternateName(String searchText)
        {
            using (GeonamesDataEntities1 DB = new GeonamesDataEntities1())
            {
                var query =
                    from name in DB.freetext_search_alternateNames_alternateName(searchText)
                    select name;
            }

        }

        public List<String> like_search_GeoNamesRestricted_name(String searchText,String whereRestr)
        {

            using (GeonamesDataEntities1 DB = new GeonamesDataEntities1())
            {
                var query =
                    (from obj in DB.like_search_GeoNamesRestricted_name(searchText,whereRestr)
                     select obj.country_code) ;
                List<String> ls = query.ToList();
                
                return ls;
            }

        }

        public List<String> like_search_GeoNamesRestricted_name_allFeature(String searchText)
        {

            using (GeonamesDataEntities1 DB = new GeonamesDataEntities1())
            {
                var query =
                    (from obj in DB.like_search_GeoNamesRestricted_name_allFeature(searchText)
                     select obj.country_code);
                List<String> ls = query.ToList();

                return ls;
            }

        }


        public List<alternateNamesLocal> like_search_alternateNames_name_allFeature(String searchText)
        {

            using (GeonamesDataEntities1 DB = new GeonamesDataEntities1())
            {
                var query =
                    (from obj in DB.like_search_alternateNames_name_allFeature(searchText)
                     select new alternateNamesLocal {
                        alternateNameId = obj.alternateNameId,
                        geoNameId = obj.geoNameId,
                        alternateName = obj.alternateName,
                        isColloquial = obj.isColloquial,
                        isHistoric = obj.isHistoric,
                        isoLanguage = obj.isoLanguage,
                        isPrefferedName = obj.isPrefferedName,
                        isShortName = obj.isShortName
                     });
                List<alternateNamesLocal> ls = query.ToList();

                return ls;
            }

        }


        public List<GeoNameLocal> like_search_GeoNamesRestricted_name_allFeature_(String searchText)
        {

            using (GeonamesDataEntities1 DB = new GeonamesDataEntities1())
            {
                var query =
                    (from obj in DB.like_search_GeoNamesRestricted_name_allFeature(searchText)
                     select new GeoNameLocal
                     {
                         geonameid = (int)obj.geonameid,
                         name = obj.name,
                         latitude = obj.latitude,
                         longitude = obj.longitude,
                         feature_class = obj.feature_class,
                         feature_code = obj.feature_code,
                         country_code = obj.country_code,
                         cc2 = obj.cc2,
                         admin1_code = obj.admin1_code,
                         admin2_code = obj.admin2_code,
                         admin3_code = obj.admin3_code,
                         admin4_code = obj.admin4_code,
                         population = obj.population,
                         elevation = obj.elevation,
                         gtopo30 = obj.gtopo30
                     });
                List<GeoNameLocal> ls = query.ToList();
                return ls;
            }

        }

        public void updateUniqueLocationFromTweetSample(uniqueLocationFromTweetSample updateEntry)
        {

            using (GeonamesDataEntities1 DB = new GeonamesDataEntities1())
            {

                try
                {
                    if (DB.uniqueLocationFromTweetSample.Any(o => o.id == updateEntry.id))
                    {
                        uniqueLocationFromTweetSample c = DB.uniqueLocationFromTweetSample.First(o => o.id == updateEntry.id);

                        if (updateEntry.gmaps != c.gmaps)
                        {
                            c.gmaps = updateEntry.gmaps;
                            DB.Entry(c).Property(e => e.gmaps).IsModified = true;
                        }
                        else if (updateEntry.bingmaps != c.bingmaps)
                        {
                            c.bingmaps = updateEntry.bingmaps;
                            DB.Entry(c).Property(e => e.bingmaps).IsModified = true;
                        }
                        else if (updateEntry.mapquest != c.mapquest)
                        {
                            c.mapquest = updateEntry.mapquest;
                            DB.Entry(c).Property(e => e.mapquest).IsModified = true;
                        }
                        else if (updateEntry.completelyProcessed != c.completelyProcessed)
                        {
                            c.completelyProcessed = updateEntry.completelyProcessed;
                            DB.Entry(c).Property(e => e.completelyProcessed).IsModified = true;
                        }


                    }
                    DB.SaveChanges();
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("{0} Exception thrown", e);
                }
            }
        }

        public void addListToUniqueLocationFromTweetSample(List<uniqueLocationFromTweetSample> resolutionList)
        {
            int i = 0;
            using (GeonamesDataEntities1 DB = new GeonamesDataEntities1())
            {
                var resListSize = resolutionList.Count();
                try
                {
                    DB.Configuration.AutoDetectChangesEnabled = false;
                    DB.Configuration.ValidateOnSaveEnabled = false;
                    foreach (uniqueLocationFromTweetSample res in resolutionList)
                    {
                        i++;
                        if (DB.uniqueLocationFromTweetSample.Any(o => o.id == res.id))
                        {
                            uniqueLocationFromTweetSample c = DB.uniqueLocationFromTweetSample.First(o => o.id == res.id);

                            if (res.gmaps != null)
                            {
                                c.gmaps = res.gmaps;
                                DB.Entry(c).Property(e => e.gmaps).IsModified = true;
                            }
                            else if (res.bingmaps != null)
                            {
                                c.bingmaps = res.bingmaps;
                                DB.Entry(c).Property(e => e.bingmaps).IsModified = true;
                            }
                            else if (res.mapquest != null)
                            {
                                c.mapquest = res.mapquest;
                                DB.Entry(c).Property(e => e.mapquest).IsModified = true;
                            }
                            else if (res.completelyProcessed != null)
                            {
                                c.completelyProcessed = res.completelyProcessed;
                                DB.Entry(c).Property(e => e.completelyProcessed).IsModified = true;
                            }
                         

                        }
                        else
                        {
                            DB.uniqueLocationFromTweetSample.Add(res);
                        }
                        if ((i % 1000) == 0)
                        {
                            System.Console.WriteLine("Saved " + i + " rows");
                            DB.SaveChanges();
                        }
                    }

                    DB.SaveChanges();
                }


                finally
                {
                    DB.Configuration.AutoDetectChangesEnabled = true;
                    DB.Configuration.ValidateOnSaveEnabled = true;
                }
            }
        }
       
        public void addListToCountryResolutionOutcome(List<countryResolutionOutcome> resolutionList)
        {
            int i = 0;
            using (GeonamesDataEntities1 DB = new GeonamesDataEntities1())
            {
                var resListSize = resolutionList.Count();
                try
                {
                    DB.Configuration.AutoDetectChangesEnabled = false;
                    DB.Configuration.ValidateOnSaveEnabled = false;
                    foreach (countryResolutionOutcome res in resolutionList)
                    {
                        i++;
                        if (DB.countryResolutionOutcome.Any(o => o.tweetRandomSample_Id == res.tweetRandomSample_Id))
                        {
                            countryResolutionOutcome c = DB.countryResolutionOutcome.First(o => o.tweetRandomSample_Id == res.tweetRandomSample_Id);
                            
                            if(res.country1!=c.country1){
                                c.country1 = res.country1;
                                DB.Entry(c).Property(e => e.country1).IsModified = true;
                            }
                            else if (res.country2 != c.country2)
                            {
                                c.country2 = res.country2;
                                DB.Entry(c).Property(e => e.country2).IsModified = true;
                            }
                            else if(res.country3!=c.country3) {
                                c.country3 = res.country3;
                                DB.Entry(c).Property(e => e.country3).IsModified = true;
                            }
                            else if(res.country4!=c.country4) {
                                c.country4 = res.country4;
                                DB.Entry(c).Property(e => e.country4).IsModified = true;
                            }
                            else if(res.country5!=c.country5) {
                                c.country5 = res.country5;
                                DB.Entry(c).Property(e => e.country5).IsModified = true;
                            }
                            else if(res.country6 != c.country6) {
                                c.country6 = res.country6;
                                DB.Entry(c).Property(e => e.country6).IsModified = true;
                            }
                            else if(res.country7 != c.country7) {
                                c.country7 = res.country7;
                                DB.Entry(c).Property(e => e.country7).IsModified = true;
                            }
                            else if(res.country8 != c.country8) {
                                c.country8 = res.country8;
                                DB.Entry(c).Property(e => e.country8).IsModified = true;
                            }
                            else if(res.country9 != c.country9) {
                                c.country9 = res.country9;
                                DB.Entry(c).Property(e => e.country9).IsModified = true;
                            }
                            else if (res.country10 != c.country10) {
                                c.country10 = res.country10;
                                DB.Entry(c).Property(e => e.country10).IsModified = true;
                            }
                            else if (res.country11 != c.country11) {
                                c.country11 = res.country11;
                                DB.Entry(c).Property(e => e.country11).IsModified = true;
                            }
                            else if (res.country12 != c.country12) {
                                c.country12 = res.country12;
                                DB.Entry(c).Property(e => e.country12).IsModified = true;
                            }
                            else if (res.country13 != c.country13) {
                                c.country13 = res.country13;
                                DB.Entry(c).Property(e => e.country13).IsModified = true;
                            }
                            else if (res.country14 != c.country14) {
                                c.country14 = res.country14;
                                DB.Entry(c).Property(e => e.country14).IsModified = true;
                            }
                            else if (res.country15 != c.country15) {
                                c.country15 = res.country15;
                                DB.Entry(c).Property(e => e.country15).IsModified = true;
                            }
                            else if (res.country16 != c.country16)
                            {
                                c.country16 = res.country16;
                                DB.Entry(c).Property(e => e.country16).IsModified = true;
                            }
                            else if (res.country17 != c.country17)
                            {
                                c.country17 = res.country17;
                                DB.Entry(c).Property(e => e.country17).IsModified = true;
                            }
                            else if (res.country18 != c.country18)
                            {
                                System.Console.WriteLine("Save id: " + res.tweetRandomSample_Id + " country " + res.country18);
                                c.country18 = res.country18;
                                DB.Entry(c).Property(e => e.country18).IsModified = true;
                            }
                          
                        }
                        else
                        {
                            DB.countryResolutionOutcome.Add(res);
                        }
                        if ((i % 1000) == 0)
                        {
                            System.Console.WriteLine("Saved " + i + " rows");
                            DB.SaveChanges();
                        }
                    }

                    DB.SaveChanges();
                }


                finally
                {
                    DB.Configuration.AutoDetectChangesEnabled = true;
                    DB.Configuration.ValidateOnSaveEnabled = true;
                }
            }
        }

        public List<GeoNameLocal> textSearch_GeoNames_name(String searchText)
        {
            
            using (GeonamesDataEntities1 DB = new GeonamesDataEntities1())
            {
                var query =
                    (from obj in DB.fulltext_search_GeoNames_name(searchText)
                    select new GeoNameLocal {
                        geonameid = (int)obj.geonameid,
                        name = obj.name,
                        latitude = obj.latitude,
                        longitude = obj.longitude,
                        feature_class = obj.feature_class,
                        feature_code = obj.feature_code,
                        country_code = obj.country_code,
                        cc2 = obj.cc2,
                        admin1_code = obj.admin1_code,
                        admin2_code = obj.admin2_code,
                        admin3_code = obj.admin3_code,
                        admin4_code = obj.admin4_code,
                        population = obj.population,
                        elevation = obj.elevation,
                        gtopo30 = obj.gtopo30
                    });
                List<GeoNameLocal> ls = query.ToList();
                return ls;
            }

        }

        public List<GeoNameLocal> nearestNeighbourSearch_GeoNamesById(int id, int resultCount)
        {
            using (GeonamesDataEntities1 DB = new GeonamesDataEntities1())
            {
                var query =
                    (from nearestPoint in DB.getNearestNeighboursById(id, resultCount)
                     select new GeoNameLocal
                     {
                         geonameid = (int)nearestPoint.geonameID,
                         name = nearestPoint.name,
                         latitude = nearestPoint.latitude,
                         longitude = nearestPoint.longitude,
                         feature_class = nearestPoint.feature_class,
                         feature_code = nearestPoint.feature_code,
                         country_code = nearestPoint.country_code,
                         cc2 = nearestPoint.cc2,
                         admin1_code = nearestPoint.admin1_code,
                         admin2_code = nearestPoint.admin2_code,
                         admin3_code = nearestPoint.admin3_code,
                         admin4_code = nearestPoint.admin4_code,
                         population = nearestPoint.pop,
                         elevation = nearestPoint.elevation,
                         gtopo30 = nearestPoint.gtopo30
                     });
                List<GeoNameLocal> ls = query.ToList();
                return ls;
            }

        }

        public List<T> executeQuery<T>(string query) {
            using (var context = new GeonamesDataEntities1())
            {
                var objectContext = ((IObjectContextAdapter)context).ObjectContext;
                var tweetCollection = objectContext.ExecuteStoreQuery<T>(query);
                List<T> tweetCollectionList = new List<T>(tweetCollection);
                System.Console.WriteLine(tweetCollectionList.Count() + " rows received from Database");
                return tweetCollectionList;
            }
        
        }

        public List<GeoNameLocal> nearestNeighbourSearch_GeoNamesByLonLat(float lon, float lat, int resultCount)
        {
            using (GeonamesDataEntities1 DB = new GeonamesDataEntities1())
            {
                var query =
                    (from nearestPoint in DB.getNearestNeighboursByCoordinates(lon, lat, resultCount)
                     select new GeoNameLocal
                     {
                         geonameid = (int)nearestPoint.geonameID,
                         name = nearestPoint.name,
                         latitude = nearestPoint.latitude,
                         longitude = nearestPoint.longitude,
                         feature_class = nearestPoint.feature_class,
                         feature_code = nearestPoint.feature_code,
                         country_code = nearestPoint.country_code,
                         cc2 = nearestPoint.cc2,
                         admin1_code = nearestPoint.admin1_code,
                         admin2_code = nearestPoint.admin2_code,
                         admin3_code = nearestPoint.admin3_code,
                         admin4_code = nearestPoint.admin4_code,
                         population = nearestPoint.pop,
                         elevation = nearestPoint.elevation,
                         gtopo30 = nearestPoint.gtopo30
                     });
                List<GeoNameLocal> ls = query.ToList();
                return ls;
            }

        }

        public List<searchResultAlternateNames> textSearch_AlternateNames_alternateNames(String searchText)
        {
            using (GeonamesDataEntities1 DB = new GeonamesDataEntities1())
            {
                var query =
                    (from database in DB.alternateNames
                     where database.alternateName == searchText
                     select new searchResultAlternateNames
                     {
                         alternateName = database.alternateName,
                         geoNameId = database.geoNameId
                     });

                List<searchResultAlternateNames> ls = query.ToList();

                return ls;
            }
        }

        public List<Tweet> getTweetsRange(int lowerBound, int upperBound) { 
        using (GeonamesDataEntities1 DB = new GeonamesDataEntities1())
        {
            var query = (from tweet in DB.tweetRandomSample.OrderBy(c => c.tweetid).Skip(lowerBound).Take((upperBound-lowerBound))
                        select new Tweet{
                                 id = tweet.id,
                                 tweetid = tweet.id,
                                 country_code = tweet.country_code,
                                 geotagged = tweet.geotagged,
                                 lat = tweet.lat,
                                 lon = tweet.lon,
                                 place = tweet.place,
                                 status = tweet.status,
                                 processed = tweet.processed,
                                 skipped = tweet.skipped,
                                 time = tweet.time,
                                 tweetTime = tweet.time,
                                 username = tweet.username

                             });
            List<Tweet> ls = query.ToList();
            return ls;
            }
        }


        public List<Tweet> getAllTweetsRandomSample()
        {
            using (GeonamesDataEntities1 DB = new GeonamesDataEntities1())
            {
                var query = (from tweet in DB.tweetRandomSample
                             select new Tweet
                             {
                                 id = tweet.id,
                                 tweetid = tweet.id,
                                 country_code = tweet.country_code,
                                 geotagged = tweet.geotagged,
                                 lat = tweet.lat,
                                 lon = tweet.lon,
                                 place = tweet.place,
                                 status = tweet.status,
                                 processed = tweet.processed,
                                 skipped = tweet.skipped,
                                 time = tweet.time,
                                 tweetTime = tweet.time,
                                 username = tweet.username

                             });
                List<Tweet> ls = query.ToList();
                return ls;
            }
        }

        public List<searchResultGeoNamesSmall> textSearch_GeoNames_alternateNames(String searchText)
        {
            using (GeonamesDataEntities1 DB = new GeonamesDataEntities1())
            {
                var query =
                    (from name in DB.GeoNames
                     where name.alternatenames == searchText
                     select new searchResultGeoNamesSmall
                     {
                         alternateName = name.alternatenames,
                         geoNameId = name.geonameid,
                         country = name.country_code
                     });

                List<searchResultGeoNamesSmall> ls = query.ToList();

                return ls;
            }
        }
    }
}

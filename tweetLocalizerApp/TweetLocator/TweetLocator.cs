using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tweetLocalizerApp.Libs.Locator;
using tweetLocalizerApp.TweetLocator;
using tweetLocalizerApp.Libs;
using System.Diagnostics;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using LinqToTwitter;
using System.Data.Entity.Spatial;


namespace tweetLocalizerApp.TweetLocator
{
    class TweetLoc : ILocator<TweetInformation>
    {
        public List<string> nGrams { get; set; }

        UserlocationTokenGenerator<string> userlocationTokenGenerator = new UserlocationTokenGenerator<string>();
        TimezoneTokenGenerator<string> timezoneTokenGenerator = new TimezoneTokenGenerator<string>();
        TweetNGramGenerator ngramGenerator = new TweetNGramGenerator();
        TweetGeoCoder geoCoder = new TweetGeoCoder();
        GeonamesDataEntities geonamesDB = new GeonamesDataEntities();
        
        knowledgeObjects knowledgeDB = new knowledgeObjects();
        
        public StatisticsData statistics = new StatisticsData();
        //a combination of the idetifiying properties ngram and geoentityId
        public Dictionary<string, HashSet<int>> knowledgeBaseIdentifierList = new Dictionary<string, HashSet<int>>();
        //a combination of the identifying properties ngramItem and ngramType
        public HashSet<Tuple<string, string>> nGramItemIdentifierList = new HashSet<Tuple<string, string>>();
        //the idetifying property of baseData, its just the basedataId
        public HashSet<int> baseDataIdentifierList = new HashSet<int>();
        
        Stopwatch globalstopwatch = new Stopwatch();
        List<KnowledgeBase> knowledgeBaseLocalList = new List<KnowledgeBase>();
        List<NGramItems> ngramItemsLocalList = new List<NGramItems>();
        List<BaseData> baseDataLocalList = new List<BaseData>();
        int learnCallCounter = new int();
        int localBulkInsertSize = new int();
        


        public TweetLoc(int bulkInsertSize) {

            //instantiate Preprocessors, tokenizer, encoder, orderer
            DeleteSigns delSigns = new DeleteSigns();
            CheckGeolocation checkGL = new CheckGeolocation();
            ToLowerCase toLowerCase = new ToLowerCase();
            StandardOrderer orderAlphanumeric = new StandardOrderer();
            Standardtokenizer tokenizer = new Standardtokenizer();
            StandardEncoder encoder = new StandardEncoder();
            tokenizer.seperator = ':';
            

            // Add the Preprocessors to the List, the preprocessors will be executed in this order. 
            List<IPreprocessor<string>> userlocationPreprocessorList = new List<IPreprocessor<string>>();
            userlocationPreprocessorList.Add(delSigns);
            userlocationPreprocessorList.Add(checkGL);
            userlocationPreprocessorList.Add(toLowerCase);

            // Add the Preprocessors to the List, the preprocessors will be executed in this order. 
            List<IPreprocessor<string>> timezonePreprocessorList = new List<IPreprocessor<string>>();
            timezonePreprocessorList.Add(delSigns);
            timezonePreprocessorList.Add(toLowerCase);
            
            //configure Userlocation Token Generator
            userlocationTokenGenerator.configure(userlocationPreprocessorList,tokenizer,encoder,orderAlphanumeric);

            //configure Timezone Token Generator
            timezoneTokenGenerator.configure(timezonePreprocessorList, tokenizer, encoder, orderAlphanumeric);


            //Getting all knowledge Entries in the Database. This is for Performance boost because EF6 is very slow
            globalWatchStartStop();
            var nGramItemsEntriesList = (from nGramItemEntry in knowledgeDB.NGramItems
                                 select new
                                 {
                                     nGramItemEntry.Id,
                                     nGramItemEntry.Item,
                                     nGramItemEntry.NGramItemType
                                 }).ToList();

            nGramItemsEntriesList.ForEach(c => nGramItemIdentifierList.Add(Tuple.Create(c.Item, c.NGramItemType)));
            
            var knowledgeBaseEntriesList = (from knowledgeEntry in knowledgeDB.KnowledgeBase
                               select new { 
                                    knowledgeEntry.Id,
                                    knowledgeEntry.NGram,
                                    knowledgeEntry.GeoNamesId
                               }).ToList();

            foreach (var item in knowledgeBaseEntriesList)
            {
                HashSet<int> tempGeoNameList;
                if (knowledgeBaseIdentifierList.TryGetValue(item.NGram, out tempGeoNameList))
                {
                    //eventually throw exception if the elemnt is present
                    tempGeoNameList.Add(item.GeoNamesId);
                    knowledgeBaseIdentifierList[item.NGram] = tempGeoNameList;
                }
                else {
                    tempGeoNameList = new HashSet<int>();
                    tempGeoNameList.Add(item.GeoNamesId);
                    knowledgeBaseIdentifierList.Add(item.NGram,tempGeoNameList);
                }

            }


            var baseDataEntriesList = (from baseDataEntry in knowledgeDB.BaseData
                                            select new
                                            {
                                                baseDataEntry.BaseDataId
                                            }).ToList();

            baseDataEntriesList.ForEach(c => baseDataIdentifierList.Add(c.BaseDataId));

            globalWatchStartStop("Get Lists " );
            

            //Database Configurations (for Performance)
            
                //geonamesDB.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
                //knowledgeDB.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
                knowledgeDB.Configuration.AutoDetectChangesEnabled = false;
                knowledgeDB.Configuration.ValidateOnSaveEnabled = false;
                ((IObjectContextAdapter)geonamesDB).ObjectContext.CommandTimeout = 180;
            
            learnCallCounter = 1;
            localBulkInsertSize = bulkInsertSize;

            //setting up twitter to tweet status
            PinAuthorizer tw = twitter();

        }

        public void globalWatchStartStop(string message = ""){
            if (globalstopwatch.IsRunning)
            {
                globalstopwatch.Stop();
                System.Console.WriteLine(message + globalstopwatch.Elapsed);
                globalstopwatch.Reset();
            }
            else
            {
                globalstopwatch.Start();
            }
            
        }

        private static PinAuthorizer twitter()
        {
            var auth = new PinAuthorizer()
            {

                CredentialStore = new InMemoryCredentialStore
                {
                    ConsumerKey = "Ssa9qRLFOUS8SdaD1TE0w",
                    ConsumerSecret = "sUcQ8oB7QJmXITjo8PGihGDxSbHrzCxmTs6BjVxlDo",
                    OAuthToken = "92306096-X3nzRe89yM1zvjX5nqsENEkmKViHqoxequa8ysRAw",
                    OAuthTokenSecret = "kYV8ZE3Px3PjeeOEjvo9VQZk37TktLXYM1UUJQCrta0yY"
                }
                //GoToTwitterAuthorization = pageLink => Process.Start(pageLink),
                //GetPin = () =>
                //{
                //    Console.WriteLine(
                //        "\nAfter authorizing this application, Twitter " +
                //        "will give you a 7-digit PIN Number.\n");
                //    Console.Write("Enter the PIN number here: ");
                //    return Console.ReadLine();
                //}
            };

            auth.AuthorizeAsync();

            return auth;
        }

        private static void statusUpdate(String tweetText, PinAuthorizer auth)
        {
            using (var twitter = new TwitterContext(auth))
            {
                var tweet = twitter.TweetAsync(tweetText);
            }


        } 


        /**
         * Flow:
         * 1. Create Working Objects
         * 2. 
         **/

        private void saveToDatabase(TweetKnowledgeObj tweetknowledge,int bulkInsertSize) {
            learnCallCounter++;

            BaseData baseDataItem = new BaseData();
            baseDataItem = getbaseDataObject((int)tweetknowledge.baseDataId);
            

            //create NgramItemsList
            foreach (var ngram in tweetknowledge.nGrams)
            {
                HashSet<int> tempGeoIdknowledgeIdList = new HashSet<int>();
                
                //check if the combination of ngram and geoid was already tracked, either in the database or in the current iterration
                
                bool checkforNgram = knowledgeBaseIdentifierList.TryGetValue(ngram.nGram,out tempGeoIdknowledgeIdList);
                if (checkforNgram&&tempGeoIdknowledgeIdList.Contains((int)tweetknowledge.geoEntityId))
                {
                    /**
                     * todo: there has to be one more case checked. ngram: "a;b ...... a;b" => bigram : a;b,..... a;b and first a is e.g userlocation whereas 2nd a is timezone
                     * in this case the ngramitems for the two bigrams arent of the same type. => wrong item list is the result. Really? 
                    **/
                    updateKnowledgeBaseEntry(tweetknowledge, ngram,baseDataItem);

                }else
                { 
                    List<NGramItems> items = persistNGramItems(knowledgeDB, ngram, ngramItemsLocalList);
                    //in persist just check if the basedata entry is already present and add it. 
                    //create the knowledgeBase Object to save

                    List<BaseData> baseDataIds = new List<BaseData>();
                    baseDataIds.Add(baseDataItem);

                    KnowledgeBase knowBase = new KnowledgeBase()
                    {
                        NGram = ngram.nGram,
                        NGramCount = 1,
                        GeoNamesId = (int)tweetknowledge.geoEntityId,
                        CountryId = tweetknowledge.countryId,
                        Admin1Id = tweetknowledge.admin1Id,
                        Admin2Id = tweetknowledge.admin2Id,
                        Admin3Id = tweetknowledge.admin3Id,
                        Admin4Id = tweetknowledge.admin4Id,
                        NGramItems = items,
                        BaseData = baseDataIds
                    };
                    
                    //just for actuality of the Identifier Lists. First check if ngram is available, if so add geoid. If not add compelte knowbase
                    if (checkforNgram)
                    {
                        tempGeoIdknowledgeIdList.Add((int)tweetknowledge.geoEntityId);
                        knowledgeBaseIdentifierList[ngram.nGram] = tempGeoIdknowledgeIdList;
                    }
                    else {
                        knowledgeBaseIdentifierList.Add(ngram.nGram,new HashSet<int>{(int)tweetknowledge.geoEntityId});
                    }

                    knowledgeBaseLocalList.Add(knowBase);
                    knowledgeDB.KnowledgeBase.Add(knowBase);
                }
                
            }
            if (learnCallCounter > bulkInsertSize)
            {
                globalWatchStartStop();
                learnCallCounter = 1;
                try
                {
                    knowledgeDB.SaveChanges();
                    knowledgeBaseLocalList = new List<KnowledgeBase>();
                    baseDataLocalList = new List<BaseData>();
                    ngramItemsLocalList = new List<NGramItems>();
                    
                    
                    }
                catch (Exception ex)
                {

                    System.Console.WriteLine(ex.Message);
                    if (ex.InnerException != null)
                    {
                        System.Console.WriteLine(ex.InnerException.Message);
                    }

                    throw;
                }
                globalWatchStartStop("save " );
            }
            
            }

     

        private BaseData getbaseDataObject(int basedataId){

                if (baseDataIdentifierList.Contains(basedataId))
                {
                    BaseData baseDataItemLocal = new BaseData(); 
                       baseDataItemLocal =  baseDataLocalList.Find(c => c.BaseDataId == basedataId);
                    //basedataObject is already Present determine if it is in the local list or in the database
                    if (baseDataItemLocal != null)
                    {
                        //base data object in local list
                       return baseDataItemLocal;
                    }
                    else
                    {
                        //base data object not in local List get it from Database
                        BaseData baseDataItemDB = new BaseData();
                        baseDataItemDB = knowledgeDB.BaseData.FirstOrDefault(c => c.BaseDataId == basedataId);
                        
                        return baseDataItemDB;
                    }
                }

                else
                {
                    BaseData tempBaseData = new BaseData() { BaseDataId = basedataId };
                    baseDataIdentifierList.Add(basedataId);
                    baseDataLocalList.Add(tempBaseData);
                    knowledgeDB.BaseData.Add(tempBaseData);
                    knowledgeDB.Entry(tempBaseData).State = System.Data.Entity.EntityState.Added;
                    return tempBaseData;
                }

        }


        private void updateKnowledgeBaseEntry(TweetKnowledgeObj tweetknowledge, Ngram ngram,BaseData baseDataItem)
        {
            KnowledgeBase tempKnowledgeBaseItemLocal = new KnowledgeBase();

            //important because automatic change detection is disabled
            
            knowledgeDB.ChangeTracker.DetectChanges();
            
            
            tempKnowledgeBaseItemLocal = knowledgeBaseLocalList.Find(c => c.GeoNamesId == tweetknowledge.geoEntityId && c.NGram.Equals(ngram.nGram));

            
            //check if in current iterration or already transferred to database
            if (tempKnowledgeBaseItemLocal != null)
            {
            //is local in the List
                
                tempKnowledgeBaseItemLocal.NGramCount += 1;
                tempKnowledgeBaseItemLocal.BaseData.Add(baseDataItem);
                }
            else
            {
            //is in database
                updateKnowledgeBaseDBEntry(ngram.nGram, (int)tweetknowledge.geoEntityId,baseDataItem);
            }
        }

        private void updateKnowledgeBaseDBEntry(string ngram,int geoentityId, BaseData baseDataItem)
        {
            
            var knowledgeBaseEntry = knowledgeDB.KnowledgeBase.SingleOrDefault(c => c.NGram.Equals(ngram)&&c.GeoNamesId == geoentityId);
            
            
            if(null==knowledgeBaseEntry.BaseData.SingleOrDefault(c=> c.BaseDataId == baseDataItem.BaseDataId)){
                knowledgeBaseEntry.BaseData.Add(baseDataItem);
                knowledgeBaseEntry.NGramCount += 1;
                
                knowledgeDB.Entry(knowledgeBaseEntry).State = System.Data.Entity.EntityState.Modified;
            }

            
            
           
        }

        private List<NGramItems> persistNGramItems(knowledgeObjects knowledgeDB, Ngram ngram, List<NGramItems> ngramItemsList)
        {
            List<NGramItems> items = new List<NGramItems>();

            foreach (var indicatortoken in ngram.nGramItems)
            {
                
                //check if it is already in the DB or ngramItemsList
                if (nGramItemIdentifierList.Contains(Tuple.Create(indicatortoken.Item2,indicatortoken.Item1)))
                {
                    NGramItems tempIndicatorItem = new NGramItems();
                    if ((tempIndicatorItem = ngramItemsList.Find(c => c.Item.Equals(indicatortoken.Item2) && c.NGramItemType.Equals(indicatortoken.Item1))) != null)
                    {
                        items.Add(tempIndicatorItem);
                    }
                    else
                    {
                        var indicatorItem = knowledgeDB.NGramItems.SingleOrDefault(c => c.Item.Equals(indicatortoken.Item2) && c.NGramItemType.Equals(indicatortoken.Item1));
                        items.Add(indicatorItem);
                    }
                }
                else
                {
                    NGramItems ngramItem = new NGramItems()
                    {
                        Item = indicatortoken.Item2,
                        NGramItemType = indicatortoken.Item1
                    };
                    items.Add(ngramItem);
                    nGramItemIdentifierList.Add(Tuple.Create(indicatortoken.Item2,indicatortoken.Item1));
                    ngramItemsList.Add(ngramItem);
                }

            }
            
            return items;
        } 

        public void learn(TweetInformation tweet)
        {
            TweetKnowledgeObj tweetKnowledge;
            GeographyData geogData;
            createTweetKnowledge(tweet, out tweetKnowledge, out geogData);

            //statistics.addDistances((double)geogData.distance);
            //statistics.addGeographyDataTweetKnowledge(geogData,tweetKnowledge);

            saveToDatabase(tweetKnowledge, localBulkInsertSize);
            
        }

        private void createTweetKnowledge(TweetInformation tweet, out TweetKnowledgeObj tweetKnowledge, out GeographyData geogData)
        {
            //create working and Result Objects
            tweetKnowledge = new TweetKnowledgeObj();

            geogData = new GeographyData();
            //add Data to work with
            tweetKnowledge.userlocation = tweet.userlocation;
            tweetKnowledge.timezone = tweet.timezone;
            tweetKnowledge.baseDataId = tweet.baseDataId;
            tweetKnowledge.longitude = tweet.longitude;
            tweetKnowledge.latitude = tweet.latitude;

            //create new Indicators with Information about the Type
            tweetKnowledge.userlocationIndicator = new UserlocationIndicator<string>("USERLOCATION", tweetKnowledge.userlocation);
            tweetKnowledge.timezoneIndicator = new TimezoneIndicator<string>("TIMEZONE", tweetKnowledge.timezone);

            //generate Tokens
            userlocationTokenGenerator.assemblyToken(tweetKnowledge.userlocationIndicator);
            timezoneTokenGenerator.assemblyToken(tweetKnowledge.timezoneIndicator);

            // set tokens in knowledge object
            // todo: make it a little more beautiful, hide this in the knwoledge Object..... somehow
            tweetKnowledge.indicatorTokens.Add(tweetKnowledge.userlocationIndicator.indicatorType, tweetKnowledge.userlocationIndicator.finalIndicatorTokens);
            tweetKnowledge.indicatorTokens.Add(tweetKnowledge.timezoneIndicator.indicatorType, tweetKnowledge.timezoneIndicator.finalIndicatorTokens);

            //create nGrams
            tweetKnowledge.nGrams = ngramGenerator.generateNGrams(tweetKnowledge.indicatorTokens, 3);

            List<int> geonamesIds = new List<int>();
            geonamesIds = geoCoder.locateGeonames(tweetKnowledge.longitude, tweetKnowledge.latitude, geonamesDB, geogData);

            tweetKnowledge.geoEntityId = geogData.geonamesId;
            tweetKnowledge.countryId = geogData.countryId;
            tweetKnowledge.admin1Id = geogData.admin1Id;
            tweetKnowledge.admin2Id = geogData.admin2Id;
            tweetKnowledge.admin3Id = null;
            tweetKnowledge.admin4Id = null;
        }

        public GeoNames locate(TweetInformation tweet)
        {
            GeoNames resultLocation = new GeoNames();
            //create working and Result Objects
            TweetKnowledgeObj tweetKnowledge;
            GeographyData geogData;
            createTweetKnowledge(tweet, out tweetKnowledge, out geogData);
            List<KnowledgeBase> possibleKnowledgeData = new List<KnowledgeBase>();

            possibleKnowledgeData = getKnowledgeBaseResults(tweetKnowledge);

            resultLocation = getBestMatchByCount(possibleKnowledgeData);

            return resultLocation;

        }

        public void saveLocateResults(TweetInformation tweet)
        {
            TweetKnowledgeObj tweetKnowledge;
            GeographyData geogData;
            createTweetKnowledge(tweet, out tweetKnowledge, out geogData);
            List<KnowledgeBase> possibleKnowledgeData = new List<KnowledgeBase>();

            possibleKnowledgeData = getKnowledgeBaseResults(tweetKnowledge);

            foreach (var obj in possibleKnowledgeData)
            {
                knowledgeBaseGeocoding dbentry = new knowledgeBaseGeocoding();
                GeoNames resolvedGeoName = new GeoNames();

                resolvedGeoName = (from geonames in geonamesDB.GeoNames
                                   where geonames.geonameid == obj.GeoNamesId
                                   select geonames).ToList().First();
                dbentry.coord_knowledgeBaseResolution = resolvedGeoName.coord;
                dbentry.coord_tweet = tweet.coord;
                dbentry.knowledgeBaseId = obj.Id;
                dbentry.tweetRandomSampleId = tweet.randomSampleId;
                knowledgeDB.knowledgeBaseGeocoding.Add(dbentry);
            }

            if (possibleKnowledgeData.Count == 0)
            {
                knowledgeBaseGeocoding dbentry = new knowledgeBaseGeocoding();
                dbentry.coord_knowledgeBaseResolution = null;
                dbentry.coord_tweet = tweet.coord;
                dbentry.knowledgeBaseId = null;
                dbentry.tweetRandomSampleId = tweet.randomSampleId;
                knowledgeDB.knowledgeBaseGeocoding.Add(dbentry);

            }



            knowledgeDB.SaveChanges();
        }

        private GeoNames getBestMatchByCount(List<KnowledgeBase> possibleKnowledgeData)
        {
            GeoNames resolvedGeoName = new GeoNames();
            int resolvedGeoNameId = new int();
            resolvedGeoNameId = 0;
            long maxCount = 0;

            foreach (var obj in possibleKnowledgeData) {
                if (obj.NGramCount > maxCount) {
                    maxCount = obj.NGramCount;
                    resolvedGeoNameId = obj.GeoNamesId;
                }
                if (resolvedGeoNameId > 0)
                {
                    resolvedGeoName = (from geonames in geonamesDB.GeoNames
                                       where geonames.geonameid == resolvedGeoNameId
                                       select geonames).ToList().First();
                }

            }
            return resolvedGeoName;

        }



        private List<KnowledgeBase> getKnowledgeBaseResults(TweetKnowledgeObj tweetKnowledge)
        {
            List<KnowledgeBase> possibleLocations = new List<KnowledgeBase>();

            foreach (Ngram ngram in tweetKnowledge.nGrams)
            {
                var possibilities = (from knowBase in knowledgeDB.KnowledgeBase
                                     where knowBase.NGram.Equals(ngram.nGram)
                                     select knowBase).ToList();

                if (possibilities != null)
                {
                    possibleLocations.AddRange(possibilities);
                }
            }

            return possibleLocations;
        }



        /**
         *Calculates the Statistics Data without tweetknowledge calculation.
          */
        public void getGeographyStatistics(TweetInformation tweet)
        {
            GeographyData geogData;
            TweetKnowledgeObj tweetKnowledge = new TweetKnowledgeObj();
            calculateGeographyData(tweet, out geogData);

            tweetKnowledge.baseDataId = tweet.baseDataId;
            tweetKnowledge.longitude = tweet.longitude;
            tweetKnowledge.latitude = tweet.latitude;
            List<int> geonamesIds = new List<int>();

            tweetKnowledge.geoEntityId = geogData.geonamesId;
            tweetKnowledge.countryId = geogData.countryId;
            tweetKnowledge.admin1Id = geogData.admin1Id;
            tweetKnowledge.admin2Id = geogData.admin2Id;
            tweetKnowledge.admin3Id = null;
            tweetKnowledge.admin4Id = null;

            statistics.addDistances((double)geogData.distance);
            statistics.addGeographyDataTweetKnowledge(geogData, tweetKnowledge);
        }

        private void calculateGeographyData(TweetInformation tweet, out GeographyData geogData)
        {
            geogData = new GeographyData();
            List<int> geonamesIds = new List<int>();
            geonamesIds = geoCoder.locateGeonames(tweet.longitude, tweet.latitude, geonamesDB, geogData);
        }



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tweetLocalizerApp.Libs.Locator;
using tweetLocalizerApp.TweetLocator;
using tweetLocalizerApp.Libs;
using System.Diagnostics;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using LinqToTwitter;


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
        Stopwatch globalstopwatch = new Stopwatch();
        List<KnowledgeBase> knowledgeBaseList = new List<KnowledgeBase>();
        List<NGramItems> ngramItemsList = new List<NGramItems>();
        int learnCallCounter = new int();
        


        public TweetLoc() {

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
            
            //configure Userlocation Token Generator
            userlocationTokenGenerator.configure(userlocationPreprocessorList,tokenizer,encoder,orderAlphanumeric);

            //configure Timezone Token Generator
            timezoneTokenGenerator.configure(userlocationPreprocessorList, tokenizer, encoder, orderAlphanumeric);


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
            globalWatchStartStop("Get Lists " );
            

            //Database Configurations (for Performance)
            
                //geonamesDB.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
                //knowledgeDB.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
                knowledgeDB.Configuration.AutoDetectChangesEnabled = false;
                knowledgeDB.Configuration.ValidateOnSaveEnabled = false;

                learnCallCounter = 0;

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

            //create NgramItemsList
            foreach (var ngram in tweetknowledge.nGrams)
            {
                HashSet<int> tempGeoIdknowledgeIdList = new HashSet<int>();
                
                if (knowledgeBaseIdentifierList.TryGetValue(ngram.nGram,out tempGeoIdknowledgeIdList)&&tempGeoIdknowledgeIdList.Contains((int)tweetknowledge.geoEntityId))
                {
                    //check if in local list or already transferred to database
                    KnowledgeBase tempKnowledgeBaseItem = new KnowledgeBase();
                    tempKnowledgeBaseItem = knowledgeBaseList.Find(c => c.GeoNamesId == tweetknowledge.geoEntityId && c.NGram.Equals(ngram.nGram));
                    if (tempKnowledgeBaseItem != null)
                    {
                        tempKnowledgeBaseItem.NGramCount += 1;
                    }
                    else
                    {
                        updateKnowledgeBase(ngram.nGram, (int)tweetknowledge.geoEntityId);
                    }
                }else
                {
                    
                    List<NGramItems> items = persistNGramItems(knowledgeDB, ngram,ngramItemsList);
                    
                    //create the knowledgeBase Object to save
                    
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
                        NGramItems = items
                    };
                    
                    //just for actuality of the Identifier Lists
                    if (knowledgeBaseIdentifierList.TryGetValue(ngram.nGram, out tempGeoIdknowledgeIdList))
                    {
                        tempGeoIdknowledgeIdList.Add((int)tweetknowledge.geoEntityId);
                        knowledgeBaseIdentifierList[ngram.nGram] = tempGeoIdknowledgeIdList;
                        
                    }
                    else {
                        knowledgeBaseIdentifierList.Add(ngram.nGram,new HashSet<int>{(int)tweetknowledge.geoEntityId});
                        
                    }

                    knowledgeBaseList.Add(knowBase);
                    knowledgeDB.KnowledgeBase.Add(knowBase);
                }
                
            }
            if (learnCallCounter > bulkInsertSize)
            {
                globalWatchStartStop();
                learnCallCounter = 0;
                try
                {
                    knowledgeDB.SaveChanges();
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

        private void updateKnowledgeBase(string ngram,int geoentityId)
        {
            
            var knowledgeBaseEntry = knowledgeDB.KnowledgeBase.SingleOrDefault(c => c.NGram.Equals(ngram)&&c.GeoNamesId == geoentityId);
            
            knowledgeBaseEntry.NGramCount += 1;
            
            knowledgeDB.KnowledgeBase.Attach(knowledgeBaseEntry);
            knowledgeDB.Entry(knowledgeBaseEntry).State = System.Data.Entity.EntityState.Modified;
           
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
            //create working and Result Objects
            TweetKnowledgeObj tweetKnowledge = new TweetKnowledgeObj();
            GeographyData geogData = new GeographyData();
            //add Data to work with
            tweetKnowledge.userlocation = tweet.userlocation;
            tweetKnowledge.timezone = tweet.timezone;
            tweetKnowledge.baseDataId = tweet.baseDataId;
            tweetKnowledge.longitude = tweet.longitude;
            tweetKnowledge.latitude = tweet.latitude;

            //create new Indicators with Information about the Type
            tweetKnowledge.userlocationIndicator = new UserlocationIndicator<string>("USERLOCATION",tweetKnowledge.userlocation);
            tweetKnowledge.timezoneIndicator = new TimezoneIndicator<string>("TIMEZONE",tweetKnowledge.timezone);

            //generate Tokens
            userlocationTokenGenerator.assemblyToken(tweetKnowledge.userlocationIndicator);
            timezoneTokenGenerator.assemblyToken(tweetKnowledge.timezoneIndicator);

            // set tokens in knowledge object
            // todo: make it a little more beautiful, hide this in the knwoledge Object..... somehow
            tweetKnowledge.indicatorTokens.Add(tweetKnowledge.userlocationIndicator.indicatorType, tweetKnowledge.userlocationIndicator.finalIndicatorTokens);
            tweetKnowledge.indicatorTokens.Add(tweetKnowledge.timezoneIndicator.indicatorType, tweetKnowledge.timezoneIndicator.finalIndicatorTokens);

            //create nGrams
            tweetKnowledge.nGrams = ngramGenerator.generateNGrams(tweetKnowledge.indicatorTokens,2);

            List<int> geonamesIds = new List<int>();
            
            geonamesIds = geoCoder.locateGeonames(tweetKnowledge.longitude, tweetKnowledge.latitude, geonamesDB, geogData);

            tweetKnowledge.geoEntityId = geogData.geonamesId;
            tweetKnowledge.countryId = geogData.countryId;
            tweetKnowledge.admin1Id = geogData.admin1Id;
            tweetKnowledge.admin2Id = geogData.admin2Id;
            tweetKnowledge.admin3Id = null;
            tweetKnowledge.admin4Id = null;

            statistics.addDistances((double)geogData.distance);
            statistics.addGeographyDataTweetKnowledge(geogData,tweetKnowledge);

            saveToDatabase(tweetKnowledge,100);
            
        }

        public void locate(TweetInformation tweet)
        {
            throw new NotImplementedException();
        }

        

       

        


    }
}

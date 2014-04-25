using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tweetLocalizerApp.Libs.Locator;
using tweetLocalizerApp.TweetLocator;
using tweetLocalizerApp.Libs;
using System.Diagnostics;


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
        public StatisticsData statistics = new StatisticsData();

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

            //A first query to speed up the DB querying process. On the first query, EF sends some meta data to sql server this drops performance significantly.   
            geonamesDB.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
            var nearestCity = (from geoNamesEntry in geonamesDB.countryCodes
                               select geoNamesEntry).Take(1);
            

        }

        /**
         * Flow:
         * 1. Create Working Objects
         * 2. 
         **/

        private void saveToDatabase(TweetKnowledgeObj tweetknowledge) {



            KnowledgeBase knowBase = new KnowledgeBase(){
                NGram = tweetknowledge.nGrams[0].nGram,
                NGramCount = 1,
                GeoNamesId = (int)tweetknowledge.geoEntityId,
                CountryId = tweetknowledge.countryId,
                Admin1Id = tweetknowledge.admin1Id,
                Admin2Id = tweetknowledge.admin2Id,
                Admin3Id = tweetknowledge.admin3Id,
                Admin4Id = tweetknowledge.admin4Id,
                
                
        };

            knowledgeObjects knowledgeDB = new knowledgeObjects();
            knowledgeDB.KnowledgeBase.Add(knowBase);
            knowledgeDB.SaveChanges();
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
            tweetKnowledge.nGrams = ngramGenerator.generateNGrams(tweetKnowledge.indicatorTokens,3);


            

            List<int> geonamesIds = new List<int>();

            
            geonamesIds = geoCoder.locateGeonames(tweetKnowledge.longitude, tweetKnowledge.latitude, geonamesDB, geogData);
            

            tweetKnowledge.geoEntityId = geonamesIds[0];
            tweetKnowledge.countryId = geonamesIds[1];
            tweetKnowledge.admin1Id = geonamesIds[2];
            tweetKnowledge.admin2Id = geonamesIds[3];
            tweetKnowledge.admin3Id = null;
            tweetKnowledge.admin4Id = null;

            statistics.addDistances((double)geogData.distance);
            statistics.addGeographyDataTweetKnowledge(geogData,tweetKnowledge);

            saveToDatabase(tweetKnowledge);
            
        }

        public void locate(TweetInformation tweet)
        {
            throw new NotImplementedException();
        }

        

       

        


    }
}

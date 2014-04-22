using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tweetLocalizerApp.Libs.Locator;
using tweetLocalizerApp.TweetLocator;
using tweetLocalizerApp.Libs;


namespace tweetLocalizerApp.TweetLocator
{
    class TweetLoc : ILocator<TweetInformation>
    {
        public List<string> nGrams { get; set; }

        UserlocationTokenGenerator<string> userlocationTokenGenerator = new UserlocationTokenGenerator<string>();
        TimezoneTokenGenerator<string> timezoneTokenGenerator = new TimezoneTokenGenerator<string>();
        TweetNGramGenerator ngramGenerator = new TweetNGramGenerator();
        TweetGeoCoder geoCoder = new TweetGeoCoder();
        
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

        }

        public void learn(TweetInformation tweet)
        {
            //create working and Result Objects
            TweetKnowledgeObj tweetKnowledge = new TweetKnowledgeObj();
            tweetKnowledge.userlocation = tweet.userlocation;
            tweetKnowledge.timezone = tweet.timezone;

            tweetKnowledge.userlocationIndicator = new UserlocationIndicator<string>("USERLOCATION",tweetKnowledge.userlocation);
            tweetKnowledge.timezoneIndicator = new TimezoneIndicator<string>("TIMEZONE",tweetKnowledge.timezone);

            //generate Tokens
            userlocationTokenGenerator.assemblyToken(tweetKnowledge.userlocationIndicator);
            timezoneTokenGenerator.assemblyToken(tweetKnowledge.timezoneIndicator);


            

            // todo: 
            // Set Informations of the Tweet. baseDataId, lon, lat, 
            // tweetKnowledge.baseDataId = tweet.

            //set tokens in knwoledge object
            // todo: make it a little more beautiful, hide this in the knwoledge Objetc..... somehow
            tweetKnowledge.indicatorTokens.Add(tweetKnowledge.userlocationIndicator.indicatorType, tweetKnowledge.userlocationIndicator.finalIndicatorTokens);
            tweetKnowledge.indicatorTokens.Add(tweetKnowledge.timezoneIndicator.indicatorType, tweetKnowledge.timezoneIndicator.finalIndicatorTokens);

            tweetKnowledge.nGrams= ngramGenerator.generateNGrams(tweetKnowledge.indicatorTokens,3);

            foreach (Ngram oj in tweetKnowledge.nGrams) {
                System.Console.WriteLine("-------------------");
                System.Console.WriteLine(oj.nGram );
                foreach (string type in oj.indicatorTypes) {
                    System.Console.WriteLine(type);
                }
            }

        }

        public void locate(TweetInformation tweet)
        {
            throw new NotImplementedException();
        }

        

       

        


    }
}

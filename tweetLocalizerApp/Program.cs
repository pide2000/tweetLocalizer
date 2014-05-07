using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tweetLocalizerApp.Libs;
using Microsoft.SqlServer.Server;
using tweetLocalizerApp.TweetLocator;
using System.Diagnostics;
using LinqToTwitter;

namespace tweetLocalizerApp
{

  
           

    class Program
    {

        public struct RoundedTimeSpan
    {

        private const int TIMESPAN_SIZE = 7; // it always has seven digits

        private TimeSpan roundedTimeSpan;
        private int precision;

        public RoundedTimeSpan(long ticks, int precision)
        {
            if (precision < 0) { throw new ArgumentException("precision must be non-negative"); }
            this.precision = precision;
            int factor = (int)System.Math.Pow(10, (TIMESPAN_SIZE - precision));

            // This is only valid for rounding milliseconds-will *not* work on secs/mins/hrs!
            roundedTimeSpan = new TimeSpan(((long)System.Math.Round((1.0 * ticks / factor)) * factor));
        }

        public TimeSpan TimeSpan { get { return roundedTimeSpan; } }

        public override string ToString()
        {
            return ToString(precision);
        }

        public string ToString(int length)
        { // this method revised 2010.01.31
            int digitsToStrip = TIMESPAN_SIZE - length;
            string s = roundedTimeSpan.ToString();
            if (!s.Contains(".") && length == 0) { return s; }
            if (!s.Contains(".")) { s += "." + new string('0', TIMESPAN_SIZE); }
            int subLength = s.Length - digitsToStrip;
            return subLength < 0 ? "" : subLength > s.Length ? s : s.Substring(0, subLength);
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




        static void Main(string[] args)
        {
            PinAuthorizer tw = twitter();

            

            using (TweetsDataEntities tweetDB = new TweetsDataEntities()) {
                tweetDB.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
                var tweetsCollection = (from tweets in tweetDB.learningBase orderby tweets.id 
                                        select tweets).ToList();

                Stopwatch stopwatch = new Stopwatch();
                TweetInformation ti = new TweetInformation();
                TweetLoc tl = new TweetLoc();
                TimeSpan timespan = new TimeSpan();
                TimeSpan actualTime = new TimeSpan();
                int i = 0;


                foreach (var item in tweetsCollection)
                {

                    i++;
                    stopwatch.Start();
                    ti = new TweetInformation();
                    ti.userlocation = item.userlocation;
                    ti.timezone = item.timezone;
                    ti.longitude = item.lon;
                    ti.latitude = item.lat;
                    ti.baseDataId = item.id;
                    tl.learn(ti);
                    stopwatch.Stop();
                    actualTime += stopwatch.Elapsed;
                    timespan += stopwatch.Elapsed;
                    stopwatch.Reset();
                    if (i % 100 == 0) {
                        string tweetTXT = i + " T " + new RoundedTimeSpan(timespan.Ticks,2) +" avg " + new RoundedTimeSpan(timespan.Ticks / i,2) + " avg1 "+ new RoundedTimeSpan(actualTime.Ticks / 100,2);
                        System.Console.WriteLine(tweetTXT);
                        statusUpdate("@pide2001 "+tweetTXT, tw);
                        
                        actualTime = TimeSpan.Zero;
                    }
                }


                //ti = new TweetInformation();
                //ti.userlocation = "atlantic";
                //ti.timezone = "zeitzone";
                //ti.longitude = -3.79908776283268;
                //ti.latitude = 43.4694290161133;
                //ti.baseDataId = 222;
                //tl.learn(ti);

                //ti = new TweetInformation();
                //ti.userlocation = "atlantic";
                //ti.timezone = "zeitzone";
                //ti.longitude = -3.79908776283268;
                //ti.latitude = 43.4694290161133;
                //ti.baseDataId = 14;
                //tl.learn(ti);

                //ti = new TweetInformation();
                //ti.userlocation = "a a a b b b c";
                //ti.timezone = "zeitzone";
                //ti.longitude = 107.026786804199;
                //ti.latitude = -6.21288728713989;
                //ti.baseDataId = 13;
                //tl.learn(ti);


                System.Console.WriteLine("Median " + tl.statistics.getMedianOfDistances());
                System.Console.WriteLine("Average " + tl.statistics.getAverageDistance());
                System.Console.WriteLine("Biggest " + tl.statistics.getBiggestDistance());
                System.Console.WriteLine("Smallest " + tl.statistics.getSmallestDistance());
                Tuple<GeographyData,TweetKnowledgeObj> know= tl.statistics.getBiggestDistanceAndInformation();
                System.Console.WriteLine("Biggest distance between" + know.Item1.geonamesId + " and " + know.Item2.baseDataId);

              

                

                
            
            }


            //Tweetinformation should be the database object of a tweet!!!!! TweetInfiormation class is just for testing purposes
            


           

            
            

#if DEBUG
            System.Console.WriteLine("Press any key to quit !");
            System.Console.ReadLine();
#endif

        }
    }
}

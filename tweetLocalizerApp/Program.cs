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
using tweetLocalizerApp.Helper;
using System.Collections;

namespace tweetLocalizerApp
{

    class Program
    {
        static void Main(string[] args)
        {
            PinAuthorizer tw = twitter();
            //learning(tw);
            statistics(tw);
        }

        private static void learning(PinAuthorizer tw)
        {
            System.Console.WriteLine("Please Type in how many LearningData should be taken (0 for all): ");
            int takeUserInput = Convert.ToInt32(Console.ReadLine());
            System.Console.WriteLine("Please Type in how many Datalines should be skipped (0 for none): ");
            int skipUserInput = Convert.ToInt32(Console.ReadLine());
            System.Console.WriteLine("Please choose the amount of Data which should be saved to the Database in one step: ");
            int bulkInsertSizeUserInput = Convert.ToInt32(Console.ReadLine());


            using (knowledgeObjects DB = new knowledgeObjects())
            {

                List<learningBase> tweetsCollection = new List<learningBase>();

                if (takeUserInput == 0)
                {

                    tweetsCollection = (List<learningBase>)(from tweets in DB.learningBase
                                                            orderby tweets.id
                                                            select tweets).Skip(skipUserInput).ToList();
                }
                else
                {
                    tweetsCollection = (List<learningBase>)(from tweets in DB.learningBase
                                                            orderby tweets.id
                                                            select tweets).Take(takeUserInput).Skip(skipUserInput).ToList();
                }



                Stopwatch stopwatch = new Stopwatch();
                TweetInformation ti = new TweetInformation();

                TimeSpan timespan = new TimeSpan();
                TimeSpan actualTime = new TimeSpan();
                int bulkinsertSize = bulkInsertSizeUserInput;

                TweetLoc tl = new TweetLoc(bulkinsertSize);
                int i = 0;

                foreach (var item in tweetsCollection)
                {
                    if (i % bulkinsertSize == 0)
                    {
                        tl = new TweetLoc(bulkinsertSize);
                    }
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
                    if (i % 1000 == 0)
                    {
                        string tweetTXT = i + " T " + new RoundedTimeSpan(timespan.Ticks, 2) + " avg " + new RoundedTimeSpan(timespan.Ticks / i, 2) + " avg5k " + new RoundedTimeSpan(actualTime.Ticks / 1000, 2);
                        System.Console.WriteLine(tweetTXT);
                        statusUpdate("@pide2001 " + tweetTXT, tw);
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


            }
        }

        private static void statistics(PinAuthorizer tw)
        {

            using (knowledgeObjects DB = new knowledgeObjects())
            {

                List<learningBase> tweetsCollection = new List<learningBase>();


                tweetsCollection = (List<learningBase>)(from tweets in DB.learningBase
                                                        orderby tweets.id
                                                        select tweets).ToList();




                Stopwatch stopwatch = new Stopwatch();
                TweetInformation ti = new TweetInformation();

                TimeSpan timespan = new TimeSpan();
                TimeSpan actualTime = new TimeSpan();


                TweetLoc tl = new TweetLoc(10);

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
                    tl.getGeographyStatistics(ti);
                    stopwatch.Stop();
                    actualTime += stopwatch.Elapsed;
                    timespan += stopwatch.Elapsed;
                    stopwatch.Reset();
                    if (i % 10 == 0)
                    {
                        string tweetTXT = i + " T " + new RoundedTimeSpan(timespan.Ticks, 2) + " avg " + new RoundedTimeSpan(timespan.Ticks / i, 2) + " avg5k " + new RoundedTimeSpan(actualTime.Ticks / 1000, 2);
                        System.Console.WriteLine(tweetTXT);
                        //statusUpdate("@pide2001 " + tweetTXT, tw);
                        actualTime = TimeSpan.Zero;
                    }
                }


                System.Console.WriteLine("Median " + tl.statistics.getMedianOfDistances());
                System.Console.WriteLine("Average " + tl.statistics.getAverageDistance());
                System.Console.WriteLine("Biggest " + tl.statistics.getBiggestDistance());
                System.Console.WriteLine("Smallest " + tl.statistics.getSmallestDistance());
                Tuple<GeographyData, TweetKnowledgeObj> know = tl.statistics.getBiggestDistanceAndInformation();
                System.Console.WriteLine("Biggest distance between " + know.Item1.geonamesId + " and " + know.Item2.baseDataId);



                System.Console.WriteLine("Press any key to exit");
                System.Console.ReadLine();




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


    }


}

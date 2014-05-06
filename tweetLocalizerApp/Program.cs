using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tweetLocalizerApp.Libs;
using Microsoft.SqlServer.Server;
using tweetLocalizerApp.TweetLocator;
using System.Diagnostics;

namespace tweetLocalizerApp
{

  

    class Program
    {
        static void Main(string[] args)
        {

            using (TweetsDataEntities tweetDB = new TweetsDataEntities()) {
                var tweetsCollection = (from tweets in tweetDB.tweetRandomSample2
                                        select tweets).Take(10000).ToList();

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
                    if (i % 10 == 0) {
                        System.Console.WriteLine();
                        System.Console.WriteLine("Tweets: {0} Total time {1} avg overall {2} avg last 10 {3}",i,timespan.TotalSeconds,timespan.TotalSeconds / i,actualTime.TotalSeconds / 10);
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

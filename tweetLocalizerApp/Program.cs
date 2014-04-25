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
                                        select tweets).Take(500).ToList();

               
                TweetInformation ti = new TweetInformation();
                TweetLoc tl = new TweetLoc();
                foreach (var item in tweetsCollection)
                {
                    ti.userlocation = item.userlocation;
                    ti.timezone = item.timezone;
                    ti.longitude = item.lon;
                    ti.latitude = item.lat;
                    ti.baseDataId = item.id;
                    tl.learn(ti);
                }

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

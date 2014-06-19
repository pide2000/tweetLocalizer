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

            System.Console.WriteLine("What would you do?: ");
            System.Console.WriteLine("1 : locate ");
            System.Console.WriteLine("2 : statistics ");
            System.Console.WriteLine("3 : learning ");
            System.Console.WriteLine("4 : analysis");
            int task = Convert.ToInt32(Console.ReadLine());

            if (task == 1) {
                locate(tw);
            }
            else if (task == 2) {
                statistics(tw);
            }
            else if (task == 3)
            {
                learning(tw);
            }
            else if (task == 4)
            {
                analysis();
            }
            else
            {
                System.Console.WriteLine("Bye");
                System.Console.WriteLine("Press any key to quit !");
                System.Console.ReadLine();
            }

            


            }

        private static void analysis()
        {
            using (knowledgeObjects DB = new knowledgeObjects())
            {
                DB.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
                //get tweetRandomSample to iterate over tweets and analyse the results
                var data = (from trs 
                            in DB.tweetRandomSample2 
                            select trs).ToList();

                int counter = 0;

                //iterate over tweets
                Stopwatch stopwatch = new Stopwatch();
                
                foreach (var item in data)
                {
                    stopwatch.Start();
                    List<List<knowledgeBaseGeocoding>> ListsByOrder = new List<List<knowledgeBaseGeocoding>>();

                    //getting knowledgeBaseGeocoding entries with Ngramorder
                    List<knowledgeBaseGeocoding> knowledgeBaseGeocodingList = item.knowledgeBaseGeocoding.ToList();
                    
                    counter ++;

                    //snippet to call distinct by ngram
                        //var distNGram = DB.knowledgeBaseGeocoding.Where(g => g.tweetRandomSample2.id == item.id).Select(g => g.KnowledgeBase.NGram).Distinct();

                    if (knowledgeBaseGeocodingList.Select(g => g.knowledgeBaseId).FirstOrDefault()!= null)
                    {
                        //split byNgramOrder
                        var max = knowledgeBaseGeocodingList.Max(g => g.NGramOrder);
                       
                        
                        stopwatch.Reset();  
                        //Add lists
                        for (int i = 1; i <= max; i++)
                        {
                            ListsByOrder.Add(item.knowledgeBaseGeocoding.Where(g => g.NGramOrder.Value == i).ToList());
                        }

                        //iterate over lists
                        foreach (var liste in ListsByOrder)
                        {
                            var order = liste.First().NGramOrder;
                            var tweetId = liste.First().tweetRandomSampleId;
                            int sum = (int)DB.getSumOfNGramCounts(tweetId,order).FirstOrDefault();
                            System.Console.WriteLine("twetid  {0} ngramorder {1} Summe {2}" ,tweetId,order,sum);

                            Tuple<int, float> idMaxPercentageCity = Tuple.Create(0, (float)0.0);
                            float currentPercentage = 0;
                            int ngramcoCity = 0;
                            foreach (var knowledgeBaseGeocodingEntry in liste)
                            {
                               int  ngramco = (int)knowledgeBaseGeocodingEntry.NgramCount;
                                currentPercentage = ((float)ngramco / (float)sum);
                                if (idMaxPercentageCity.Item2 < currentPercentage)
                                {
                                    idMaxPercentageCity = Tuple.Create(knowledgeBaseGeocodingEntry.id, currentPercentage);
                                    ngramcoCity = ngramco;
                                }
                            }

                            int geonamesidCity = liste.Where(g => g.id == idMaxPercentageCity.Item1).Select(g => g.KnowledgeBase.GeoNamesId).FirstOrDefault();
                            int knowledgeBaseIdCity = liste.Where(g => g.id == idMaxPercentageCity.Item1).Select(g => g.KnowledgeBase.Id).FirstOrDefault(); ;
                            double? percentageMaxCity = idMaxPercentageCity.Item2;
                            int countCity = (int)liste.Where(g => g.id == idMaxPercentageCity.Item1).Select(g => g.KnowledgeBase.NGramCount).FirstOrDefault();
                            //System.Console.WriteLine("geonId {0} knowledgeBaseid {1} percentageMax {2} count {3}" ,geonamesid,knowledgeBaseId,percentageMax,count);

                            
                            //admin2
                            var result = liste.GroupBy(o => o.KnowledgeBase.Admin2Id)
                                        .Select(g => new { admin2Id = g.Key, total = g.Sum(i => i.NgramCount) });

                            Tuple<int,float> idMaxPercentageAdmin2 = Tuple.Create(0, (float)0.0);
                            int lossAdmin2 = 0;
                            int ngramcoAdmin2 = 0;
                            foreach (var entryadmin2 in result)
                            {
                                int ngramco = (int)entryadmin2.total;
                                currentPercentage = ((float)ngramco / (float)sum);
                                System.Console.WriteLine(currentPercentage + " " + entryadmin2.admin2Id);
                                if (idMaxPercentageAdmin2.Item2 < currentPercentage)
                                {
                                    if (entryadmin2.admin2Id != null)
                                    {
                                        idMaxPercentageAdmin2 = Tuple.Create((int)entryadmin2.admin2Id, currentPercentage);
                                        ngramcoAdmin2 = ngramco;
                                    }
                                    else {
                                        lossAdmin2 = (int)entryadmin2.total;
                                       
                                    }
                                 }
                            }
                            System.Console.WriteLine(idMaxPercentageAdmin2);
                            System.Console.WriteLine("loss adm2 " + lossAdmin2);


                            //admin1
                            var resultAdmin1 = liste.GroupBy(o => o.KnowledgeBase.Admin1Id)
                                        .Select(g => new { admin1Id = g.Key, total = g.Sum(i => i.NgramCount) });

                            Tuple<int, float>  idMaxPercentageAdmin1 = Tuple.Create(0, (float)0.0);
                            int lossAdmin1 = 0;
                            int ngramcoAdmin1 = 0;
                            foreach (var entryadmin1 in resultAdmin1)
                            {
                                int ngramco = (int)entryadmin1.total;
                                currentPercentage = ((float)ngramco / (float)sum);
                                System.Console.WriteLine(currentPercentage + " " + entryadmin1.admin1Id);
                                if (idMaxPercentageAdmin1.Item2 < currentPercentage)
                                {
                                    if (entryadmin1.admin1Id != null)
                                    {
                                        idMaxPercentageAdmin1 = Tuple.Create((int)entryadmin1.admin1Id, currentPercentage);
                                        ngramcoAdmin1 = ngramco;
                                    }
                                    else
                                    {
                                        lossAdmin1 = (int)entryadmin1.total;
                                       
                                    }
                                }
                            }
                            System.Console.WriteLine(idMaxPercentageAdmin1);

                            System.Console.WriteLine("loss adm1 " + lossAdmin1);

                            //country
                            var resultCountry = liste.GroupBy(o => o.KnowledgeBase.CountryId)
                                        .Select(g => new { countryId = g.Key, total = g.Sum(i => i.NgramCount) });

                            Tuple<int,float> idMaxPercentageCountry = Tuple.Create(0, (float)0.0);
                            int ngramcoCountry = 0;
                            foreach (var entryCountry in resultCountry)
                            {
                                int ngramco = (int)entryCountry.total;
                                currentPercentage = ((float)ngramco / (float)sum);
                                System.Console.WriteLine(currentPercentage + " " + entryCountry.countryId);
                                if (idMaxPercentageCountry.Item2 < currentPercentage)
                                {

                                    idMaxPercentageCountry = Tuple.Create((int)entryCountry.countryId, currentPercentage);
                                    ngramcoCountry = ngramco;
                                }
                            }
                            System.Console.WriteLine(idMaxPercentageCountry);


                            
                    DB.resultsKnowledgeBaseGeocoding.Add(new resultsKnowledgeBaseGeocoding { 
                        nGramOrder = order,
                        overallCount = sum,
                        tweetRandomSampleId = tweetId,
                        city_geonamesId = geonamesidCity,
                        city_knowledgeBaseId = knowledgeBaseIdCity,
                        city_percentage = percentageMaxCity,
                        adm2_geonamesId = idMaxPercentageAdmin2.Item1,
                        adm2_percentage = idMaxPercentageAdmin2.Item2,
                        adm2_loss = lossAdmin2,
                        adm2_count = ngramcoAdmin2,
                        adm1_geonamesId = idMaxPercentageAdmin1.Item1,
                        adm1_percentage = idMaxPercentageAdmin1.Item2,
                        adm1_loss = lossAdmin1,
                        adm1_count = ngramcoAdmin1,
                        country_geonamesId = idMaxPercentageCountry.Item1,
                        country_percentage = idMaxPercentageCountry.Item2,
                        country_count = ngramcoCountry});


                    DB.SaveChanges();

                        }
                    }



                    
                    
                    
                    
                    //calculate percentage
                    //foreach (var item2 in maxNGramOrderList)
                    //{
                        
                    //    System.Console.WriteLine(item2.KnowledgeBase.NGram + " " + item2.KnowledgeBase.NGramCount + " " + (float)item2.KnowledgeBase.NGramCount/sum + " " + item2.NGramOrder);
                       
                    //}
                    if (counter == 40) { break; }
                      
                    
                }
                
                
            }

            System.Console.WriteLine("Press any key to quit !");
            System.Console.ReadLine();
        }
        


        private static void learning(PinAuthorizer tw)
        {
            System.Console.WriteLine("Please Type in how many LearningData should be taken (0 for all): ");
            int takeUserInput = Convert.ToInt32(Console.ReadLine());
            System.Console.WriteLine("Please Type in how many Datalines should be skipped (0 for none): ");
            int skipUserInput = Convert.ToInt32(Console.ReadLine());
            System.Console.WriteLine("Please choose the amount of Data which should be saved to the Database in one step: ");
            int bulkInsertSizeUserInput = Convert.ToInt32(Console.ReadLine());
            System.Console.WriteLine("Please type in the Information intervall: ");
            int informationIntervall = Convert.ToInt32(Console.ReadLine());
            System.Console.WriteLine("Do you want to retrieve Tweets about the progress? (0 no 1 yes)");
            int tweetInformation = Convert.ToInt32(Console.ReadLine());


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

                    if (i % informationIntervall == 0)
                    {
                        string tweetTXT = i + " T " + new RoundedTimeSpan(timespan.Ticks, 2) + " avg " + new RoundedTimeSpan(timespan.Ticks / i, 2) + " avg5k " + new RoundedTimeSpan(actualTime.Ticks / informationIntervall, 2);
                        System.Console.WriteLine(tweetTXT);
                        if (tweetInformation == 1)
                        {
                            statusUpdate("@pide2001 " + tweetTXT, tw);
                        }
                        actualTime = TimeSpan.Zero;
                    }
                }
                System.Console.WriteLine("Press any key to quit !");
                System.Console.ReadLine();





            }
        }


        private static void locate(PinAuthorizer tw)
        {
            System.Console.WriteLine("Please Type in how many DataLines should be taken (0 for all): ");
            int takeUserInput = Convert.ToInt32(Console.ReadLine());
            System.Console.WriteLine("Please Type in how many Datalines should be skipped (0 for none): ");
            int skipUserInput = Convert.ToInt32(Console.ReadLine());
            System.Console.WriteLine("Please type in the Information intervall: ");
            int informationIntervall = Convert.ToInt32(Console.ReadLine());
            System.Console.WriteLine("Do you want to retrieve Tweets about the progress? (0 no 1 yes)");
            int tweetInformation = Convert.ToInt32(Console.ReadLine());
            

            using (knowledgeObjects DB = new knowledgeObjects())
            {

                List<tweetRandomSample2> tweetsCollection = new List<tweetRandomSample2>();

                if (takeUserInput == 0)
                {

                    tweetsCollection = (List<tweetRandomSample2>)(from tweets in DB.tweetRandomSample2
                                                            orderby tweets.id
                                                            select tweets).Skip(skipUserInput).ToList();
                }
                else
                {
                    tweetsCollection = (List<tweetRandomSample2>)(from tweets in DB.tweetRandomSample2
                                                            orderby tweets.id
                                                            select tweets).Take(takeUserInput).Skip(skipUserInput).ToList();
                }



                Stopwatch stopwatch = new Stopwatch();
                TweetInformation ti = new TweetInformation();

                TimeSpan timespan = new TimeSpan();
                TimeSpan actualTime = new TimeSpan();


                TweetLoc tl = new TweetLoc(0);
                
                int i = 0;
                GeoNames knowledgeResult = new GeoNames();
                GeoNames tweetCountry = new GeoNames();
                foreach (var item in tweetsCollection)
                {
                    using (GeonamesDataEntities geonamesDB = new GeonamesDataEntities()) {
                        tweetCountry = (from geonames in geonamesDB.GeoNames 
                                        where geonames.geonameid == item.geoNames_geoNamesId 
                                        select geonames).ToList().First();
                    }


                    i++;
                    stopwatch.Start();
                    ti = new TweetInformation();
                    ti.userlocation = item.userlocation;
                    ti.timezone = item.timezone;
                    ti.longitude = item.lon;
                    ti.latitude = item.lat;
                    ti.baseDataId = item.id;
                    ti.coord = item.coord;
                    ti.randomSampleId = item.id;
                    tl.saveLocateResults(ti);
                    stopwatch.Stop();
                    actualTime += stopwatch.Elapsed;
                    timespan += stopwatch.Elapsed;
                    stopwatch.Reset();
                    if (i % informationIntervall == 0)
                    {
                        string tweetTXT = i + " T " + new RoundedTimeSpan(timespan.Ticks, 2) + " avg " + new RoundedTimeSpan(timespan.Ticks / i, 2) + " avg5k " + new RoundedTimeSpan(actualTime.Ticks / informationIntervall, 2);
                        System.Console.WriteLine(tweetTXT);
                        if (tweetInformation == 1)
                        { 
                            statusUpdate("@pide2001 " + tweetTXT, tw);
                            }
                        actualTime = TimeSpan.Zero;
                    }
                }

                System.Console.WriteLine("Press any key to quit !");
                System.Console.ReadLine();





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




                System.Console.WriteLine("Press any key to quit !");
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




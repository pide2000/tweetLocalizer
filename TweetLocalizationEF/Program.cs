using System;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using Lucuma;
using Lucuma.Libs;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Text.RegularExpressions;
using LinqToTwitter;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Data.Entity.Spatial;


namespace TweetLocalizationEF
{
     class Location {
        public String locationString;
        public List<String> seperatedLocations { get; set; }
        public List<List<GeoNameLocal>> locationResults = new List<List<GeoNameLocal>>();



        public Location(string placestring) {
            locationString = placestring;
            seperatedLocations = locationPreprocessing(locationString).ToList();
        
        }

        private static String[] locationPreprocessing(String text)
        {
            
            String words = Regex.Replace(text, @"[^a-zA-Z0-9 ./;.,_-]", "");
            string[] split = Array.ConvertAll(words.Split(new Char[] { ',' }), p => p.Trim());
            return split;
        }



        

        

    }

     public class TupleList<T1, T2> : List<Tuple<T1, T2>> {
         public void Add(T1 item1, T2 item2) {
             Add(new Tuple<T1, T2>(item1, item2));
         
         }
     
     
     }


    public class tokenized{
        public int geoNamesId { get; set; }
        public String[] tokenArray { get; set; }
    
    }

    public class geoNamesId_To_geoTreeNodeId {
        public List<geonamesid_geotreenodeidstring> list { get; set; }
        public bool testIsInList(geonamesid_geotreenodeidstring obj) {
            return list.Contains(obj);
        }
    
    }

    public class coordinates {
        public int id { get; set; }
        public DbGeography point1 { get; set; }
        public DbGeography point2 { get; set; }
    
    }
    class Program
    {

        /**
         * This is a implementation of the Levensthein Distance from https://gist.github.com/wickedshimmy/449595
         **/

        public static int EditDistance(string original, string modified)
        {
            int len_orig = original.Length;
            int len_diff = modified.Length;

            var matrix = new int[len_orig + 1, len_diff + 1];
            for (int i = 0; i <= len_orig; i++)
                matrix[i, 0] = i;
            for (int j = 0; j <= len_diff; j++)
                matrix[0, j] = j;

            for (int i = 1; i <= len_orig; i++)
            {
                for (int j = 1; j <= len_diff; j++)
                {
                    int cost = modified[j - 1] == original[i - 1] ? 0 : 1;
                    var vals = new int[] {
				matrix[i - 1, j] + 1, //deletion
				matrix[i, j - 1] + 1, //insertion
				matrix[i - 1, j - 1] + cost //substitution
			};
                    matrix[i, j] = vals.Min();
                    if (i > 1 && j > 1 && original[i - 1] == modified[j - 2] && original[i - 2] == modified[j - 1])
                        matrix[i, j] = Math.Min(matrix[i, j], matrix[i - 2, j - 2] + cost);
                }
            }
            return matrix[len_orig, len_diff];
        }

        public static String[] locationPreprocessing(String text)
        {

            String words = System.Text.RegularExpressions.Regex.Replace(text, @"[^\p{L}\p{N} ]", " ");
            words = System.Text.RegularExpressions.Regex.Replace(words, @"\s+", " ");
            words = words.Trim();
            string[] split = Array.ConvertAll(words.Split(new Char[] { ' ' }), p => p.Trim());
            return split;
        }


        /* Unicode Categories 
         * regex to use categories @"[\p{X}]" http://msdn.microsoft.com/en-us/library/20bw873z(v=vs.110).aspx
         * with X= L for all letters, N for all numbers, P for punctuation characters, S for Symbols http://msdn.microsoft.com/en-us/library/20bw873z(v=vs.110).aspx#SupportedUnicodeGeneralCategories 
         */
        private static char[] getSigns(String text)
        {

            String signs = System.Text.RegularExpressions.Regex.Replace(text, @"[\p{P}\p{N}\p{S} ]", "");
            char [] signList = signs.ToCharArray();
            return signList;
        }

        public static Tuple<float,float> precisionRecallSingle(TupleList<String,String> outcomelist)
        {
            
            float truePositive = 0;
            float falsePositive = 0;
            float falseNegative = 0;
            float precision = 0; // Precision = tp/tp+fp
            float recall = 0;// Recall = tp/tp+fn
           
            foreach(Tuple<String,String> ele in outcomelist){
                if (ele.Item1 == null) {
                   
                }
                else if (ele.Item2 == null || ele.Item2.Equals("NIL")) {
                    falseNegative++;
                }
                else if (ele.Item1 == ele.Item2)
                {
                    truePositive++;
                }
                else if (ele.Item1 != ele.Item2)
                {
                    falsePositive++;
                }

            }
            
            precision = truePositive / (truePositive + falsePositive);
            recall = truePositive / (truePositive + falseNegative);
            
            Tuple<float, float> pc = new Tuple<float,float>(precision,recall);
            return pc;
        
        }

        public static List<float> precisionRecall() {
            geoNamesDatabaseConnection conn = new geoNamesDatabaseConnection();
            List<countryResolutionOutcome> resolutionOutcomeList = new List<countryResolutionOutcome>(conn.executeQuery<countryResolutionOutcome>("SELECT * FROM geoNamesData.dbo.countryResolutionOutcome"));
            List<float> pc = new List<float>();
            float truePositive2 = 0; 
            float falsePositive2 = 0; 
            float falseNegative2 = 0;
            float precision2 = 0; // Precision = tp/tp+fp
            float recall2 = 0;// Recall = tp/tp+fn
            float truePositive3 = 0;
            float falsePositive3 = 0;
            float falseNegative3 = 0;
            float precision3 = 0; // Precision = tp/tp+fp
            float recall3 = 0;// Recall = tp/tp+fnfloat truePositive2 = 0; 
            float truePositive4 = 0;
            float falsePositive4 = 0;
            float falseNegative4 = 0;
            float precision4 = 0; // Precision = tp/tp+fp
            float recall4 = 0;// Recall = tp/tp+fnfloat truePositive2 = 0; 
            float truePositive5 = 0;
            float falsePositive5 = 0;
            float falseNegative5 = 0;
            float precision5 = 0; // Precision = tp/tp+fp
            float recall5 = 0;// Recall = tp/tp+fnfloat truePositive2 = 0; 
            float truePositive6 = 0;
            float falsePositive6 = 0;
            float falseNegative6 = 0;
            float precision6 = 0; // Precision = tp/tp+fp
            float recall6 = 0;// Recall = tp/tp+fnfloat truePositive2 = 0; 
            float truePositive7 = 0;
            float falsePositive7 = 0;
            float falseNegative7 = 0;
            float precision7 = 0; // Precision = tp/tp+fp
            float recall7 = 0;// Recall = tp/tp+fn

            foreach(countryResolutionOutcome ele in resolutionOutcomeList){
                if (ele.country1 == null) {
                   
                }
                else if (ele.country2 == null) {
                    falseNegative2++;
                }
                else if (ele.country1 == ele.country2)
                {
                    truePositive2++;
                }
                else if (ele.country1 != ele.country2)
                {
                    falsePositive2++;
                }

            }
            

            precision2 = truePositive2 / (truePositive2 + falsePositive2);
            
            recall2 = truePositive2 / (truePositive2 + falseNegative2);
            System.Console.WriteLine(" tp2:: " + truePositive2 + " fp2: " + falsePositive2 + " fn2: " + falseNegative2);
            System.Console.WriteLine("p2: " + precision2);
            System.Console.WriteLine("r2: " + recall2);
            pc.Add(precision2);
            pc.Add(recall2);

            foreach (countryResolutionOutcome ele in resolutionOutcomeList)
            {
                if (ele.country1 == null)
                {
                }
                else if (ele.country3 == null)
                {
                    falseNegative3++;
                }
                else if (ele.country1 == ele.country3)
                {
                    truePositive3++;
                }
                else if (ele.country1 != ele.country3)
                {
                    falsePositive3++;
                }

            }

            precision3 = truePositive3 / (truePositive3 + falsePositive3);
            recall3 = truePositive3 / (truePositive3 + falseNegative3);
            System.Console.WriteLine(" tp3:: " + truePositive3 + " fp3: " + falsePositive3 + " fn3: " + falseNegative3);
            System.Console.WriteLine("p3: " + precision3);
            System.Console.WriteLine("r3: " + recall3);
            
            pc.Add(precision3);
            pc.Add(recall3);


            foreach (countryResolutionOutcome ele in resolutionOutcomeList)
            {
                if (ele.country1 == null)
                {
                }
                else if (ele.country4 == null)
                {
                    falseNegative4++;
                }
                else if (ele.country1 == ele.country4)
                {
                    truePositive4++;
                }
                else if (ele.country1 != ele.country4)
                {
                    falsePositive4++;
                }

                
            }
            precision4 = truePositive4 / (truePositive4 + falsePositive4);
            recall4 = truePositive4 / (truePositive4 + falseNegative4);
            System.Console.WriteLine(" tp4:: " + truePositive4 + " fp4: " + falsePositive4 + " fn4: " + falseNegative4);
            System.Console.WriteLine("p4: " + precision4);
            System.Console.WriteLine("r4: " + recall4);
            pc.Add(precision4);
            pc.Add(recall4);

            foreach (countryResolutionOutcome ele in resolutionOutcomeList)
            {
                if (ele.country1 == null)
                {
                }
                else if (ele.country5 == null)
                {
                    falseNegative5++;
                }
                else if (ele.country1 == ele.country5)
                {
                    truePositive5++;
                }
                else if (ele.country1 != ele.country5)
                {
                    falsePositive5++;
                }

               
            }
            precision5 = truePositive5 / (truePositive5 + falsePositive5);
            recall5 = truePositive5 / (truePositive5 + falseNegative5);
            System.Console.WriteLine(" tp5:: " + truePositive5 + " fp5: " + falsePositive5 + " fn5: " + falseNegative5);
            System.Console.WriteLine("p5: " + precision5);
            System.Console.WriteLine("r5: " + recall5);
            pc.Add(precision5);
            pc.Add(recall5);

            foreach (countryResolutionOutcome ele in resolutionOutcomeList)
            {
                if (ele.country1 == null)
                {
                }
                else if (ele.country6 == null)
                {
                    falseNegative6++;
                }
                else if (ele.country1 == ele.country6)
                {
                    truePositive6++;
                }
                else if (ele.country1 != ele.country6)
                {
                    falsePositive6++;
                }

                
            }
            precision6 = truePositive6 / (truePositive6 + falsePositive6);
            recall6 = truePositive6 / (truePositive6 + falseNegative6);
            System.Console.WriteLine(" tp6:: " + truePositive6 + " fp6: " + falsePositive6 + " fn6: " + falseNegative6);
            System.Console.WriteLine("p6: " + precision6);
            System.Console.WriteLine("r6: " + recall6);
            pc.Add(precision6);
            pc.Add(recall6);

            foreach (countryResolutionOutcome ele in resolutionOutcomeList)
            {
                if (ele.country1 == null)
                {
                }
                else if (ele.country7 == null)
                {
                    falseNegative7++;
                }
                else if (ele.country1 == ele.country7)
                {
                    truePositive7++;
                }
                else if (ele.country1 != ele.country7)
                {
                    falsePositive7++;
                }

            }

            precision7 = truePositive7 / (truePositive7 + falsePositive7);
            recall7 = truePositive7 / (truePositive7 + falseNegative7);
            System.Console.WriteLine(" tp7:: " + truePositive7 + " fp7: " + falsePositive7 + " fn7: " + falseNegative7);
            System.Console.WriteLine("p7: " + precision7);
            System.Console.WriteLine("r7: " + recall7);
            pc.Add(precision7);
            pc.Add(recall7);

            
            return pc;
        
        }

        public static void someStats(geoNamesDatabaseConnection conn)
        {

            //foreach (GeoNameLocal obj in localizator.nearestNeighbourSearch_GeoNames((float)-97.0828170776367, (float)33.1733779907227, 10))
            //{
            //    System.Console.WriteLine(obj.country_code);
            //}
            List<Tweet> tweetsDot = conn.getTweetsWhereCharacterIsLike("%.%");
            List<Tweet> tweetsNo = conn.getTweetsWhereCharacterIsLike("%No.%");
            List<Tweet> tweetsSt = conn.getTweetsWhereCharacterIsLike("%St.%");
            System.Console.WriteLine("Tweets with . in Location:" + tweetsDot.Count());
            System.Console.WriteLine("Tweets with No. in Location:" + tweetsNo.Count());
            System.Console.WriteLine("Tweets with St. in Location:" + tweetsSt.Count());
            System.Console.WriteLine("Tweets with . except St. and No." + (tweetsDot.Count() - tweetsNo.Count() - tweetsSt.Count()));
            List<Tweet> sortedOut = new List<Tweet>(tweetsDot);
            foreach (Tweet t in tweetsDot)
            {
                if (t.place.Contains("No.") || t.place.Contains("St."))
                {
                    sortedOut.Remove(t);
                }
            }
            System.Console.WriteLine("without st and no " + sortedOut.Count());
        }

        /**
         * if uniqueMatch is true only unique perfect matches will be returned,
         * if false the function returns perfect matches even if a whole List of perfect matches is returned. In this case
         * only the first perfect match of the search will be returned.
         * **/
        public static List<String> getTweetLocationNaiveCases(Tweet tweet, bool uniqueMatch, ref geoNamesDatabaseConnection conn, String featureClassRestriction)
        {
            
            List<String> temp;
            List<String> countryList = new List<String>();
            List<String> uniqueList = new List<String>();
            var currentChars = locationPreprocessing(tweet.place);
           
            foreach (String str in currentChars)
            {
                //int time1 = Environment.TickCount;
                temp = new List<string>(conn.like_search_GeoNamesRestricted_name(str, featureClassRestriction));
                //int time2 = Environment.TickCount;
                //System.Console.WriteLine("like_search for " + str + " takes " + (time2-time1) + " milliseconds");
                if (uniqueMatch && temp.Count() == 1)
                {
                    uniqueList.Add(temp[0]);
                }
                else if (!uniqueMatch && temp.Count()==1) {
                    countryList.Add(temp[0]);
                }
                else if (!uniqueMatch && temp.Count() > 1)
                {
                    countryList.Add(temp[0]);
                }
                
              }

            

            if (uniqueMatch)
            {
                if (uniqueList.Count() == 0)
                {
                    uniqueList.Add(null);
                }
                return uniqueList;
            }
            else if (!uniqueMatch)
            {
                if (countryList.Count() == 0) {
                    countryList.Add(null);
                }
                return countryList;
            }
            else { return null; }
           
        }

        public static List<String> 
            getTweetLocationNaiveCase5(Tweet tweet, bool uniqueMatch, ref geoNamesDatabaseConnection conn)
        {

            List<String> temp;
            List<String> countryList = new List<String>();
            List<String> uniqueList = new List<String>();
            var currentChars = locationPreprocessing(tweet.place);

            foreach (String str in currentChars)
            {
                //int time1 = Environment.TickCount;
                temp = new List<string>(conn.like_search_GeoNamesRestricted_name_allFeature(str));
                //int time2 = Environment.TickCount;
                //System.Console.WriteLine("like_search for " + str + " takes " + (time2-time1) + " milliseconds");
                if (uniqueMatch && temp.Count() == 1)
                {
                    uniqueList.Add(temp[0]);
                }
                else if (!uniqueMatch && temp.Count() == 1)
                {
                    countryList.Add(temp[0]);
                }
                else if (!uniqueMatch && temp.Count() > 1)
                {
                    countryList.Add(temp[0]);
                }

            }



            if (uniqueMatch)
            {
                if (uniqueList.Count() == 0)
                {
                    uniqueList.Add(null);
                }
                return uniqueList;
            }
            else if (!uniqueMatch)
            {
                if (countryList.Count() == 0)
                {
                    countryList.Add(null);
                }
                return countryList;
            }
            else { return null; }

        }

        public static List<List<String>> getTweetLocation(Tweet tweet,ref geoNamesDatabaseConnection conn, String featureClassRestriction)
        {
            List<List<String>> locationList = new List<List<String>>();
           
            var currentChars = locationPreprocessing(tweet.place);
            int i=0;
            foreach (String str in currentChars)
            {
                //int time1 = Environment.TickCount;
                locationList.Add(conn.like_search_GeoNamesRestricted_name(str, featureClassRestriction));
                locationList[i].Insert(0, str);
                    i++;
                //int time2 = Environment.TickCount;
                //System.Console.WriteLine("like_search for " + str + " takes " + (time2-time1) + " milliseconds");
            }

            return locationList;
        }

        /**
            *  naive case1:
            *  - like search(sql) 
            *  - only search in feature_class = 'P' => cities villages.... (www.geonames.org/export/codes.html)
            *  - just unique perfect matches will be taken into consideration. If there are more than one perfect match per search -> discard it
            *    e.g. like search for abstatt results in {{abstatt,DE}}, will be taken. But like search for heilbronn results in {{heilbronn,DE},{heilbronn,DE},{heilbronn,US}} will be discarded 
            *  - if there is more than one perfect match for all given terms (e.g. in case of Abstatt, Pennsylvania. which is non-sense) 
            *    the country only will be taken when the countries given by the like search match 
            *    so Abstatt->DE, Pennsylvania->US will be discarded
            *    saved in countryResolutionoutcome field: country2
            **/
        private static void naiveCase1()
        {
            geoNamesDatabaseConnection conn = new geoNamesDatabaseConnection();
            List<countryResolutionOutcome> countryResolutionOutcomeList = new List<countryResolutionOutcome>();
            List<Tweet> tweetList = new List<Tweet>(conn.getAllTweetsRandomSample());
            int timeStampOne = Environment.TickCount;
            int timeStampTwo;
            int timeStampThree = timeStampOne;
            int processedTweets = 0;
            foreach (Tweet tweet in tweetList)
            {
                List<String> countryList = getTweetLocationNaiveCases(tweet, true, ref conn, "P");
                if (countryList.All(o => o == countryList.First()))
                {
                    countryResolutionOutcomeList.Add(new countryResolutionOutcome { tweetRandomSample_Id = tweet.id, country2 = countryList.First() });
                }
                else
                {
                    countryResolutionOutcomeList.Add(new countryResolutionOutcome { tweetRandomSample_Id = tweet.id, country2 = null });
                }
                processedTweets++;
                if(processedTweets%1000==0){
                    timeStampTwo = timeStampThree;
                    timeStampThree = Environment.TickCount; 
                    string tempTime = TimeSpan.FromMilliseconds(timeStampThree - timeStampTwo).ToString(@"dd\:hh\:mm\:ss\:ff") ;
                    string passedTime = TimeSpan.FromMilliseconds(timeStampThree - timeStampOne).ToString(@"dd\:hh\:mm\:ss\:ff");
                    System.Console.WriteLine("Processed " + processedTweets + " in " + tempTime + " Passed Time so far" + passedTime);
                }
            }
            conn.addListToCountryResolutionOutcome(countryResolutionOutcomeList);
        }

        /**
             * naive case2:
             *  - like search(sql) 
             *  - only search in feature_class = 'P' => cities villages.... (www.geonames.org/export/codes.html)
             *  - if there are more unique matches, the first one of the given list will be taken. e.g.{{heilbronn,DE},{heilbronn,DE},{heilbronn,US}} => DE will be taken
             *  - if there is more than one match in all given terms (e.g. in case of Abstatt, Pennsylvania. which is non-sense) 
             *    the country only will be taken when the countries given by the like search match 
             *    so Abstatt->DE, Pennsylvania->US will be discarded
             *    saved in countryResolutionoutcome field: country3
            **/
        public static void naiveCase2(){
            geoNamesDatabaseConnection conn = new geoNamesDatabaseConnection();
            List<countryResolutionOutcome> countryResolutionOutcomeList = new List<countryResolutionOutcome>();
            List<Tweet> tweetList = new List<Tweet>(conn.getAllTweetsRandomSample());
            int timeStampOne = Environment.TickCount;
            int timeStampTwo;
            int timeStampThree = timeStampOne;
            int processedTweets = 0;
            foreach (Tweet tweet in tweetList)
            {
                List<String> countryList = getTweetLocationNaiveCases(tweet, false, ref conn, "P");
                if (countryList.All(o => o == countryList.First()))
                {
                    countryResolutionOutcomeList.Add(new countryResolutionOutcome { tweetRandomSample_Id = tweet.id, country3 = countryList.First() });
                }
                else
                {
                    countryResolutionOutcomeList.Add(new countryResolutionOutcome { tweetRandomSample_Id = tweet.id, country3 = null });
                }
                processedTweets++;
                if (processedTweets % 1000 == 0)
                {
                    timeStampTwo = timeStampThree;
                    timeStampThree = Environment.TickCount;
                    string tempTime = TimeSpan.FromMilliseconds(timeStampThree - timeStampTwo).ToString(@"dd\:hh\:mm\:ss\:ff");
                    string passedTime = TimeSpan.FromMilliseconds(timeStampThree - timeStampOne).ToString(@"dd\:hh\:mm\:ss\:ff");
                    System.Console.WriteLine("Processed " + processedTweets + " in " + tempTime + " Passed Time so far" + passedTime);
                }
            }
            conn.addListToCountryResolutionOutcome(countryResolutionOutcomeList);
        
        }

        /**
               *  naive case3:
               *  - like search(sql) 
               *  - only search in feature_class = 'A' => country, state, region .... (www.geonames.org/export/codes.html)
               *  - just unique perfect matches will be taken into consideration. If there are more than one perfect match per search -> discard it
               *    e.g. like search for abstatt results in {{abstatt,DE}}, will be taken. But like search for heilbronn results in {{heilbronn,DE},{heilbronn,DE},{heilbronn,US}} will be discarded 
               *  - if there is more than one perfect match for all given terms (e.g. in case of Abstatt, Pennsylvania. which is non-sense) 
               *    the country only will be taken when the countries given by the like search match 
               *    so Abstatt->DE, Pennsylvania->US will be discarded
               *    saved in countryResolutionoutcome field: country4
               **/
        public static void naiveCase3()
        {
            geoNamesDatabaseConnection conn = new geoNamesDatabaseConnection();
            List<countryResolutionOutcome> countryResolutionOutcomeList = new List<countryResolutionOutcome>();
            List<Tweet> tweetList = new List<Tweet>(conn.getAllTweetsRandomSample());
            int timeStampOne = Environment.TickCount;
            int timeStampTwo;
            int timeStampThree = timeStampOne;
            int processedTweets = 0;
            foreach (Tweet tweet in tweetList)
            {
                List<String> countryList = getTweetLocationNaiveCases(tweet, true, ref conn, "A");
                if (countryList.All(o => o == countryList.First()))
                {
                    countryResolutionOutcomeList.Add(new countryResolutionOutcome { tweetRandomSample_Id = tweet.id, country4 = countryList.First() });
                }
                else
                {
                    countryResolutionOutcomeList.Add(new countryResolutionOutcome { tweetRandomSample_Id = tweet.id, country4 = null });
                }
                processedTweets++;
                if (processedTweets % 1000 == 0)
                {
                    timeStampTwo = timeStampThree;
                    timeStampThree = Environment.TickCount;
                    string tempTime = TimeSpan.FromMilliseconds(timeStampThree - timeStampTwo).ToString(@"dd\:hh\:mm\:ss\:ff");
                    string passedTime = TimeSpan.FromMilliseconds(timeStampThree - timeStampOne).ToString(@"dd\:hh\:mm\:ss\:ff");
                    System.Console.WriteLine("Processed " + processedTweets + " in " + tempTime + " Passed Time so far" + passedTime);
                }
            }
            conn.addListToCountryResolutionOutcome(countryResolutionOutcomeList);

        }

        /**
            * naive case4:
            *  - like search(sql) 
            *  - only search in feature_class = 'A' => country, state, region.... (www.geonames.org/export/codes.html)
            *  - if there are more unique matches, the first one of the given list will be taken. e.g.{{heilbronn,DE},{heilbronn,DE},{heilbronn,US}} => DE will be taken
            *  - if there is more than one match in all given terms (e.g. in case of Abstatt, Pennsylvania. which is non-sense) 
            *    the country only will be taken when the countries given by the like search match 
            *    so Abstatt->DE, Pennsylvania->US will be discarded
            *    saved in countryResolutionoutcome field: country5
           **/
        public static void naiveCase4()
        {
            geoNamesDatabaseConnection conn = new geoNamesDatabaseConnection();
            List<countryResolutionOutcome> countryResolutionOutcomeList = new List<countryResolutionOutcome>();
            List<Tweet> tweetList = new List<Tweet>(conn.getAllTweetsRandomSample());
            int timeStampOne = Environment.TickCount;
            int timeStampTwo;
            int timeStampThree = timeStampOne;
            int processedTweets = 0;
            foreach (Tweet tweet in tweetList)
            {
                List<String> countryList = getTweetLocationNaiveCases(tweet, false, ref conn, "A");
                if (countryList.All(o => o == countryList.First()))
                {
                    countryResolutionOutcomeList.Add(new countryResolutionOutcome { tweetRandomSample_Id = tweet.id, country5 = countryList.First() });
                }
                else
                {
                    countryResolutionOutcomeList.Add(new countryResolutionOutcome { tweetRandomSample_Id = tweet.id, country5 = null });
                }
                processedTweets++;
                if (processedTweets % 1000 == 0)
                {
                    timeStampTwo = timeStampThree;
                    timeStampThree = Environment.TickCount;
                    string tempTime = TimeSpan.FromMilliseconds(timeStampThree - timeStampTwo).ToString(@"dd\:hh\:mm\:ss\:ff");
                    string passedTime = TimeSpan.FromMilliseconds(timeStampThree - timeStampOne).ToString(@"dd\:hh\:mm\:ss\:ff");
                    System.Console.WriteLine("Processed " + processedTweets + " in " + tempTime + " Passed Time so far" + passedTime);
                }
            }
            conn.addListToCountryResolutionOutcome(countryResolutionOutcomeList);

        }

        /**
              *  naive case5:
              *  - like search(sql) 
              *  - only search in feature_class = 'A' AND 'B'
              *  - just unique perfect matches will be taken into consideration. If there are more than one perfect match per search -> discard it
              *    e.g. like search for abstatt results in {{abstatt,DE}}, will be taken. But like search for heilbronn results in {{heilbronn,DE},{heilbronn,DE},{heilbronn,US}} will be discarded 
              *  - if there is more than one perfect match for all given terms (e.g. in case of Abstatt, Pennsylvania. which is non-sense) 
              *    the country only will be taken when the countries given by the like search match 
              *    so Abstatt->DE, Pennsylvania->US will be discarded
              *    saved in countryResolutionoutcome field: country6
              **/
        private static void naiveCase5()
        {
            geoNamesDatabaseConnection conn = new geoNamesDatabaseConnection();
            List<countryResolutionOutcome> countryResolutionOutcomeList = new List<countryResolutionOutcome>();
            List<Tweet> tweetList = new List<Tweet>(conn.getAllTweetsRandomSample());
            int timeStampOne = Environment.TickCount;
            int timeStampTwo;
            int timeStampThree = timeStampOne;
            int processedTweets = 0;
            foreach (Tweet tweet in tweetList)
            {
                List<String> countryList = getTweetLocationNaiveCase5(tweet, true, ref conn);
                if (countryList.All(o => o == countryList.First()))
                {
                    countryResolutionOutcomeList.Add(new countryResolutionOutcome { tweetRandomSample_Id = tweet.id, country6 = countryList.First() });
                }
                else
                {
                    countryResolutionOutcomeList.Add(new countryResolutionOutcome { tweetRandomSample_Id = tweet.id, country6 = null });
                }
                processedTweets++;
                if (processedTweets % 1000 == 0)
                {
                    timeStampTwo = timeStampThree;
                    timeStampThree = Environment.TickCount;
                    string tempTime = TimeSpan.FromMilliseconds(timeStampThree - timeStampTwo).ToString(@"dd\:hh\:mm\:ss\:ff");
                    string passedTime = TimeSpan.FromMilliseconds(timeStampThree - timeStampOne).ToString(@"dd\:hh\:mm\:ss\:ff");
                    System.Console.WriteLine("Processed " + processedTweets + " in " + tempTime + " Passed Time so far" + passedTime);
                }
            }
            conn.addListToCountryResolutionOutcome(countryResolutionOutcomeList);
        }

        /**
           * naive case6:
           *  - like search(sql) 
           *  - only search in feature_class = 'A' AND 'B'
           *  - if there are more unique matches, the first one of the given list will be taken. e.g.{{heilbronn,DE},{heilbronn,DE},{heilbronn,US}} => DE will be taken
           *  - if there is more than one match in all given terms (e.g. in case of Abstatt, Pennsylvania. which is non-sense) 
           *    the country only will be taken when the countries given by the like search match 
           *    so Abstatt->DE, Pennsylvania->US will be discarded
           *    saved in countryResolutionoutcome field: country7
          **/
        private static void naiveCase6()
        {
            geoNamesDatabaseConnection conn = new geoNamesDatabaseConnection();
            List<countryResolutionOutcome> countryResolutionOutcomeList = new List<countryResolutionOutcome>();
            List<Tweet> tweetList = new List<Tweet>(conn.getAllTweetsRandomSample());
            int timeStampOne = Environment.TickCount;
            int timeStampTwo;
            int timeStampThree = timeStampOne;
            int processedTweets = 0;
            foreach (Tweet tweet in tweetList)
            {
                List<String> countryList = getTweetLocationNaiveCase5(tweet, false, ref conn);
                if (countryList.All(o => o == countryList.First()))
                {
                    countryResolutionOutcomeList.Add(new countryResolutionOutcome { tweetRandomSample_Id = tweet.id, country7 = countryList.First() });
                }
                else
                {
                    countryResolutionOutcomeList.Add(new countryResolutionOutcome { tweetRandomSample_Id = tweet.id, country7 = null });
                }
                processedTweets++;
                if (processedTweets % 1000 == 0)
                {
                    timeStampTwo = timeStampThree;
                    timeStampThree = Environment.TickCount;
                    string tempTime = TimeSpan.FromMilliseconds(timeStampThree - timeStampTwo).ToString(@"dd\:hh\:mm\:ss\:ff");
                    string passedTime = TimeSpan.FromMilliseconds(timeStampThree - timeStampOne).ToString(@"dd\:hh\:mm\:ss\:ff");
                    System.Console.WriteLine("Processed " + processedTweets + " in " + tempTime + " Passed Time so far" + passedTime);
                }
            }
            conn.addListToCountryResolutionOutcome(countryResolutionOutcomeList);
        }

        private static void gmapsGeocoding(geoNamesDatabaseConnection conn, List<uniqueLocationFromTweetSample> uniqueLoc, int locationCount = 0)
        {
            if (locationCount == 0)
            {
                locationCount = uniqueLoc.Count;
            }
            GeoCoder gc = new GeoCoder();
           
            try
            {
                for (int i = 0; i < locationCount; i++)
                {
                    
                    IGeoCodeResult result = new GeoCodeResult();
                    System.Console.WriteLine(i + "  " + uniqueLoc[i].locationString);
                    result = gc.GetCoordinates(uniqueLoc[i].locationString);
                    if (result.Error.Equals("OVER_QUERY_LIMIT"))
                    {
                        System.Console.WriteLine("Error: " + result.Error + "Program going to halt");
                        System.Console.WriteLine("Request: " + result.request);
                        JObject response = (JObject)JsonConvert.DeserializeObject(result.response);
                        System.Console.WriteLine("JSON Response: " + response.ToString());
                        int whileCounter = 0;
                        int waitingTime = 200;
                        while (result.Error.Equals("OVER_QUERY_LIMIT") && whileCounter <= 5)
                        {
                            whileCounter++;
                            waitingTime = waitingTime * 2;
                            System.Console.WriteLine("Waiting for one second. " + whileCounter + "seconds waited so far");
                            System.Threading.Thread.Sleep(waitingTime);
                            result = gc.GetCoordinates(uniqueLoc[i].locationString);
                        }
                        
                    }
                    else if (result.Error.Equals("REQUEST_DENIED"))
                    {
                        System.Console.WriteLine("Error: " + result);
                        System.Console.WriteLine("Request: " + result.request);
                        JObject response = (JObject)JsonConvert.DeserializeObject(result.response);
                        System.Console.WriteLine("JSON Response: " + response.ToString());
                        int whileCounter = 0;
                        while (result.Error.Equals("REQUEST_DENIED"))
                        {
                            whileCounter++;
                            System.Console.WriteLine("Waiting for one second. " + whileCounter + "seconds waited so far");
                            System.Threading.Thread.Sleep(1000);
                            result = gc.GetCoordinates(uniqueLoc[i].locationString);
                        }
                        
                        uniqueLoc[i].gmaps = result.Locations.First().Country;
                    
                    }
                    else if (result.Error.Equals("ZERO_RESULTS")) {
                        System.Console.WriteLine("No results");
                        uniqueLoc[i].gmaps = "ZER";
                    
                    }
                    else if (result.Error.Equals("INVALID_REQUEST"))
                    {
                        System.Console.WriteLine("Error: " + result + "Program going to halt");
                        System.Console.WriteLine("Request: " + result.request);
                        JObject response = (JObject)JsonConvert.DeserializeObject(result.response);
                        System.Console.WriteLine("JSON Response: " + response.ToString());
                        uniqueLoc[i].gmaps = result.request.ToString();
                        
                    }
                    else
                    {
                        if (result.HasValue)
                        {
                            uniqueLoc[i].gmaps = result.Locations.First().Country;
                        }
                        else { 
                            uniqueLoc[i].gmaps = "NIL";
                        }
                    }
                    conn.updateUniqueLocationFromTweetSample(uniqueLoc[i]);

                }
            }

            catch (Exception e)
            {
                System.Console.WriteLine("{0} Exception thrown", e);
            }
           
        }

        private static List<gmapsGeocoding> gmapsGeocodingJSONResult(geoNamesDatabaseConnection conn, List<tweetRandomSample2> tweetrandomsample2, int skip)
        {
            List<gmapsGeocoding> gmapsResultList = new List<gmapsGeocoding>();
            GeoCoder gc = new GeoCoder();
            int i = 0;
            try
            {
               
               foreach(tweetRandomSample2 tweet in tweetrandomsample2.Skip(skip))
                {
                   
                   IGeoCodeResult result = new GeoCodeResult();
                   System.Console.WriteLine(i + "  " + tweet.userlocation);
                   result = gc.GetCoordinates(tweet.userlocation);
                   
                    
                   if (result.Error.Equals("OVER_QUERY_LIMIT"))
                    {
                        System.Console.WriteLine("Error: " + result.Error + "Program going to halt");
                        System.Console.WriteLine("Request: " + result.request);
                       
                        int whileCounter = 0;
                        int waitingTime = 200;
                        while (result.Error.Equals("OVER_QUERY_LIMIT") && whileCounter <= 5)
                        {
                            whileCounter++;
                            waitingTime = waitingTime * 2;
                            System.Console.WriteLine( whileCounter + " seconds waited so far");
                            System.Threading.Thread.Sleep(waitingTime);
                            result = gc.GetCoordinates(tweet.userlocation);
                        }
                        if (!result.Error.Equals("OVER_QUERY_LIMIT"))
                        {
                            if (result.Locations.Count > 0)
                            {
                                gmapsResultList.Add(new gmapsGeocoding
                                {
                                    formattedAddress = result.Locations.First().Address,
                                    longitude = result.Locations.First().Longitude,
                                    latitude = result.Locations.First().Latitude,
                                    tweetRandomSample2_Id = tweet.id,
                                    country = result.Locations.First().Country,
                                    type = result.Locations.First().Type
                                });
                            }
                            else
                            {
                                gmapsResultList.Add(new gmapsGeocoding
                                {
                                    tweetRandomSample2_Id = tweet.id,
                                    resultStatus = result.Error
                                });
                            }
                        }
                        else {
                            gmapsResultList.Add(new gmapsGeocoding
                            {
                                tweetRandomSample2_Id = tweet.id,
                                resultStatus=result.Error
                            });
                        }
                    }
                    else if (result.Error.Equals("REQUEST_DENIED"))
                    {
                        System.Console.WriteLine("Error: " + result);
                        System.Console.WriteLine("Request: " + result.request);
                        
                        int whileCounter = 0;
                        while (result.Error.Equals("REQUEST_DENIED"))
                        {
                            whileCounter++;
                            System.Console.WriteLine("Waiting for one second. " + whileCounter + "seconds waited so far");
                            System.Threading.Thread.Sleep(1000);
                            result = gc.GetCoordinates(tweet.userlocation);
                        }
                        if (!result.Error.Equals("OVER_QUERY_LIMIT"))
                        {

                            gmapsResultList.Add(new gmapsGeocoding
                            {
                                formattedAddress = result.Locations.First().Address,
                                longitude = result.Locations.First().Longitude,
                                latitude = result.Locations.First().Latitude,
                                tweetRandomSample2_Id = tweet.id,
                                country = result.Locations.First().Country,
                                type = result.Locations.First().Type
                            });
                        }
                        else
                        {
                            gmapsResultList.Add(new gmapsGeocoding
                            {
                                tweetRandomSample2_Id = tweet.id,
                                resultStatus = result.Error
                            });
                        }
                    }
                    else if (result.Error.Equals("ZERO_RESULTS"))
                    {
                        System.Console.WriteLine("No results");
                        string response = result.response;
                        gmapsResultList.Add(new gmapsGeocoding
                        {
                            tweetRandomSample2_Id = tweet.id,
                            resultStatus = result.Error
                        });

                    }
                    else if (result.Error.Equals("INVALID_REQUEST"))
                    {
                        System.Console.WriteLine("Error: " + result + "Program going to halt");
                        System.Console.WriteLine("Request: " + result.request);
                        string response = result.response;
                        gmapsResultList.Add(new gmapsGeocoding
                        {
                            tweetRandomSample2_Id = tweet.id,
                            resultStatus = result.Error
                        });

                    }
                    else
                    {
                        if (result.Locations.Count > 0)
                        {
                            gmapsResultList.Add(new gmapsGeocoding
                            {
                                formattedAddress = result.Locations.First().Address,
                                longitude = result.Locations.First().Longitude,
                                latitude = result.Locations.First().Latitude,
                                tweetRandomSample2_Id = tweet.id,
                                country = result.Locations.First().Country,
                                type = result.Locations.First().Type
                            });
                        }
                        else {
                            gmapsResultList.Add(new gmapsGeocoding
                            {
                                tweetRandomSample2_Id = tweet.id,
                                resultStatus = "ZERO_RESULTS_LOCAL_DETECTION"
                            });
                        
                        }
                    }
                   //todo: Save data to database
                    //conn.updateUniqueLocationFromTweetSample();

                }
               return gmapsResultList;
            }

            catch (Exception e)
            {
                System.Console.WriteLine("{0} Exception thrown", e);
                return gmapsResultList;
            }



        
        }

        private static void bingGeocoding(geoNamesDatabaseConnection conn, List<uniqueLocationFromTweetSample> uniqueLoc, int locationCount = 0)
        {
            if (locationCount == 0) {
                locationCount = uniqueLoc.Count;
            }

            GeoCoder gc = new GeoCoder();
            IGeoCodeResult result = new GeoCodeResult();
            try
            {
                for (int i = 0; i < locationCount; i++)
                {
                    result = new GeoCodeResult();
                    System.Console.WriteLine(i + "  " + uniqueLoc[i].locationString);
                    result = gc.GetCoordinates(uniqueLoc[i].locationString);
                    if (result.Error.Equals("No Results"))
                    {
                        System.Console.WriteLine("Error: " + result.Error + " some kind of strange MS Error Handling thing happened, waiting for 1 second.");
                        int whileCounter = 0;
                        int waitingTime = 1000;
                        while (result.Error.Equals("No Results") && whileCounter <= 10) {
                            whileCounter++;
                            waitingTime = whileCounter * 1000; 
                            System.Console.WriteLine("Waiting for one second. " + whileCounter + "seconds waited so far");
                            System.Threading.Thread.Sleep(waitingTime);
                            result = gc.GetCoordinates(uniqueLoc[i].locationString);
                        }
                        if (whileCounter == 10)
                        {
                            uniqueLoc[i].bingmaps = "NIL";
                        }
                        else {
                            uniqueLoc[i].bingmaps = result.Locations.First().Country;
                        }
                        
                        
                    }
                    else
                    {
                        uniqueLoc[i].bingmaps = result.Locations.First().Country;
                    }


                }
            }

            catch (Exception e)
            {
                System.Console.WriteLine("{0} Exception thrown", e);
            }
            finally
            {
                conn.addListToUniqueLocationFromTweetSample(uniqueLoc);
            }
        }

        private static List<List<GeoNameLocal>> getLikeSearchResultsForTweet(geoNamesDatabaseConnection conn, String locationString){
            List<List<GeoNameLocal>> geoNameResults = new List<List<GeoNameLocal>>();
            var processedStrings = locationPreprocessing(locationString);
            foreach (String str in processedStrings) {
                List<GeoNameLocal> tempList = new List<GeoNameLocal>(conn.like_search_GeoNamesRestricted_name_allFeature_(str));
                geoNameResults.Add(tempList);
            }
            return geoNameResults;
        }

        private static void transferUniqueLocationResolutionToResolutionList(geoNamesDatabaseConnection conn) {
            
            List<uniqueLocationFromTweetSample> gmapsResolution = new List<uniqueLocationFromTweetSample>(conn.executeQuery<uniqueLocationFromTweetSample>("select * from geonamesData.dbo.uniqueLocationFromTweetSample where gmaps is not null"));
            List<countryResolutionOutcome> resolutionList = new List<countryResolutionOutcome>(conn.executeQuery<countryResolutionOutcome>("select * from geonamesData.dbo.countryResolutionOutcome where country18 is null"));
            List<tweetRandomSample> tweetList = new List<tweetRandomSample>(conn.executeQuery<tweetRandomSample>("select * from geonamesData.dbo.tweetRandomSample join geonamesData.dbo.countryResolutionOutcome ON geonamesData.dbo.tweetRandomSample.id=geonamesData.dbo.countryResolutionOutcome.tweetRandomSample_Id where country18 is null"));

            foreach (tweetRandomSample tw in tweetList) {
                uniqueLocationFromTweetSample result = gmapsResolution.Find(x => x.locationString.Equals(tw.place));
                if (result != null)
                {
                    System.Console.WriteLine("tweet location string " + tw.place + " result unique " + result.locationString + " country result " + result.gmaps);
                }
                if (result != null && !result.gmaps.Contains("http")) {
                    System.Console.WriteLine("result and not http ");
                    int index = resolutionList.FindIndex(x => x.tweetRandomSample_Id == tw.id);
                    System.Console.WriteLine(" Index of resolution List " + index);
                    resolutionList[index].country18 = result.gmaps;
                    System.Console.WriteLine(" resolutionList country: " + resolutionList[index].country18 + " resolutionList tweet id: " + resolutionList[index].tweetRandomSample_Id);
                    System.Console.WriteLine("--------------------------------------------");
                }
            
            }
            conn.addListToCountryResolutionOutcome(resolutionList);

        
        }

        public static List<tokenized> tokenize(List<GeoNameLocal> geoNamesList){
        List<tokenized> tokenizedList = new List<tokenized>();
        String[] token = null;
        foreach(GeoNameLocal geoNameEntry in geoNamesList){
            token = null;
            token = locationPreprocessing(geoNameEntry.name);
            if (token.Length > 1) {
                tokenizedList.Add(new tokenized() { geoNamesId = geoNameEntry.geonameid, tokenArray = token });
            }
        } 
           return tokenizedList;
       }

        public static List<tokenized> tokenizeAlternates(List<alternateNames3> geoNamesList){
        List<tokenized> tokenizedList = new List<tokenized>();
        String[] token = null;
        foreach(alternateNames3 geoNameEntry in geoNamesList){
            token = null;
            token = locationPreprocessing(geoNameEntry.alternateName);
            if (token.Length > 1) {
                tokenizedList.Add(new tokenized() { geoNamesId = geoNameEntry.geoNameId.GetValueOrDefault(), tokenArray = token });
            }
        }

        return tokenizedList;
       }

        public static void addTokenizedStringToTokenTree(tokenized toAdd, string parentId, int level, GeonamesDataEntities1 conn, geoNamesId_To_geoTreeNodeId checkIsInConnection)
       {
           for (int i = level; i < toAdd.tokenArray.Count(); i++ )
           {
                //addNode(parentId, toAdd.tokenArray[i]);
               var watch4 = Stopwatch.StartNew();
               conn.insertNode_TokenTreeGeoNames(toAdd.tokenArray[i], parentId);
               watch4.Stop();
               Console.WriteLine("----insertNode in addTokenStringToTree: {0} Text: {1} parentId: {2}", watch4.Elapsed, toAdd.tokenArray[i], parentId);
              
               var query = conn.getNodeIdString(parentId, toAdd.tokenArray[i]);
                List<getNodeIdString_Result> liste = query.ToList();
                List<string> resultList = new List<string>();
                foreach (var str in liste)
                {
                    resultList.Add(str.HierarchyString);
                }
                parentId = resultList.First();
               
           }
           //parentId is the id of the leaf
               var watch5 = Stopwatch.StartNew();
           if (!(checkIsInConnection.testIsInList(new geonamesid_geotreenodeidstring {GeoNames_geonamesid=toAdd.geoNamesId,nodeId=parentId })))
                {
              watch5.Stop();
              Console.WriteLine("----addGeonames_tokenTree in addTokenStringToTree: {0} ", watch5.Elapsed);
      
               //System.Console.WriteLine("addTokenizedStringToTokenTree -> addNodeId_GeoNamesId");
               //conn.addNodeId_GeoNamesId(toAdd.geoNamesId, parentId);
               try
               {
                       //var watch5 = Stopwatch.StartNew();
                       conn.addNodeToGeonames(toAdd.geoNamesId, parentId);
                       //watch5.Stop();
                       //Console.WriteLine("----addGeonames_tokenTree in addTokenStringToTree: {0} ", watch5.Elapsed);
               }
               catch (Exception ex)
               {
                   System.Console.WriteLine("ERROR: " + ex.Message);
                   if (ex.InnerException != null)
                   {
                       System.Console.WriteLine("Inner Exception Message: " + ex.InnerException.Message);

                   }
               }
           }
           
       }

        private static void countryResolutionHierarchy(geoNamesDatabaseConnection conn) {
           

            List<Tweet> tweetList = conn.getAllTweetsRandomSample();
            List<uniqueLocationFromTweetSample> uniqueLocations = new List<uniqueLocationFromTweetSample>(conn.executeQuery<uniqueLocationFromTweetSample>("select * from geonamesData.dbo.uniqueLocationFromTweetSample"));
            List<countryResolutionOutcome> countryResolutionOutcome = new List<countryResolutionOutcome>(conn.executeQuery<countryResolutionOutcome>("select * from geonamesData.dbo.countryResolutionOutcome"));
            


            //Add all locations to a list with Lovcation Objects to work with. 
            List<Location> temporaryLocationList = new List<Location>();
            foreach (var uniqueLocation in uniqueLocations) {
                temporaryLocationList.Add(new Location(uniqueLocation.locationString));
            }



            List<GeoNameLocal> tempResultListGeoNames = new List<GeoNameLocal>();
            List<alternateNamesLocal> tempResultListAlternateName = new List<alternateNamesLocal>();
            GeoNameLocal tempGeoName = new GeoNameLocal();
            String query = null;
            int timeStampOne = 0;
            int timeStampTwo = 0;
            int count = 0;
            //iterate locations and search for sperated locations
            foreach (var location in temporaryLocationList) {
                count++;
               
                
                //for each location take every processed location find results in GeoNamesRestricted and alternateNames, if the geoname object 
                //found in alternatename is already in the list discard it else add it to the list.
                foreach (var processedLocation in location.seperatedLocations) {
                    timeStampOne = Environment.TickCount;
                    tempResultListGeoNames = new List<GeoNameLocal>(conn.like_search_GeoNamesRestricted_name_allFeature_(processedLocation));
                    tempResultListAlternateName = new List<alternateNamesLocal>(conn.like_search_alternateNames_name_allFeature(processedLocation));
                    
                    foreach(var alternateName in tempResultListAlternateName){
                        if(!tempResultListGeoNames.Any(x => x.geonameid == alternateName.geoNameId)){
                            query = "select * from geonamesData.dbo.GeoNames where geoNameId=" + alternateName.geoNameId;
                            tempGeoName = new GeoNameLocal();
                            tempGeoName = conn.executeQuery<GeoNameLocal>(query)[0];
                            tempResultListGeoNames.Add(tempGeoName);
                        }
                    }
                    
                
                }
                location.locationResults.Add(tempResultListGeoNames);
                timeStampTwo = Environment.TickCount;
                string passedTime = TimeSpan.FromMilliseconds(timeStampTwo - timeStampOne).ToString(@"dd\:hh\:mm\:ss\:ff");
                System.Console.WriteLine("Passed Time: " + passedTime);
                System.Console.WriteLine("----------------Location " + count + "/" + temporaryLocationList.Count());
                if (count > 4) { break; }
            }



            int number = 1;
                System.Console.WriteLine("locationString " + temporaryLocationList[number].locationString);
                foreach (var procloc in temporaryLocationList[number].seperatedLocations) {
                    System.Console.WriteLine(procloc);
                
                }
                foreach (var locRes in temporaryLocationList[number].locationResults) {
                    System.Console.WriteLine("--------------processedLoc");
                    foreach (var locString in locRes) {
                        System.Console.WriteLine(locString.name + " " + locString.geonameid);
                    }
                
                
                }
            
            
            
            
            //System.Console.WriteLine(conn.like_search_GeoNamesRestricted_name_allFeature_("New York")[0].name);
            //List<GeoNameLocal> tempGeoNameList = conn.like_search_GeoNamesRestricted_name_allFeature_("New York");
            //List<alternateNamesLocal> alternateNamesList = new List<alternateNamesLocal>(new List<alternateNamesLocal>(conn.like_search_alternateNames_name_allFeature("Bundesrepublik Deutschland")));



            
        
        
        }

        private static PinAuthorizer twitter() {
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

        private static void statusUpdate(String tweetText, PinAuthorizer auth) {
            using (var twitter = new TwitterContext(auth))
            {
                var tweet =  twitter.TweetAsync(tweetText);
            }
        
        
        } 

        public static void findDeepestMatchInTokenTree(tokenized tokenData, GeonamesDataEntities1 conn, geoNamesId_To_geoTreeNodeId checkIsInConnection) {
            string rootId = "/";
            List<string> nodeIds = new List<string>();
            nodeIds.Add(rootId);
            string parentId;
            
            //stopwatch
            //var watch1 = Stopwatch.StartNew();

            for(int i=0; i<tokenData.tokenArray.Count();i++){
                parentId = nodeIds.First();
                
                List<getNodeIdString_Result> liste = conn.getNodeIdString(parentId, tokenData.tokenArray[i]).ToList();
                List<string> resultList = new List<string>();
                foreach (var str in liste)
                {
                    resultList.Add(str.HierarchyString);
                }
                nodeIds = resultList;
                
                //no nodes found so add the rest of the token array
                if (nodeIds.Count() == 0)
                {
                    
                    addTokenizedStringToTokenTree(tokenData, parentId, i, conn,checkIsInConnection);
                    break;
                }
                //More than One node found this is an error because this shouldnt happen
                else if (nodeIds.Count > 1) {
                    System.Console.WriteLine("Error: there are " + nodeIds.Count() + " matches in level " + (i + 1) + " with NodeText" + tokenData.tokenArray[i]);
                }
                //the complete token array is already in the tree so add just a database entry
                else if (nodeIds.Count() == 1 && tokenData.tokenArray.Count() - 1 == i) {
                    if (!(checkIsInConnection.testIsInList(new geonamesid_geotreenodeidstring { nodeId = nodeIds.First(), GeoNames_geonamesid = tokenData.geoNamesId })))
                        //!(checkIsInConnection.testIsInList(new geonamesid_geotreenodeidstring { nodeId = nodeIds.First(), GeoNames_geonamesid = tokenData.geoNamesId }))
                    {
                        
                        //conn.addNodeId_GeoNamesId(tokenData.geoNamesId, nodeIds.First());
                        try
                        {

                            conn.addNodeToGeonames(tokenData.geoNamesId, nodeIds.First());
                            
                        }
                        catch (Exception ex)
                        {
                            System.Console.WriteLine("ERROR: " + ex.Message);
                            if (ex.InnerException != null)
                            {
                                System.Console.WriteLine("Inner Exception Message: " + ex.InnerException.Message);

                            }
                        }
                        System.Console.WriteLine("Done");
                    }
                
                }
                // there is exactly one entry just go down the tree one more level
                else if (nodeIds.Count() == 1)
                {
                }
                
            }
            //watch1.Stop();
            //Console.WriteLine("-Main Loop Token: {0}",watch1.Elapsed);
        
        }        

        public static float getDistance(float point1, float point2){
            
        float value;
            value = (float)1.5;
        return value;
        }


        static void Main(string[] args)
        {

            //createTokenTree();

            //geoNamesDatabaseConnection conn = new geoNamesDatabaseConnection();
            //List<coordinates> coordList = new List<coordinates>();
            //using (GeonamesDataEntities1 DB = new GeonamesDataEntities1())
            //{
            //    var query = from gg in DB.gmapsGeocoding
            //                join trs in DB.tweetRandomSample2
            //                on gg.tweetRandomSample2_Id equals trs.id

            //                select new coordinates
            //                {
            //                    id = gg.id,
            //                    point1 = gg.coord,
            //                    point2 = trs.coord
            //                };
            //    foreach (var obj in query)
            //    {
            //        coordList.Add(obj);
            //    }
            //    foreach (var obj in coordList)
            //    {

            //        if (obj.point1 != null && obj.point2 != null)
            //        {
            //            var upd = DB.gmapsGeocoding.Find(obj.id);
            //            if (upd != null)
            //            {
            //                System.Console.WriteLine(obj.id);
            //                upd.distance_to_tweet_coord = obj.point1.Distance(obj.point2);
            //                DB.Entry(upd).Property(e => e.distance_to_tweet_coord).IsModified = true;

            //            }
            //        }
            //        DB.SaveChanges();
            //    }

            //};

            
           
         
            
            
            
            geoCodingGMapseRoutine();
         
            //gmapsGeocoding(conn, uniqueLoc);
            //gmapsGeocoding(conn,uniqueLoc,2500);

                /**
                 * emergency function to reset a column to null,  
                 * be careful and set the country field 
                 **/
                //setCountryFieldToNull()

#if DEBUG
                System.Console.WriteLine("Press any key to quit !");
                System.Console.ReadLine();
#endif
        }

        private static void geoCodingGMapseRoutine()
        {
            geoNamesDatabaseConnection conn = new geoNamesDatabaseConnection();
            List<tweetRandomSample2> tweetliste = conn.executeQuery<tweetRandomSample2>("SELECT TOP 2500 trs.* FROM [dbo].[tweetRandomSample2] trs LEFT JOIN [gmapsGeocoding] mA ON trs.id = mA.tweetRandomSample2_id WHERE mA.tweetRandomSample2_id IS NULL AND userlocation != ''");
            GeonamesDataEntities1 db = new GeonamesDataEntities1();



            List<gmapsGeocoding> liste = gmapsGeocodingJSONResult(conn, tweetliste, 0);
            foreach (var ttt in liste)
            {

                db.gmapsGeocoding.Add(ttt);

            }
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
        }

        private static void createTokenTree()
        {
            PinAuthorizer tw = twitter();

            geoNamesDatabaseConnection conn = new geoNamesDatabaseConnection();
            geoNamesId_To_geoTreeNodeId geoNamesId_To_geoTreeNode = new geoNamesId_To_geoTreeNodeId();
            geoNamesId_To_geoTreeNode.list = conn.executeQuery<geonamesid_geotreenodeidstring>("SELECT * FROM [dbo].geonamesid_geotreenodeidstring");
            List<alternateNames3> liste = conn.executeQuery<alternateNames3>("SELECT * FROM [dbo].[View_1] ORDER BY alternateName");

            List<tokenized> result = tokenizeAlternates(liste);
            System.Console.WriteLine(result.Count());

            int timeStampTwo;
            int timeStampOne;
            using (GeonamesDataEntities1 DB = new GeonamesDataEntities1())
            {
                timeStampOne = Environment.TickCount;
                int timeStampOnePersistent = timeStampOne;
                int i = 1503000;
                foreach (var ttt in result.Skip(1503000))
                {
                    findDeepestMatchInTokenTree(ttt, DB, geoNamesId_To_geoTreeNode);
                    i++;
                    if (i % 1000 == 0)
                    {
                        timeStampTwo = Environment.TickCount;
                        string timeOne = calculatePassedTimeAndPrint(timeStampTwo, timeStampOne);
                        string timeTwo = calculatePassedTimeAndPrint(timeStampTwo, timeStampOnePersistent);
                        string tweetText = "count: " + i + " timeLast: " + timeOne + " timeAll: " + timeTwo + " @pide2001 #tlefkit";
                        statusUpdate(tweetText, tw);
                        timeStampOne = Environment.TickCount;
                    }
                }
            }
        }
        private static string calculatePassedTimeAndPrint(int timeStampTwo,int timeStampOne)
        {
            string passedTime = TimeSpan.FromMilliseconds(timeStampTwo - timeStampOne).ToString(@"dd\:hh\:mm\:ss\:ff");
            System.Console.WriteLine("Passed Time: " + passedTime);
            return passedTime;
        }


     

        private static void countSigns()
        {
            twitterStreamDatabaseConnector connection = new twitterStreamDatabaseConnector();
            List<String> firstList = new List<String>(connection.executeQuery<String>("SELECT userlocation FROM [dbo].[Retweets] where id in(select min(id) from [dbo].[Retweets] group by tweetid)"));
            List<String> secondList = new List<String>(connection.executeQuery<String>("SELECT sourceuserlocation FROM [dbo].[Retweets] where id in(select min(id) from [dbo].[Retweets] group by tweetid)"));
            firstList.AddRange(secondList);


            Dictionary<Tuple<char, string>, int> charlist = new Dictionary<Tuple<char, string>, int>();

            //int timeStampOne = 0;
            int timeStampTwo = 0;
            int timeStampThree = Environment.TickCount;
            int counter = 0;
            foreach (var userlocation in firstList)
            {
                counter++;
                //timeStampOne = Environment.TickCount;
                char[] liste = getSigns(userlocation);
                Tuple<char, string> characterAndCode = null;
                foreach (var character in liste)
                {
                    var intchar = (int)character;
                    characterAndCode = new Tuple<char, string>(character, intchar.ToString("X4"));
                    if (charlist.ContainsKey(characterAndCode))
                    {

                        charlist[characterAndCode] += 1;
                    }
                    else
                    {
                        charlist.Add(characterAndCode, 1);
                    }

                }
                timeStampTwo = Environment.TickCount;
                //string passedTime = TimeSpan.FromMilliseconds(timeStampTwo - timeStampOne).ToString(@"dd\:hh\:mm\:ss\:ff");
                if (counter % 500000 == 0)
                {
                    string wholeTime = TimeSpan.FromMilliseconds(timeStampThree - timeStampTwo).ToString(@"dd\:hh\:mm\:ss\:ff");
                    System.Console.WriteLine("#userlocations so far:" + counter + " already passed time since start: " + wholeTime);
                }
            }
            //System.Console.WriteLine(charlist.Count());
            var sortedDict = from entry in charlist orderby entry.Value descending select entry;
            saveAsJsonToFile(@"C:\Users\bolch\TwitterAnalysisData\signCount.json", sortedDict);
        }

        public class signCount{
            public Tuple<char,string> Key;
            public int Value;
        
        }

        private static List<dynamic> getJSON(string filepath) {
            using (StreamReader reader = new StreamReader(filepath)) {
                string json = reader.ReadToEnd();
                List<dynamic> items = JsonConvert.DeserializeObject<List<dynamic>>(json);
                return items;
            }
        
        
        }

        private static void saveTextToFile(string text, string filePath) {
            System.IO.File.WriteAllText(@filePath, text);
        
        }

        private static void saveAsJsonToFile(String filepath,dynamic objectToSave)
        {
            using (FileStream fs = File.Open(filepath, FileMode.CreateNew))
            using (StreamWriter sw = new StreamWriter(fs))
            using (JsonWriter jw = new JsonTextWriter(sw))
            {
                jw.Formatting = Formatting.Indented;
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(jw, objectToSave);

            }
        }

        private static TupleList<float,float> calculatePrecisionRecall(geoNamesDatabaseConnection conn)
        {
            List<countryResolutionOutcome> croL = new List<countryResolutionOutcome>(conn.executeQuery<countryResolutionOutcome>("select * from geonamesData.dbo.countryResolutionOutcome"));

            TupleList<string, string> countryListGmaps = new TupleList<string, string>();
            TupleList<string, string> countryListCase1 = new TupleList<string, string>();
            TupleList<string, string> countryListCase2 = new TupleList<string, string>();
            TupleList<string, string> countryListCase3 = new TupleList<string, string>();
            TupleList<string, string> countryListCase4 = new TupleList<string, string>();
            TupleList<string, string> countryListCase5 = new TupleList<string, string>();
            TupleList<string, string> countryListCase6 = new TupleList<string, string>();
            TupleList<string, string> countryListCase7 = new TupleList<string, string>();
            TupleList<string, string> countryListCase8 = new TupleList<string, string>();
            TupleList<string, string> countryListCase9 = new TupleList<string, string>();
            TupleList<string, string> countryListCase10 = new TupleList<string, string>();

            foreach (var ele in croL)
            {
                countryListGmaps.Add(ele.country1, ele.country18);
                countryListCase1.Add(ele.country1, ele.country2);
                countryListCase2.Add(ele.country1, ele.country3);
                countryListCase3.Add(ele.country1, ele.country4);
                countryListCase4.Add(ele.country1, ele.country5);
                countryListCase5.Add(ele.country1, ele.country6);
                countryListCase6.Add(ele.country1, ele.country7);
                countryListCase7.Add(ele.country1, ele.country8);
                countryListCase8.Add(ele.country1, ele.country9);
                countryListCase10.Add(ele.country1, ele.country10);
            }

            System.Console.WriteLine(countryListGmaps.Count());

            Tuple<float, float> pr = precisionRecallSingle(countryListGmaps);
            Tuple<float, float> pr2 = precisionRecallSingle(countryListCase1);
            Tuple<float, float> pr3 = precisionRecallSingle(countryListCase2);
            Tuple<float, float> pr4 = precisionRecallSingle(countryListCase3);
            Tuple<float, float> pr5 = precisionRecallSingle(countryListCase4);
            Tuple<float, float> pr6 = precisionRecallSingle(countryListCase5);
            Tuple<float, float> pr7 = precisionRecallSingle(countryListCase6);
            Tuple<float, float> pr8 = precisionRecallSingle(countryListCase7);
            Tuple<float, float> pr9 = precisionRecallSingle(countryListCase8);
            Tuple<float, float> pr10 = precisionRecallSingle(countryListCase9);

            TupleList<float, float> precisionRecallList = new TupleList<float, float>();
            precisionRecallList.Add(pr);
            precisionRecallList.Add(pr2);
            precisionRecallList.Add(pr3);
            precisionRecallList.Add(pr4);
            precisionRecallList.Add(pr5);
            precisionRecallList.Add(pr6);
            precisionRecallList.Add(pr7);
            precisionRecallList.Add(pr8);
            precisionRecallList.Add(pr9);
            precisionRecallList.Add(pr10);
            return precisionRecallList;
        }

        
        private static void setCountryFieldToNull()
        {
            geoNamesDatabaseConnection conn = new geoNamesDatabaseConnection();
            List<countryResolutionOutcome> countryResolutionOutcomeList = new List<countryResolutionOutcome>();
            List<Tweet> liste = new List<Tweet>(conn.getAllTweetsRandomSample());
            foreach (Tweet twe in liste)
            {
                countryResolutionOutcomeList.Add(new countryResolutionOutcome { tweetRandomSample_Id = twe.id, country2 = null });

            }
            conn.addListToCountryResolutionOutcome(countryResolutionOutcomeList);
        }

       
    }
}


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwitterWebFrontend.Models;
using TwitterWebFrontend.ViewModels;

namespace TwitterWebFrontend.Controllers
{
    public class StatisticsController : Controller
    {

        public static Dictionary<string,double?> getStatisticValues(List<double?> valueList, int divider, int round = 100){
            double? average = 0;
            double? median = 0;
            double? firstQuartil = 0;
            double? thirdQuartil = 0;
            double? valueListCount = valueList.Count();
            Dictionary<string, double?> resultDict = new Dictionary<string, double?>();

            average = valueList.Average();
            
            
            if (valueListCount % 2 == 0)
            {
                median = (valueList[(int)(valueListCount / 2)] + valueList[(int)((valueListCount / 2) + 1)]) / 2;
                  }
            else {
                median = valueList[(int)((valueListCount + 1) / 2)];
                
            }
            
            firstQuartil = valueList[(int)(0.25 * (valueListCount + 1))];
            thirdQuartil = valueList[(int)(0.75 * (valueListCount + 1))];
           
            resultDict.Add("Average", Math.Round((double)(average / divider),round));
            resultDict.Add("First Quartil", Math.Round((double)(firstQuartil / divider),round));
            resultDict.Add("Median", Math.Round((double)(median / divider), round));
            resultDict.Add("Third Quartil", Math.Round((double)(thirdQuartil / divider), round));

            return resultDict;
        }

        public static Dictionary<string, double?> getPrecisionRecall(List<GeoNames_trs2_gmapsCoding> countryData) {
            Dictionary<string, double?> resultDict = new Dictionary<string, double?>() ;

            double truePositive = 0;
            double falsePositive = 0;
            double falseNegative = 0;

            double precision = 0;
            double recall = 0;

            foreach (var item in countryData) {
                if (!(String.IsNullOrEmpty(item.country_of_tweet_by_distance_method)))
                {
                    if (String.IsNullOrEmpty(item.gmaps_country))
                    {
                        falseNegative++;
                    }
                    else if (item.gmaps_country.Replace(" ", "").Equals(item.country_of_tweet_by_distance_method.Replace(" ", "")))
                    {
                        truePositive++;
                    }
                    else if (!item.gmaps_country.Replace(" ", "").Equals(item.country_of_tweet_by_distance_method.Replace(" ", "")))
                    {
                        falsePositive++;
                       
                    }
                    
                }

            }
            precision = Math.Round((truePositive / (truePositive + falsePositive))*100,2);
            recall = Math.Round((truePositive / (truePositive + falseNegative))*100,2);
          

            resultDict.Add("Precision", precision);
            resultDict.Add("Recall", recall);

            return resultDict;
            
        }

        

        //
        // GET: /Statistics/gmapsGeocodingStatistics
        private GeonamesDataEntities1 db = new GeonamesDataEntities1();
        public ActionResult gmapsGeocodingStatistics()
        {
            var gmapsGeocodingResult = new gmapsGeocodingStatisticsViewModel();
            List<double?> distancesList = new List<double?>();
            gmapsGeocodingResult.dataDictDynamic = new List<Dictionary<string, dynamic>>();
            Dictionary<string, dynamic> cumulativeFrequencyData = new Dictionary<string, dynamic>();
            List<gmapsGeocoding> gmapsGeocodingData = new List<gmapsGeocoding>();
            List<GeoNames_trs2_gmapsCoding> countryData = new List<GeoNames_trs2_gmapsCoding>();
            gmapsGeocodingData = db.gmapsGeocoding.ToList();
            gmapsGeocodingResult.gmapsGeocoding = gmapsGeocodingData;


            countryData = db.GeoNames_trs2_gmapsCoding.ToList();

            distancesList = (from s in db.gmapsGeocoding where s.distance_to_tweet_coord.HasValue
                                         orderby s.distance_to_tweet_coord ascending
                                         select s.distance_to_tweet_coord).ToList();


            int counter = 0;
            int distListCount = distancesList.Count();
            for(int i = 1;i<distancesList.Last()/1000;i++){
                counter = distancesList.Where(x => (x / 1000) <= i*10).Count();
                if (counter < distListCount) {
                    cumulativeFrequencyData.Add((i * 10).ToString(), counter);
                    }
                }   
            
            
            gmapsGeocodingResult.dataDictDynamic.Add(cumulativeFrequencyData);
            gmapsGeocodingResult.dataDict = getStatisticValues(distancesList,1000,2);
            gmapsGeocodingResult.dataDict2 = getPrecisionRecall(countryData);
            
            return View(gmapsGeocodingResult);
        }

        // GET: /Statistics/trsStatistics
        public ActionResult trsStatistics()
        {
            var trsViewModel = new trsStatisticsViewModel();
            trsViewModel.dataDict = new Dictionary<string, double?>();
            List<tweetRandomSample2> trsList = new List<tweetRandomSample2>();
            trsList = db.tweetRandomSample2.ToList();

            int count = trsList.Count(); 
            double wrongLatLng = 0;
           
            
            foreach (var item in trsList)
            {
                
            }
            
            
           

            return View(trsViewModel);
        }



        public ActionResult Index()
        {

            return View();
        }
    }
}
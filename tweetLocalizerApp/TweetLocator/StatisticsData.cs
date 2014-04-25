using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tweetLocalizerApp.TweetLocator
{
    class StatisticsData
    {
        
        public List<Tuple<GeographyData,TweetKnowledgeObj>> geographyDataList { get; set; }
        public List<double> distances { get; set; }
        public double accumulatedDistances {get;set;}
        public int distancesCount { get; set; }

        public StatisticsData() {
            distances = new List<double>();
            geographyDataList = new List<Tuple<GeographyData,TweetKnowledgeObj>>();
        }

        public void addGeographyDataTweetKnowledge(GeographyData geography,TweetKnowledgeObj tweetKnow)
        {
            this.geographyDataList.Add(Tuple.Create(geography,tweetKnow));
        }

        public Tuple<GeographyData,TweetKnowledgeObj> getBiggestDistanceAndInformation()
        {
            geographyDataList = geographyDataList.OrderBy(x=>x.Item1.distance).ToList();
            return geographyDataList.Last();
        }

        public void addDistances(double distance) {
            distances.Add(distance);
            this.accumulatedDistances+=distance;
            distancesCount++;
        }

        public double getBiggestDistance() {
            distances.Sort();
            return distances.Last();
        }

        public double getSmallestDistance()
        {
            distances.Sort();
            return distances.First();
        }

        public double getMedianOfDistances() {
            double median = 0;
            distances.Sort();
             if (distancesCount % 2 == 0)
            {
                median = (distances[(int)(distancesCount / 2)] + distances[(int)((distancesCount / 2) + 1)]) / 2;
                  }
            else {
                median = distances[(int)((distancesCount + 1) / 2)];
            }
            return median;
        }

        public double getAverageDistance() {
            double average = 0;
            average = accumulatedDistances / distancesCount;
            return average;
        
        }


    }
}

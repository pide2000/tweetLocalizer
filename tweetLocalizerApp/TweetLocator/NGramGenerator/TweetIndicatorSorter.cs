using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tweetLocalizerApp.Libs;

namespace tweetLocalizerApp.TweetLocator
{
    class TweetIndicatorSorter : IIndicatorSorter
    {

        public List<Tuple<string, List<string>>> sortEncodedIndicatorTokens(Dictionary<string, List<string>> locationIndicator)
        {
            List<Tuple<string, List<string>>> sortedList = new List<Tuple<string, List<string>>>();
            sortedList.Add(Tuple.Create("USERLOCATION",locationIndicator["USERLOCATION"]));
            sortedList.Add(Tuple.Create("TIMEZONE", locationIndicator["TIMEZONE"]));
            return sortedList;


        }
    }
}

using System.Collections.Generic;
using TwitterWebFrontend.Models;

namespace TwitterWebFrontend.ViewModels
{
    public class trsStatisticsViewModel
    {
        public IEnumerable<tweetRandomSample2> gmapsGeocoding { get; set; }

        public List<Dictionary<string,dynamic>> dataDictDynamic { get; set; }

        public Dictionary<string, double?> dataDict { get; set; }

        public Dictionary<string, double?> dataDict2 { get; set; }
        
    }
}

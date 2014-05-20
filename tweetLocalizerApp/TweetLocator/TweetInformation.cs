using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tweetLocalizerApp.Libs.Locator;
using System.Data.Entity.Spatial;

namespace tweetLocalizerApp.TweetLocator
{
    public class TweetInformation
    {
        public string userlocation { get; set; }
        public string timezone { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
        public int baseDataId { get; set; }

        public System.Data.Entity.Spatial.DbGeography coord { get;set; }

        public int randomSampleId { get; set; }
    }
}

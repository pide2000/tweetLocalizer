using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Spatial;
using tweetLocalizerApp.Libs;

namespace tweetLocalizerApp.TweetLocator
{
    public class GeographyData : IGeographyData
    {
        public int? geonamesId { get; set; }
        public int? countryId { get; set; }
        public int? admin1Id { get; set; }
        public int? admin2Id { get; set; }
        public double? distance { get; set; }

    }
}

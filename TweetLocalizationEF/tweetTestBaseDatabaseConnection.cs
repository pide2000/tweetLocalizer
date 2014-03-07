using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetLocalizationEF;

namespace TweetLocalizationEF
{
    class tweetsLocal : tweetRandomSample { }

    class coordinatesOfTweet {
        public float id { get; set; }
        public float lat { get; set; }
        public float lon { get; set; }
    }

    class tweetTestBaseDatabaseConnection
    {
        public List<coordinatesOfTweet> getCoordinatesOfTweetById(int id) {
            using (GeonamesDataEntities1 tweetsDb = new GeonamesDataEntities1())
            {
                var query = (
                    from db in tweetsDb.tweetRandomSample
                    where db.id == id
                    select new coordinatesOfTweet { 
                        id = db.id,
                        lat = (float)db.lat,
                        lon = (float)db.lon
                    }
                    );
                    List<coordinatesOfTweet> ret = query.ToList();
                    return ret;
                    }

                
                
                    }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tweetLocalizerApp.Libs;

namespace tweetLocalizerApp.TweetLocator
{
    class TweetKnowledgeObj : knowledgeObj
    {
        public Dictionary<String, List<string>> indicatorTokens { get; set; }
        public string userlocation { get; set; }
        public string timezone { get; set; }

        public UserlocationIndicator<string> userlocationIndicator { get; set; }

        public TimezoneIndicator<string> timezoneIndicator { get; set; }

        public TweetKnowledgeObj() {
            this.indicatorTokens = new Dictionary<string, List<string>>();
        }
        

        

    }
}

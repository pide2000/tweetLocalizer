using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tweetLocalizerApp.Libs.Locator;

namespace tweetLocalizerApp.TweetLocator
{
    class StandardEncoder : EncoderAbstract<string>
    {
        public override List<string> encode(List<string> indicatorTokens)
        {
            return indicatorTokens;
        }
    }

    class userLocationEncoder : EncoderAbstract<string>
    {
        public override List<string> encode(List<string> indicatorTokens)
        {
            return indicatorTokens;
        }
    }
}

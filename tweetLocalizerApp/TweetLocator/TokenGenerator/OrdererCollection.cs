using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tweetLocalizerApp.TweetLocator;

namespace tweetLocalizerApp.TweetLocator
{
    class StandardOrderer : SorterAbstract<string>
    {
        public override List<string> sort(List<string> indicatorTokens)
        {
            indicatorTokens.Sort();
            return indicatorTokens;
        }
    }

    class OrderUserlocation : SorterAbstract<string>
    {
        public override List<string> sort(List<string> indicatorTokens)
        {
            indicatorTokens.Sort();
            return indicatorTokens;
        }
    }
}

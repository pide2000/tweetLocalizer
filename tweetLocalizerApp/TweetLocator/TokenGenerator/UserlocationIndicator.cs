using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tweetLocalizerApp.Libs.Locator;

namespace tweetLocalizerApp.TweetLocator
{
    class UserlocationIndicator<T> : LocationIndicatorAbstract<T>
    {
        public UserlocationIndicator(string indicatorType, T indicatorItem) : base(indicatorType,indicatorItem){}
    }
}

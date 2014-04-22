using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tweetLocalizerApp.TweetLocator
{
    public class TimezoneIndicator<T> : LocationIndicatorAbstract<T>
    {
        public TimezoneIndicator(string indicatorType, T indicatorItem) : base(indicatorType, indicatorItem) { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tweetLocalizerApp.Libs.Locator;

namespace tweetLocalizerApp.TweetLocator
{
    abstract class SorterAbstract<T> : ISorter<T>
    {
        private List<String> encodedIndicatorTokens;
        private List<String> sortedIndicatorTokens;

        //making a local copy to work with
        private void getEncodedIndicatorTokens(List<String> encodedIndicatorTokens)
        {
            this.encodedIndicatorTokens = encodedIndicatorTokens;
        }
        public abstract List<String> sort(List<String> indicatorTokens);

        public List<String> sortEncodedIndicatorTokens(List<String> encodedIndicatorTokens)
        {
            getEncodedIndicatorTokens(encodedIndicatorTokens);
            sortedIndicatorTokens = sort(this.encodedIndicatorTokens);
            return sortedIndicatorTokens;
        }
    }
}

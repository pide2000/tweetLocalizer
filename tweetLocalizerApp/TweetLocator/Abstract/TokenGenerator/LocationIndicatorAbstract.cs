using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tweetLocalizerApp.Libs.Locator;

namespace tweetLocalizerApp.TweetLocator
{
    public abstract class LocationIndicatorAbstract<T> : ILocationIndicator<T>
    {
        public string indicatorType { get; set; }
        public T originalIndicatorItem { get; set; }
        public T currentIndicatorItem { get; set; }
        public List<T> indicatorTokens { get; set; }
        public List<String> encodedIndicatorTokens { get; set; }
        public List<String> orderedIndicatorTokens { get; set; }
        public List<String> finalIndicatorTokens { get; set; }
        public String indicatorSequence { get; set; }
        public IPreprocessor<T> lastPreprocessor { get; set; }

        private int preprocessorCounter;
        
        
        public LocationIndicatorAbstract(string indicatorType,T indicatorItem) {
            this.originalIndicatorItem = indicatorItem;
            this.indicatorType = indicatorType;
            this.currentIndicatorItem = indicatorItem;
        }

        public void incrementPreprocessorCounter()
        {
            this.preprocessorCounter++;
        }

        public void decrementPreprocessorCounter()
        {
            this.preprocessorCounter--;
        }
    }
}

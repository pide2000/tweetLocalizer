using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tweetLocalizerApp.Libs.Locator
{
    public interface ILocationIndicator<T>
    {
        string indicatorType { get; set; }
        T originalIndicatorItem { get; set; }
        T currentIndicatorItem { get; set; }
        List<String> encodedIndicatorTokens { get; set; }
        IPreprocessor<T> lastPreprocessor { get; set; }
        void incrementPreprocessorCounter();
        void decrementPreprocessorCounter();
        List<T> indicatorTokens {get; set;}
        List<String> orderedIndicatorTokens { get; set; }
        String indicatorSequence { get; set; }
        List<String> finalIndicatorTokens { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tweetLocalizerApp.TweetLocator;
using tweetLocalizerApp.Libs;
using tweetLocalizerApp.Libs.Locator;

namespace tweetLocalizerApp.TweetLocator
{
    public class TimezoneTokenGenerator<T> : TokenGeneratorAbstract<T>
    {

        public override void configure(List<IPreprocessor<T>> preprocessorList, ITokenizer<T> tokenizer, IEncoder<T> encoder, ISorter<T> orderer)
        {
            this.preprocessorList = preprocessorList;
            this.orderer = orderer;
            this.encoder = encoder;
            this.tokenizer = tokenizer;
            
        }

        public override void assemblyToken(ILocationIndicator<T> locationIndicator)
        {
            
                foreach (IPreprocessor<T> preprocessor in this.preprocessorList)
                {
                    locationIndicator.currentIndicatorItem = preprocessor.processLocationIndicator(locationIndicator.currentIndicatorItem);
                    locationIndicator.lastPreprocessor = preprocessor;
                    locationIndicator.incrementPreprocessorCounter();
                }
                locationIndicator.indicatorTokens = this.tokenizer.tokenizeIndicatorItem(locationIndicator.currentIndicatorItem);
                locationIndicator.encodedIndicatorTokens = this.encoder.encodeIndicatorTokens(locationIndicator.indicatorTokens);
                locationIndicator.orderedIndicatorTokens=this.orderer.sortEncodedIndicatorTokens(locationIndicator.encodedIndicatorTokens);
                locationIndicator.finalIndicatorTokens = locationIndicator.orderedIndicatorTokens;
                
            
        }

       
    }
}

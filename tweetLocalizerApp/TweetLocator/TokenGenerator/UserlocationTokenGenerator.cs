using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tweetLocalizerApp.Libs.Locator;

namespace tweetLocalizerApp.TweetLocator
{
    public class UserlocationTokenGenerator<T> : TokenGeneratorAbstract<T>
    {
        public override void configure(List<IPreprocessor<T>> preprocessorList, ITokenizer<T> tokenizer, IEncoder<T> encoder, ISorter<T> orderer, List<string> stopwords)
        {
            this.preprocessorList = preprocessorList;
            this.orderer = orderer;
            this.encoder = encoder;
            this.tokenizer = tokenizer;
            this.stopwords = stopwords;
        }

        public override void assemblyToken(ILocationIndicator<T> locationIndicator)
        {
            foreach (IPreprocessor<T> preprocessor in this.preprocessorList)
            {
                locationIndicator.currentIndicatorItem = preprocessor.processLocationIndicator(locationIndicator.currentIndicatorItem);          
            }
            locationIndicator.indicatorTokens = this.tokenizer.tokenizeIndicatorItem(locationIndicator.currentIndicatorItem);
            List<T> tempIndicatorTokens = new List<T>(locationIndicator.indicatorTokens);
            foreach (var token in tempIndicatorTokens) {
                if (this.stopwords.Any(g => g.Equals(token)))
                {
                    locationIndicator.indicatorTokens.Remove(token);
                }
                
            }
            
            locationIndicator.encodedIndicatorTokens = this.encoder.encodeIndicatorTokens(locationIndicator.indicatorTokens);
            
            
            
            locationIndicator.orderedIndicatorTokens = this.orderer.sortEncodedIndicatorTokens(locationIndicator.encodedIndicatorTokens);
            locationIndicator.finalIndicatorTokens = locationIndicator.orderedIndicatorTokens;
        }
    }
}

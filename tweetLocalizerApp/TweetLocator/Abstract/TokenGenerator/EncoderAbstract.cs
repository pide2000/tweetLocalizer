using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace tweetLocalizerApp.Libs.Locator
{
    abstract class EncoderAbstract<T> : IEncoder<T>
    {
        private List<T> indicatorTokens;
        private List<String> encodedIndicatorTokens;


        //making a local copy to work with
        private void getIndicatorItem(List<T> indicatorTokens)
        {
            this.indicatorTokens = indicatorTokens;
        }

        //one has to ensure that the output is a list of strings. 
        public abstract List<String> encode(List<T> indicatorTokens);

        private List<String> finalEncoding(List<String> indicatorItems){
            List<String> urlEncodedItems = new List<string>();
            foreach (var item in indicatorItems)
            {
                urlEncodedItems.Add(WebUtility.UrlEncode(item));
            }
            return urlEncodedItems;
        
        }
        public List<String> encodeIndicatorTokens(List<T> indicatorTokens)
        {
            getIndicatorItem(indicatorTokens);
            encodedIndicatorTokens = encode(this.indicatorTokens);
            encodedIndicatorTokens = finalEncoding(encodedIndicatorTokens);
            return encodedIndicatorTokens;
        }
        
    }
}

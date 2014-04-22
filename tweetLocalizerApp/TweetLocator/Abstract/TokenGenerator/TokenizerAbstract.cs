using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tweetLocalizerApp.Libs.Locator
{
    abstract class TokenizerAbstract<T> : ITokenizer<T> 
    {

        private T indicatorItem;
        private List<T> indicatorTokens;
        public char seperator { get; set; }
        //making a local copy to work with
        private void getIndicatorItem(T indicatorItem) {
            this.indicatorItem = indicatorItem;
        }
       
        public abstract List<T> tokenize(T indicatorItem, char seperator);

        public List<T> tokenizeIndicatorItem(T indicatorItem){
            getIndicatorItem(indicatorItem);
            indicatorTokens = tokenize(this.indicatorItem,this.seperator);
            return indicatorTokens;
        }

    }
}

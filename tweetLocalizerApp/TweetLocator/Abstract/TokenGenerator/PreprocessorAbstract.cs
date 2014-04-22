using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tweetLocalizerApp.Libs.Locator
{
    abstract class PreprocessorAbstract<T>:IPreprocessor<T>
    {
        private T localIndicatorItem;
        private T processedIndicatorItem;

        private void getIndicatorItem(T indicatorItem)
        {
            this.localIndicatorItem = indicatorItem;
        }

        public abstract T preprocess(T indicatorItem);

        public T processLocationIndicator(T indicatorItem)
        {
            getIndicatorItem(indicatorItem);
            this.processedIndicatorItem = preprocess(this.localIndicatorItem);

            return this.processedIndicatorItem;
            
        }

    }
}

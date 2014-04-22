using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tweetLocalizerApp.Libs.Locator
{
    public interface IPreprocessor<T>
    {
        
        //perform the overall Process to preprocess the indicatorItem
        T processLocationIndicator(T currentItem);
    }
}

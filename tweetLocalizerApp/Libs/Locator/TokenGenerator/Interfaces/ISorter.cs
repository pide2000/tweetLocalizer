using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tweetLocalizerApp.Libs.Locator
{
    public interface ISorter<T>
    {
        List<String> sortEncodedIndicatorTokens(List<String> locationIndicator);
    }
}

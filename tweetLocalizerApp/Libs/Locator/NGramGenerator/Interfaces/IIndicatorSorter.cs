using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tweetLocalizerApp.Libs
{
    interface IIndicatorSorter
    {
        List<Tuple<string,List<string>>> sortEncodedIndicatorTokens(Dictionary<string,List<string>> locationIndicator);
    }
}

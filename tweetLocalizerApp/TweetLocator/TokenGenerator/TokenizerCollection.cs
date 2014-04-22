using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tweetLocalizerApp.Libs.Locator;

namespace tweetLocalizerApp.TweetLocator
{
    class Standardtokenizer : TokenizerAbstract<string>
    {

        public override List<String> tokenize(string indicatorItem, char seperator)
        {
            List<String> split = Array.ConvertAll(indicatorItem.Split(new Char[] { seperator }), p => p.Trim()).ToList();
            return split;
        }
    }

    class userLocationTokenizer : TokenizerAbstract<string>
    {

        public override List<String> tokenize(string indicatorItem, char seperator)
        {
            List<string> split = Array.ConvertAll(indicatorItem.Split(new Char[] { seperator }), p => p.Trim()).ToList();
            return split;
        }
    }
}

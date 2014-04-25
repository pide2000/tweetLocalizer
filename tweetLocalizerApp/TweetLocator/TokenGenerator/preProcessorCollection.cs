using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tweetLocalizerApp.Libs.Locator;

namespace tweetLocalizerApp.TweetLocator
{
    public class DeleteSigns : PreprocessorAbstract<string>
    {
        public override string preprocess(string indicatorItem)
        {
            String words = System.Text.RegularExpressions.Regex.Replace(indicatorItem, @"[^\p{L}\p{N} ]", " ");
            words = System.Text.RegularExpressions.Regex.Replace(words, @"\s+", " ").Trim();
            
            return words;
        }
    }

    class CheckGeolocation : PreprocessorAbstract<string>
    {
        public override string preprocess(string indicatorItem)
        {
            string[] split = Array.ConvertAll(indicatorItem.Split(new Char[] { ' ' }), p => p.Trim());
            int length = split.Length;

            //check every combination of Tokens against geonames database. search single tokens in normal base and 2-, 3- tokens in the tree
            string resultString = string.Join(":", split);
            return resultString;
        }
    }

    class ToLowerCase : PreprocessorAbstract<string>
    {
        public override string preprocess(string indicatorItem)
        {
            return indicatorItem.ToLower();

        }

    }


   

    
}

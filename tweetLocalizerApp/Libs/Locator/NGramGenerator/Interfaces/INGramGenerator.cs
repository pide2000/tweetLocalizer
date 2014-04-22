using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tweetLocalizerApp.Libs
{
    interface INGramGenerator
    {
        List<Ngram> generateNGrams(Dictionary<string, List<string>> indicatorTokenList, int nGramOrder);
    }
}

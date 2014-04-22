using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tweetLocalizerApp.Libs
{
    class Ngram : IEquatable<Ngram>
    {
        public string nGram { get; set; }
        /*
         * The nGramOrder is the Degree of the Ngram. 
         * 0 -> maxgram, 1->Unigram, 2->Bigram, 3->TriGram ......
         */
        public int nGramOrder { get; set; }

        public List<string> indicatorTypes;

        public Ngram(string ngram, int order,List<string> indicatorTypes )
        {
            this.nGram = ngram;
            this.nGramOrder = order;
            this.indicatorTypes = indicatorTypes ;
        }
        public Ngram(string ngram, int order)
        {
            this.nGram = ngram;
            this.nGramOrder = order;
        }

        public override int GetHashCode()
        {
            return this.nGram.GetHashCode() + this.nGramOrder.GetHashCode(); // Or something like that
        }


        public bool Equals(Ngram other)
        {
            return this.nGram == other.nGram && this.nGramOrder == other.nGramOrder;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tweetLocalizerApp.Libs;

namespace tweetLocalizerApp.TweetLocator
{
    class TweetNGramGenerator : INGramGenerator
    {
        public TweetIndicatorSorter sorter { get; set; }

        public TweetNGramGenerator() {
            sorter = new TweetIndicatorSorter();
        }

        //creates a single list of strings from a Dictionary doesnt preserve ItemTypes! 
        // deprecated
        private List<string> concatLists(Dictionary<string,List<string>> tokens) {
            List<string> result = new List<string>();
            object last = tokens.Last();

            foreach (KeyValuePair<string,List<string>> ol in tokens) {
                result.AddRange(ol.Value);
            }
            return result;
        }

        private List<Tuple<string,string>> concatListsWithType(List<Tuple<string, List<string>>> tokens)
        {
            List<Tuple<string, string>> result = new List<Tuple<string, string>>();
            object last = tokens.Last();
            string itemType = ""; 
            foreach (Tuple<string, List<string>> ol in tokens)
            {
                itemType = ol.Item1;
                foreach(string item in ol.Item2)
                    result.Add(Tuple.Create(itemType,item));
                //another delimiter to distinguish between type borders. Not implemnted yet.
                //if(!ol.Equals(last))result.Add(";");
            }
            return result;
        }

        private HashSet<Ngram> nGramize(List<Tuple<string,string>> tokenList, int ngramOrder)
        {
            HashSet<Ngram> ngramlist = new HashSet<Ngram>();
            string ngramString = "";

            HashSet<string> itemTypes = new HashSet<string>();
            //maxgram
            bool firstElement = true;
            foreach (Tuple<string,string> token in tokenList)
            {
                if (firstElement) {
                    itemTypes.Add(token.Item1);
                    ngramString = ngramString + token.Item2;
                    firstElement = false;
                }
                else
                {
                    if (itemTypes.Contains(token.Item1))
                    {
                        ngramString = ngramString + '.' + token.Item2;
                    }
                    else {
                        itemTypes.Add(token.Item1);
                        ngramString = ngramString + ';' + token.Item2;
                    }
                    
                }
            }
            ngramlist.Add(new Ngram(ngramString, 0, itemTypes.ToList()));

            itemTypes.Clear();

            //iterate over orders to get all orders
            if (ngramOrder > tokenList.Count)
            {
                ngramOrder = tokenList.Count;
            }
            for (int order = ngramOrder; order > 0; order--)
            {
                //set starting point
                for (int startingPoint = 0; startingPoint <= tokenList.Count() - order; startingPoint++)
                {
                    itemTypes.Clear();
                    ngramString = "";
                    firstElement = true;
                    //get all tokens
                    for (int currentToken = startingPoint; currentToken - startingPoint < order; currentToken++)
                    {
                        if (firstElement)
                        {
                            itemTypes.Add(tokenList[currentToken].Item1);
                            ngramString = ngramString + tokenList[currentToken].Item2;
                            firstElement = false;
                        }else
                        {
                            if (itemTypes.Contains(tokenList[currentToken].Item1))
                            {
                                ngramString = ngramString + "." + tokenList[currentToken].Item2;

                            }
                            else {
                                itemTypes.Add(tokenList[currentToken].Item1);
                                ngramString = ngramString + ";" + tokenList[currentToken].Item2;
                            }
                        }
                    }
                    ngramlist.Add(new Ngram(ngramString, order, itemTypes.ToList()));
                }
            }
            return ngramlist;

        } 


        

        private HashSet<Ngram> nGramize(List<string> tokenList, int  ngramOrder) {
            HashSet<Ngram> ngramlist = new HashSet<Ngram>();
            string ngramString = "";
            
            foreach (string token in tokenList)
            {
                if (token != tokenList.Last())
                {
                    ngramString = ngramString + token + '.';
                }
                else {
                    ngramString = ngramString + token;
                }  
            }
            ngramlist.Add(new Ngram(ngramString,0));
            //iterate over orders to get all orders
            if (ngramOrder > tokenList.Count) {
                ngramOrder = tokenList.Count;
            }
            for (int order = ngramOrder; order > 0; order--) {
                //set starting point
                for (int startingPoint = 0; startingPoint <= tokenList.Count()-order; startingPoint++)
                {
                    ngramString = "";
                    //get all tokens
                    for (int currentToken = startingPoint; currentToken-startingPoint < order; currentToken++) {
                        if (currentToken-startingPoint < order-1)
                        {
                            ngramString = ngramString + tokenList[currentToken] + '.';
                        }
                        else {
                            ngramString = ngramString + tokenList[currentToken];
                        }

                        
                    }
                    ngramlist.Add(new Ngram(ngramString, order));
                    
                }
            }

            return ngramlist;
      
        } 

        public List<Ngram> generateNGrams(Dictionary<string, List<string>> indicatorTokenList,int nGramOrder)
        {
            List<string> concatenated = new List<string>();
            List<Ngram> ngrams = new List<Ngram>();
            List<Tuple<string, List<string>>> workingList = new List<Tuple<string, List<string>>>();
            //sort the indictortypes 
            workingList = sorter.sortEncodedIndicatorTokens(indicatorTokenList);

            List<Tuple<string, string>> testliste = new List<Tuple<string, string>>();

            testliste = concatListsWithType(workingList);

            foreach (Tuple<string,string> tup in testliste) {
                System.Console.WriteLine(tup.Item1 + " " + tup.Item2);
            }

            ngrams = nGramize(testliste, nGramOrder).ToList();

            return ngrams;

        }





        
    }
}

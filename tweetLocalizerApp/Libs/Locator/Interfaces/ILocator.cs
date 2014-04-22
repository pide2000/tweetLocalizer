using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tweetLocalizerApp.Libs.Locator
{
    interface ILocator<T>
    {
        /*  
         *  THIS IS THE MAIN ACCESS POINT TO LEARN AND LOCATE!!!!
         *  Instantiate & configure TokenGenerators for every Indicator
         *  Instantiate Preprocessors, Orderer, Encoder and Tokenizer to configure TokenGenerators
         *  Instantiate & Configure NgramGenerator
         *  Instantiate & Configure reverseGeocoder
         *  Instantiate & Configure DatabaseAccess 
         *  1. To save knowledge 2. to search for ngrams
         *  Intantiate knowledgeObject -> it holds all the information to save to database
         */


        /**
         * learn(), locate():
         * create a locationIndicator Object for every location indicator
         * create a geoInformationObject
         **/
        List<string> nGrams {get;set;}
        void learn(T knowledgeObj);
        void locate(T knowledgeObj);
    }
}

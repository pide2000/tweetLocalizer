using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace tweetLocalizerApp.Libs.Locator
{

    /**
     * The Tokenizer should ensure to tokenize the LocationIndicator. 
     * All the preprocessing should be finished and the Item to tokenize should be in a form where a seperator indicates the split points. 
     * Example: Item='New York:NY:US' -> tokenizer -> {New York}{NY}{US} 
     * Item=12.34567:34.123566 -> tokenizer -> {12.34567}{34.123566}
     **/
    public interface ITokenizer<T>
    {
        char seperator { get; set; }
        List<T> tokenizeIndicatorItem(T indicatorItem);


    }
}

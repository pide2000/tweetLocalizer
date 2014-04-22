using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tweetLocalizerApp.Libs.Locator;

namespace tweetLocalizerApp.Libs.Locator
{
    interface ITokenGenerator<T>
    {
        List<IPreprocessor<T>> preprocessorList{get;set;}
        ITokenizer<T> tokenizer{get;set;}
        IEncoder<T> encoder { get; set; }
        ISorter<T> orderer { get; set; }
        void assemblyToken(ILocationIndicator<T> locationIndictor);
        void configure(List<IPreprocessor<T>> preprocessorList, ITokenizer<T> tokenizer, IEncoder<T> encoder, ISorter<T> orderer);
        
    }
}

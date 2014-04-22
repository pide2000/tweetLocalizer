using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tweetLocalizerApp.Libs;
using tweetLocalizerApp.Libs.Locator;

namespace tweetLocalizerApp.TweetLocator
{
    public abstract class TokenGeneratorAbstract<T> : ITokenGenerator<T>
    {
        public List<IPreprocessor<T>> preprocessorList{ get; set; } 
        public ITokenizer<T> tokenizer { get; set; }
        public IEncoder<T> encoder { get; set; }
        public ISorter<T> orderer { get; set; }
        
        abstract public void configure(List<IPreprocessor<T>> preprocessorList, ITokenizer<T> tokenizer, IEncoder<T> encoder, ISorter<T> orderer);
        abstract public void assemblyToken(ILocationIndicator<T> locationIndictor);
        
    }
}

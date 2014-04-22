using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tweetLocalizerApp.Libs.Locator
{
    public interface IEncoder<T>
    {
        List<String> encodeIndicatorTokens(List<T> locationIndicator);
    }
}

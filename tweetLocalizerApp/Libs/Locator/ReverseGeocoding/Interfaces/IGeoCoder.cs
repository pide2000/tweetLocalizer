using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tweetLocalizerApp.Libs;

namespace tweetLocalizerApp.TweetLocator.GeoCoder
{
    public interface IGeoCoder
    {
         IGeographyData locate(float longitude, float latitude);

        

    }
}

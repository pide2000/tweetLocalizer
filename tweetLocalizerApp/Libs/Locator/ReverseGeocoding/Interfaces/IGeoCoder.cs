using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tweetLocalizerApp.TweetLocator.GeoCoder
{
    interface IGeoCoder
    {
        int locate(float longitude, float latitude);

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tweetLocalizerApp.Libs;
using Microsoft.SqlServer.Server;
using tweetLocalizerApp.TweetLocator;

namespace tweetLocalizerApp
{

  

    class Program
    {
        static void Main(string[] args)
        {
            //Tweetinformation should be the database object of a tweet!!!!! TweetInfiormation class is just for testing purposes
            TweetInformation ti = new TweetInformation();
            
            ti.userlocation = "AAB, Z ,BR, Z";
            ti.timezone = "tz";

            TweetLoc tl = new TweetLoc(); 

            tl.learn(ti);

            
            

#if DEBUG
            System.Console.WriteLine("Press any key to quit !");
            System.Console.ReadLine();
#endif

        }
    }
}

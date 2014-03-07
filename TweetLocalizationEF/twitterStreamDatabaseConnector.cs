using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TweetLocalizationEF;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;

namespace TweetLocalizationEF
{
    class tweetRetweetPairLocal : Retweets
    {
    }
    class twitterStreamDatabaseConnector{

         public List<T> executeQuery<T>(string query) {
                using (var context = new TwitterStreamEntities())
                {
                    var objectContext = ((IObjectContextAdapter)context).ObjectContext;
                    var collection = objectContext.ExecuteStoreQuery<T>(query);
                    List<T> collectionList = new List<T>(collection);
                    System.Console.WriteLine(collectionList.Count() + " rows received from Database");
                    return collectionList;
                }
        
            }
    }
}



       


       

        

        

      
       


      

       
        

        
       

       
        



       

       
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tweetLocalizerApp.Libs.Locator;

namespace tweetLocalizerApp.TweetLocator
{
    public class DeleteSigns : PreprocessorAbstract<string>
    {
        public override string preprocess(string indicatorItem)
        {
            String words = System.Text.RegularExpressions.Regex.Replace(indicatorItem, @"[^\p{L}\p{N} ]", " ");
            words = System.Text.RegularExpressions.Regex.Replace(words, @"\s+", " ").Trim();
            
            return words;
        }
    }

    class CheckGeolocation : PreprocessorAbstract<string>
    {
        public override string preprocess(string indicatorItem)
        {
            string[] split = Array.ConvertAll(indicatorItem.Split(new Char[] { ' ' }), p => p.Trim());
            
            List<string> result = new List<string>();
            string resultString = "";
            string parentId = "/";
            string lastParent = "/";
            int count = 0;
            using (GeonamesDataEntities geonamesDB = new GeonamesDataEntities())
                {

                if (split.Length == 1) {
                    resultString = split[0];
                }
                else if (split.Length == 2) { 
                    //check first element
                    parentId = geonamesDB.getNodeId(split[0], parentId).FirstOrDefault();
                    if(parentId!=null){
                        //check second element
                        parentId = geonamesDB.getNodeId(split[1], parentId).FirstOrDefault();
                        if(parentId!=null && geonamesDB.checkNodeId(parentId)!=null){
                            resultString = string.Join(" ",split);
                        }else{
                            resultString = string.Join(":",split);
                        }
                    }else{
                    resultString = string.Join(":",split);
                    }
                
                //split.length bigger than two
                }else{
                        for (int startingPoint = 0; startingPoint < split.Length; startingPoint++)
                        {
                            
                            List<string> temporaryGeoname = new List<string>();
                            
                            for (int arrayiter = startingPoint; arrayiter < split.Length; arrayiter++)
                            {
                                lastParent = parentId;
                                parentId = geonamesDB.getNodeId(split[arrayiter],parentId).FirstOrDefault();
                                //first elemtn isnt in the tree on level 1
                                
                                if(parentId == null && temporaryGeoname.Count == 0){
                                    result.Add(split[arrayiter]);
                                    parentId = "/";
                                    
                                    break;
                                // geoname with more than one token found, add it as new token to result
                                }else if(parentId == null && temporaryGeoname.Count > 0){
                                    
                                    startingPoint = arrayiter-1;
                                    if (null != geonamesDB.checkNodeId(lastParent).FirstOrDefault())
                                    {
                                        result.Add(string.Join(" ", temporaryGeoname));
                                    }
                                    else {
                                        result.AddRange(temporaryGeoname);
                                    }
                                    
                                    temporaryGeoname.Clear();
                                    parentId = "/";
                                    break;
                                }else{
                                    temporaryGeoname.Add(split[arrayiter]);
                                    if(arrayiter == split.Length-1){
                                        startingPoint = arrayiter;
                                    }
                                }
                            }
                            if (temporaryGeoname.Count > 0) {
                                if (null != geonamesDB.checkNodeId(lastParent).FirstOrDefault())
                                {
                                    result.Add(string.Join(" ", temporaryGeoname));
                                }
                                else
                                {
                                    result.AddRange(temporaryGeoname);
                                }
                            }
                        
                        
                        }

                        resultString = string.Join<string>(":", result);
                }
            }
            
            return resultString;
        }
    }

    class ToLowerCase : PreprocessorAbstract<string>
    {
        public override string preprocess(string indicatorItem)
        {
            return indicatorItem.ToLower();

        }

    }


   

    
}

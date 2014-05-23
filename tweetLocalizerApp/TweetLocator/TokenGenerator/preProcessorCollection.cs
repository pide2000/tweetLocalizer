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
            string parentId = "/";
            string lastParent = "/";
            int count = 0;
            if (split.Length > 1)
            {
                using (GeonamesDataEntities geonamesDB = new GeonamesDataEntities())
                {
                    for (int startingPoint = 0; startingPoint < split.Length - 1; startingPoint++)
                    {
                        List<string> temporaryGeoname = new List<string>();

                        for (int arrayiter = startingPoint; arrayiter < split.Length; arrayiter++)
                        {
                            lastParent = parentId;
                            parentId = geonamesDB.getNodeId(split[arrayiter], parentId).FirstOrDefault();
                            if (parentId == null)
                            {
                                if (temporaryGeoname.Count == 0)
                                {
                                    result.Add(split[arrayiter]);
                                }
                                parentId = "/";
                                count = arrayiter;
                                break;
                            }
                            else
                            {
                                temporaryGeoname.Add(split[arrayiter]);
                            }
                        }

                        if (temporaryGeoname.Count > 0)
                        {
                            int? geonamesId = geonamesDB.checkNodeId(lastParent).FirstOrDefault();

                            if (geonamesId != null)
                            {
                                string[] tempgeoname = temporaryGeoname.ToArray();
                                result.Add(string.Join(" ", tempgeoname));
                            }
                            else
                            {
                                result.AddRange(temporaryGeoname);
                            }
                        }


                        if (split.Length == 2)
                        {
                            result.Add(split[split.Length - 1]);
                            break;
                        }
                        else if (temporaryGeoname.Count == 0 && count == split.Length-1)
                        {
                            result.Add(split[split.Length - 1]);
                        }
                        else if (temporaryGeoname.Count == 0){
                           
                        }
                        else if (count < split.Length - 1)
                        {
                            startingPoint = count - 1;
                        }
                        else if (count == split.Length - 1)
                        {
                            result.Add(split[split.Length - 1]);
                            break;
                        }


                    }
                }
            }
            else {
                result = split.ToList();
            }
            //check every combination of Tokens against geonames database. search single tokens in normal base and 2-, 3- tokens in the tree
            string resultString = string.Join<string>(":", result);
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

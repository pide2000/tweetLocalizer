using System.Collections.Generic;
using TwitterWebFrontend.Models;

namespace TwitterWebFrontend.ViewModels
{
    public class manualAssignementViewModel
    {
        public IEnumerable<GeoNamesRestricted> geoNamesRestricted { get; set;}
        public IEnumerable<get_tweetRandomSample2_notProcessed_Result> tweetRandomSample2 { get; set;}
        public manualAssign_tweetRandomSample2_to_geonamesRestricted manualAssign { get; set; }
        
        public IEnumerable<contains_search_alternateNames3Join_Result> containsSearchAlternateNames3 { get; set; }
        public IEnumerable<contains_search_geoNamesRestricted_Result> containsSearchGeoNamesRestricted { get; set; }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TweetLocalizationEF
{
    using System;
    
    public partial class getTweetsWhereLocationLike_Result
    {
        public int id { get; set; }
        public long tweetid { get; set; }
        public string username { get; set; }
        public double lon { get; set; }
        public double lat { get; set; }
        public string hashtag { get; set; }
        public System.DateTime time { get; set; }
        public string status { get; set; }
        public System.DateTime tweetTime { get; set; }
        public long skipped { get; set; }
        public string place { get; set; }
        public bool geotagged { get; set; }
        public System.Data.Entity.Spatial.DbGeography coord { get; set; }
        public Nullable<bool> processed { get; set; }
        public string country_code { get; set; }
    }
}

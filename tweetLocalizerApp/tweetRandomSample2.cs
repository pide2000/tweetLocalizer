//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace tweetLocalizerApp
{
    using System;
    using System.Collections.Generic;
    
    public partial class tweetRandomSample2
    {
        public tweetRandomSample2()
        {
            this.knowledgeBaseGeocoding = new HashSet<knowledgeBaseGeocoding>();
        }
    
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
        public string userlocation { get; set; }
        public string lang { get; set; }
        public int utc_offset { get; set; }
        public string timezone { get; set; }
        public System.Data.Entity.Spatial.DbGeography coord { get; set; }
        public Nullable<bool> processed { get; set; }
        public Nullable<int> geoNames_geoNamesId { get; set; }
    
        public virtual ICollection<knowledgeBaseGeocoding> knowledgeBaseGeocoding { get; set; }
    }
}

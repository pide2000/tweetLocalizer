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
    using System.Collections.Generic;
    
    public partial class geonamesid_geotreenodeidstring : IEquatable<geonamesid_geotreenodeidstring>
    {
        public string nodeId { get; set; }
        public int GeoNames_geonamesid { get; set; }

        public bool Equals(geonamesid_geotreenodeidstring other) {
            if (this.nodeId.Equals(other.nodeId) && this.GeoNames_geonamesid == other.GeoNames_geonamesid) {
                return true;
            }
            else
            {
                return false;

            }
        
        }

    }
}

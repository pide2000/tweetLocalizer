//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TwitterWebFrontend.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class timeZones
    {
        public timeZones()
        {
            this.GeoNames = new HashSet<GeoNames>();
        }
    
        public int ID { get; set; }
        public string country_code { get; set; }
        public string time_zone_id { get; set; }
        public Nullable<double> GMT_offset { get; set; }
        public Nullable<double> DST_offset { get; set; }
        public Nullable<double> rawOffset { get; set; }
    
        public virtual ICollection<GeoNames> GeoNames { get; set; }
    }
}

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
    
    public partial class admin1Codes
    {
        public string ID { get; set; }
        public string name { get; set; }
        public string asciiname { get; set; }
        public Nullable<int> geonamesId { get; set; }
    
        public virtual GeoNames GeoNames { get; set; }
    }
}

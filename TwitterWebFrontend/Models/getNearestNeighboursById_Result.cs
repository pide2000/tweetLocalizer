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
    
    public partial class getNearestNeighboursById_Result
    {
        public Nullable<int> geonameID { get; set; }
        public string name { get; set; }
        public string asciiname { get; set; }
        public Nullable<double> latitude { get; set; }
        public Nullable<double> longitude { get; set; }
        public string feature_class { get; set; }
        public string feature_code { get; set; }
        public string country_code { get; set; }
        public string cc2 { get; set; }
        public string admin1_code { get; set; }
        public string admin2_code { get; set; }
        public string admin3_code { get; set; }
        public string admin4_code { get; set; }
        public Nullable<long> pop { get; set; }
        public Nullable<int> elevation { get; set; }
        public Nullable<int> gtopo30 { get; set; }
        public System.Data.Entity.Spatial.DbGeography geog { get; set; }
    }
}

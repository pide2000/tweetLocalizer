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
    
    public partial class countryinfo
    {
        public string iso_alpha2 { get; set; }
        public string iso_alpha3 { get; set; }
        public Nullable<int> iso_numeric { get; set; }
        public string fips_code { get; set; }
        public string name { get; set; }
        public string capital { get; set; }
        public Nullable<double> areainsqkm { get; set; }
        public Nullable<long> population { get; set; }
        public string continent { get; set; }
        public string tld { get; set; }
        public string currencyCode { get; set; }
        public string currencyName { get; set; }
        public string phone { get; set; }
        public string postalCodeFormat { get; set; }
        public string postalCodeRegex { get; set; }
        public string languages { get; set; }
        public Nullable<int> geonameId { get; set; }
        public string neighbours { get; set; }
        public string equivalentFipsCode { get; set; }
    
        public virtual GeoNamesRestricted GeoNamesRestricted { get; set; }
        public virtual GeoNames GeoNames { get; set; }
    }
}

﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class GeonamesDataEntities1 : DbContext
    {
        public GeonamesDataEntities1()
            : base("name=GeonamesDataEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<alternateNames3> alternateNames3 { get; set; }
        public virtual DbSet<countryCodes> countryCodes { get; set; }
        public virtual DbSet<countryinfo> countryinfo { get; set; }
        public virtual DbSet<GeoNames> GeoNames { get; set; }
        public virtual DbSet<GeoNamesRestricted> GeoNamesRestricted { get; set; }
        public virtual DbSet<manualAssign_tweetRandomSample2_to_geonamesRestricted> manualAssign_tweetRandomSample2_to_geonamesRestricted { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<timeZones> timeZones { get; set; }
        public virtual DbSet<tweetRandomSample> tweetRandomSample { get; set; }
        public virtual DbSet<tweetRandomSample2> tweetRandomSample2 { get; set; }
        public virtual DbSet<geonamesid_geotreenodeidstring> geonamesid_geotreenodeidstring { get; set; }
        public virtual DbSet<View_1> View_1 { get; set; }
        public virtual DbSet<gmapsGeocoding> gmapsGeocoding { get; set; }
        public virtual DbSet<GeoNames_trs2_gmapsCoding> GeoNames_trs2_gmapsCoding { get; set; }
    
        [DbFunction("GeonamesDataEntities1", "contains_search_alternateNames3Join")]
        public virtual IQueryable<contains_search_alternateNames3Join_Result> contains_search_alternateNames3Join(string searchString)
        {
            var searchStringParameter = searchString != null ?
                new ObjectParameter("searchString", searchString) :
                new ObjectParameter("searchString", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<contains_search_alternateNames3Join_Result>("[GeonamesDataEntities1].[contains_search_alternateNames3Join](@searchString)", searchStringParameter);
        }
    
        [DbFunction("GeonamesDataEntities1", "contains_search_geoNamesRestricted")]
        public virtual IQueryable<contains_search_geoNamesRestricted_Result> contains_search_geoNamesRestricted(string searchString)
        {
            var searchStringParameter = searchString != null ?
                new ObjectParameter("searchString", searchString) :
                new ObjectParameter("searchString", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<contains_search_geoNamesRestricted_Result>("[GeonamesDataEntities1].[contains_search_geoNamesRestricted](@searchString)", searchStringParameter);
        }
    
        [DbFunction("GeonamesDataEntities1", "contains_search_GeoNamesRestricted_name")]
        public virtual IQueryable<contains_search_GeoNamesRestricted_name_Result> contains_search_GeoNamesRestricted_name(string searchWord)
        {
            var searchWordParameter = searchWord != null ?
                new ObjectParameter("searchWord", searchWord) :
                new ObjectParameter("searchWord", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<contains_search_GeoNamesRestricted_name_Result>("[GeonamesDataEntities1].[contains_search_GeoNamesRestricted_name](@searchWord)", searchWordParameter);
        }
    
        [DbFunction("GeonamesDataEntities1", "freetext_search_alternateNames_alternateName")]
        public virtual IQueryable<freetext_search_alternateNames_alternateName_Result> freetext_search_alternateNames_alternateName(string searchWord)
        {
            var searchWordParameter = searchWord != null ?
                new ObjectParameter("searchWord", searchWord) :
                new ObjectParameter("searchWord", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<freetext_search_alternateNames_alternateName_Result>("[GeonamesDataEntities1].[freetext_search_alternateNames_alternateName](@searchWord)", searchWordParameter);
        }
    
        [DbFunction("GeonamesDataEntities1", "freetext_search_GeoNamesRestricted_name")]
        public virtual IQueryable<freetext_search_GeoNamesRestricted_name_Result> freetext_search_GeoNamesRestricted_name(string searchWord)
        {
            var searchWordParameter = searchWord != null ?
                new ObjectParameter("searchWord", searchWord) :
                new ObjectParameter("searchWord", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<freetext_search_GeoNamesRestricted_name_Result>("[GeonamesDataEntities1].[freetext_search_GeoNamesRestricted_name](@searchWord)", searchWordParameter);
        }
    
        [DbFunction("GeonamesDataEntities1", "fulltext_search_GeoNames_name")]
        public virtual IQueryable<fulltext_search_GeoNames_name_Result> fulltext_search_GeoNames_name(string searchWord)
        {
            var searchWordParameter = searchWord != null ?
                new ObjectParameter("searchWord", searchWord) :
                new ObjectParameter("searchWord", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<fulltext_search_GeoNames_name_Result>("[GeonamesDataEntities1].[fulltext_search_GeoNames_name](@searchWord)", searchWordParameter);
        }
    
        [DbFunction("GeonamesDataEntities1", "fulltext_search_GeoNamesRestricted_name")]
        public virtual IQueryable<fulltext_search_GeoNamesRestricted_name_Result> fulltext_search_GeoNamesRestricted_name(string searchWord)
        {
            var searchWordParameter = searchWord != null ?
                new ObjectParameter("searchWord", searchWord) :
                new ObjectParameter("searchWord", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<fulltext_search_GeoNamesRestricted_name_Result>("[GeonamesDataEntities1].[fulltext_search_GeoNamesRestricted_name](@searchWord)", searchWordParameter);
        }
    
        [DbFunction("GeonamesDataEntities1", "get_tweetRandomSample2_notProcessed")]
        public virtual IQueryable<get_tweetRandomSample2_notProcessed_Result> get_tweetRandomSample2_notProcessed()
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<get_tweetRandomSample2_notProcessed_Result>("[GeonamesDataEntities1].[get_tweetRandomSample2_notProcessed]()");
        }
    
        [DbFunction("GeonamesDataEntities1", "getCountryOfNearestPointById")]
        public virtual IQueryable<getCountryOfNearestPointById_Result> getCountryOfNearestPointById(Nullable<int> tweets_id)
        {
            var tweets_idParameter = tweets_id.HasValue ?
                new ObjectParameter("tweets_id", tweets_id) :
                new ObjectParameter("tweets_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<getCountryOfNearestPointById_Result>("[GeonamesDataEntities1].[getCountryOfNearestPointById](@tweets_id)", tweets_idParameter);
        }
    
        [DbFunction("GeonamesDataEntities1", "getNearestNeighboursByCoordinates")]
        public virtual IQueryable<getNearestNeighboursByCoordinates_Result> getNearestNeighboursByCoordinates(Nullable<double> lon, Nullable<double> lat, Nullable<int> nearestNeighboursCount)
        {
            var lonParameter = lon.HasValue ?
                new ObjectParameter("lon", lon) :
                new ObjectParameter("lon", typeof(double));
    
            var latParameter = lat.HasValue ?
                new ObjectParameter("lat", lat) :
                new ObjectParameter("lat", typeof(double));
    
            var nearestNeighboursCountParameter = nearestNeighboursCount.HasValue ?
                new ObjectParameter("nearestNeighboursCount", nearestNeighboursCount) :
                new ObjectParameter("nearestNeighboursCount", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<getNearestNeighboursByCoordinates_Result>("[GeonamesDataEntities1].[getNearestNeighboursByCoordinates](@lon, @lat, @nearestNeighboursCount)", lonParameter, latParameter, nearestNeighboursCountParameter);
        }
    
        [DbFunction("GeonamesDataEntities1", "getNearestNeighboursById")]
        public virtual IQueryable<getNearestNeighboursById_Result> getNearestNeighboursById(Nullable<int> tweets_id, Nullable<int> nearestNeighboursCount)
        {
            var tweets_idParameter = tweets_id.HasValue ?
                new ObjectParameter("tweets_id", tweets_id) :
                new ObjectParameter("tweets_id", typeof(int));
    
            var nearestNeighboursCountParameter = nearestNeighboursCount.HasValue ?
                new ObjectParameter("nearestNeighboursCount", nearestNeighboursCount) :
                new ObjectParameter("nearestNeighboursCount", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<getNearestNeighboursById_Result>("[GeonamesDataEntities1].[getNearestNeighboursById](@tweets_id, @nearestNeighboursCount)", tweets_idParameter, nearestNeighboursCountParameter);
        }
    
        [DbFunction("GeonamesDataEntities1", "getNodeIdString")]
        public virtual IQueryable<getNodeIdString_Result> getNodeIdString(string parentId, string tokenText)
        {
            var parentIdParameter = parentId != null ?
                new ObjectParameter("parentId", parentId) :
                new ObjectParameter("parentId", typeof(string));
    
            var tokenTextParameter = tokenText != null ?
                new ObjectParameter("tokenText", tokenText) :
                new ObjectParameter("tokenText", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<getNodeIdString_Result>("[GeonamesDataEntities1].[getNodeIdString](@parentId, @tokenText)", parentIdParameter, tokenTextParameter);
        }
    
        [DbFunction("GeonamesDataEntities1", "getTweetsWhereLocationLike")]
        public virtual IQueryable<getTweetsWhereLocationLike_Result> getTweetsWhereLocationLike(string character)
        {
            var characterParameter = character != null ?
                new ObjectParameter("character", character) :
                new ObjectParameter("character", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<getTweetsWhereLocationLike_Result>("[GeonamesDataEntities1].[getTweetsWhereLocationLike](@character)", characterParameter);
        }
    
        [DbFunction("GeonamesDataEntities1", "giveNearestGeographicPoint")]
        public virtual IQueryable<giveNearestGeographicPoint_Result> giveNearestGeographicPoint(System.Data.Entity.Spatial.DbGeography geodata)
        {
            var geodataParameter = geodata != null ?
                new ObjectParameter("geodata", geodata) :
                new ObjectParameter("geodata", typeof(System.Data.Entity.Spatial.DbGeography));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<giveNearestGeographicPoint_Result>("[GeonamesDataEntities1].[giveNearestGeographicPoint](@geodata)", geodataParameter);
        }
    
        [DbFunction("GeonamesDataEntities1", "like_search_alternateNames_name_allFeature")]
        public virtual IQueryable<like_search_alternateNames_name_allFeature_Result> like_search_alternateNames_name_allFeature(string searchString)
        {
            var searchStringParameter = searchString != null ?
                new ObjectParameter("searchString", searchString) :
                new ObjectParameter("searchString", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<like_search_alternateNames_name_allFeature_Result>("[GeonamesDataEntities1].[like_search_alternateNames_name_allFeature](@searchString)", searchStringParameter);
        }
    
        [DbFunction("GeonamesDataEntities1", "like_search_GeoNamesRestricted_name")]
        public virtual IQueryable<like_search_GeoNamesRestricted_name_Result> like_search_GeoNamesRestricted_name(string searchString, string restriction)
        {
            var searchStringParameter = searchString != null ?
                new ObjectParameter("searchString", searchString) :
                new ObjectParameter("searchString", typeof(string));
    
            var restrictionParameter = restriction != null ?
                new ObjectParameter("restriction", restriction) :
                new ObjectParameter("restriction", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<like_search_GeoNamesRestricted_name_Result>("[GeonamesDataEntities1].[like_search_GeoNamesRestricted_name](@searchString, @restriction)", searchStringParameter, restrictionParameter);
        }
    
        [DbFunction("GeonamesDataEntities1", "like_search_GeoNamesRestricted_name_ALL")]
        public virtual IQueryable<like_search_GeoNamesRestricted_name_ALL_Result> like_search_GeoNamesRestricted_name_ALL(string searchString)
        {
            var searchStringParameter = searchString != null ?
                new ObjectParameter("searchString", searchString) :
                new ObjectParameter("searchString", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<like_search_GeoNamesRestricted_name_ALL_Result>("[GeonamesDataEntities1].[like_search_GeoNamesRestricted_name_ALL](@searchString)", searchStringParameter);
        }
    
        [DbFunction("GeonamesDataEntities1", "like_search_GeoNamesRestricted_name_allFeature")]
        public virtual IQueryable<like_search_GeoNamesRestricted_name_allFeature_Result> like_search_GeoNamesRestricted_name_allFeature(string searchString)
        {
            var searchStringParameter = searchString != null ?
                new ObjectParameter("searchString", searchString) :
                new ObjectParameter("searchString", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<like_search_GeoNamesRestricted_name_allFeature_Result>("[GeonamesDataEntities1].[like_search_GeoNamesRestricted_name_allFeature](@searchString)", searchStringParameter);
        }
    
        public virtual int addNodeToGeonames(Nullable<int> geonamesid, string nodeId)
        {
            var geonamesidParameter = geonamesid.HasValue ?
                new ObjectParameter("geonamesid", geonamesid) :
                new ObjectParameter("geonamesid", typeof(int));
    
            var nodeIdParameter = nodeId != null ?
                new ObjectParameter("nodeId", nodeId) :
                new ObjectParameter("nodeId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("addNodeToGeonames", geonamesidParameter, nodeIdParameter);
        }
    
        public virtual int insertNode_TokenTreeGeoNames(string tokenText, string parentId)
        {
            var tokenTextParameter = tokenText != null ?
                new ObjectParameter("tokenText", tokenText) :
                new ObjectParameter("tokenText", typeof(string));
    
            var parentIdParameter = parentId != null ?
                new ObjectParameter("parentId", parentId) :
                new ObjectParameter("parentId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("insertNode_TokenTreeGeoNames", tokenTextParameter, parentIdParameter);
        }
    
        public virtual int sp_alterdiagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_alterdiagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_creatediagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_creatediagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_dropdiagram(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_dropdiagram", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagramdefinition_Result> sp_helpdiagramdefinition(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagramdefinition_Result>("sp_helpdiagramdefinition", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagrams_Result> sp_helpdiagrams(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagrams_Result>("sp_helpdiagrams", diagramnameParameter, owner_idParameter);
        }
    
        public virtual int sp_renamediagram(string diagramname, Nullable<int> owner_id, string new_diagramname)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var new_diagramnameParameter = new_diagramname != null ?
                new ObjectParameter("new_diagramname", new_diagramname) :
                new ObjectParameter("new_diagramname", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_renamediagram", diagramnameParameter, owner_idParameter, new_diagramnameParameter);
        }
    
        public virtual int sp_upgraddiagrams()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_upgraddiagrams");
        }
    
        [DbFunction("GeonamesDataEntities1", "getNearestNeighbourGeoNameId")]
        public virtual IQueryable<Nullable<int>> getNearestNeighbourGeoNameId(System.Data.Entity.Spatial.DbGeography geodata)
        {
            var geodataParameter = geodata != null ?
                new ObjectParameter("geodata", geodata) :
                new ObjectParameter("geodata", typeof(System.Data.Entity.Spatial.DbGeography));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<Nullable<int>>("[GeonamesDataEntities1].[getNearestNeighbourGeoNameId](@geodata)", geodataParameter);
        }
    
        public virtual int InsertGmapsGeocodingCountryByLatLon(Nullable<int> id, string country, string resultStat)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("id", id) :
                new ObjectParameter("id", typeof(int));
    
            var countryParameter = country != null ?
                new ObjectParameter("country", country) :
                new ObjectParameter("country", typeof(string));
    
            var resultStatParameter = resultStat != null ?
                new ObjectParameter("resultStat", resultStat) :
                new ObjectParameter("resultStat", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("InsertGmapsGeocodingCountryByLatLon", idParameter, countryParameter, resultStatParameter);
        }
    
        public virtual int updateTweetRandomSample1(Nullable<int> id, Nullable<int> geonameid)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("id", id) :
                new ObjectParameter("id", typeof(int));
    
            var geonameidParameter = geonameid.HasValue ?
                new ObjectParameter("geonameid", geonameid) :
                new ObjectParameter("geonameid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("updateTweetRandomSample1", idParameter, geonameidParameter);
        }
    }
}

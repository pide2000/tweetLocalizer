﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TweetsDataEntities : DbContext
    {
        public TweetsDataEntities()
            : base("name=TweetsDataEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tweetRandomSample> tweetRandomSample { get; set; }
        public virtual DbSet<tweetRandomSample2> tweetRandomSample2 { get; set; }
        public virtual DbSet<learningBase> learningBase { get; set; }
    }
}

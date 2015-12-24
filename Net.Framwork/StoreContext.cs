﻿using Net.Framework.StoreModel;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Net.Framework
{
    public class StoreContext : DbContext
    {
        //static StoreContext()
        //{
        //    Database.SetInitializer<StoreContext>(null);
        //}

        //public StoreContext()
        //    : base("Name=StoreContext")
        //{
        //}

        public StoreContext()
            : base(ConfigurationManager.ConnectionStrings["StoreContext"].ConnectionString)
        {
            //Database.SetInitializer<StoreContext>(null);
        }

        public DbSet<MemberT> MemberT { get; set; }
        public DbSet<DetailT> DetailT { get; set; }
        public DbSet<StorePrinterT> StorePrinterT { get; set; }
        public DbSet<StorePrintingCompanyPrinterT> StorePrintingCompanyPrinterT { get; set; }
        public DbSet<StorePrintingCompanyT> StorePrintingCompanyT { get; set; }
        public DbSet<StorePrintingTypeT> StorePrintingTypeT { get; set; }
        public DbSet<StoreProductT> StoreProductT { get; set; }
        public DbSet<StoreProductDetailT> StoreProductDetailT { get; set; }
        public DbSet<StoreProductUpdateHistoryT> StoreProductUpdateHistoryT { get; set; }
        public DbSet<StoreReviewT> StoreReviewT { get; set; }
        public DbSet<StoreLikesT> StoreLikesT { get; set; }
        public DbSet<StoreMaterialT> StoreMaterialT { get; set; }
        public DbSet<StoreMemberT> StoreMemberT { get; set; }
        public DbSet<StoreNoticeT> StoreNoticeT { get; set; }
        public DbSet<StoreCategoryT> StoreCategoryT { get; set; }
				public DbSet<MemberMsgT> MemberMsgT { get; set; }
        
        //public DbSet<DetailT> DetailT { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<IncludeMetadataConvention>();

            //modelBuilder.Entity<MemberT>().Map(bp =>
            //{
            //    bp.Properties(
            //        p => new
            //        {
            //            p.MemberId,
            //            p.MemberNm,
            //            p.AppId,
            //            p.RegDt
            //        });
            //    bp.ToTable("Member");
            //});

            //modelBuilder.Entity<MemberT>().Map(bp =>
            //{
            //    bp.Properties(
            //        p => new
            //        {
            //            p.PhoneNumber
            //        });
            //    bp.ToTable("Detail");
            //}).Property(e => e.PhoneNumber).HasColumnName("Phone_Number");


        }
    }
}
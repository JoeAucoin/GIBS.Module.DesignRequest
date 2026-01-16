using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Oqtane.Modules;
using Oqtane.Repository;
using Oqtane.Infrastructure;
using Oqtane.Repository.Databases.Interfaces;
using GIBS.Module.DesignRequest.Models;

namespace GIBS.Module.DesignRequest.Repository
{
    public class DesignRequestContext : DBContextBase, ITransientService, IMultiDatabase
    {
        public virtual DbSet<Models.DesignRequest> DesignRequest { get; set; }
        public virtual DbSet<Models.Detail> Detail { get; set; }
        public virtual DbSet<Models.DetailToRequest> DetailToRequest { get; set; }
        public virtual DbSet<Models.Appliance> Appliance { get; set; }
        public virtual DbSet<Models.ApplianceToRequest> ApplianceToRequest { get; set; }
        public virtual DbSet<Models.FileToRequest> FileToRequest { get; set; }
        public virtual DbSet<Models.NoteToRequest> NoteToRequest { get; set; }
        public virtual DbSet<Models.NotificationToRequest> NotificationToRequest { get; set; }

        // Start new DbSets
        public virtual DbSet<Models.UserCredit> UserCredit { get; set; }
        public virtual DbSet<Models.CreditPackage> CreditPackage { get; set; }
        public virtual DbSet<Models.CreditTransaction> CreditTransaction { get; set; }
        public virtual DbSet<Models.PaymentRecord> PaymentRecord { get; set; }
        // End new DbSets

        public DesignRequestContext(IDBContextDependencies DBContextDependencies) : base(DBContextDependencies)
        {
            // ContextBase handles multi-tenant database connections
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Models.DesignRequest>().ToTable(ActiveDatabase.RewriteName("GIBSDesignRequest"));
            builder.Entity<Models.Detail>().ToTable(ActiveDatabase.RewriteName("GIBSDesignRequest_Detail"));
            builder.Entity<Models.DetailToRequest>().ToTable(ActiveDatabase.RewriteName("GIBSDesignRequest_DetailToRequest"));
            builder.Entity<Models.Appliance>().ToTable(ActiveDatabase.RewriteName("GIBSDesignRequest_Appliance"));
            builder.Entity<Models.ApplianceToRequest>().ToTable(ActiveDatabase.RewriteName("GIBSDesignRequest_ApplianceToRequest"));
            builder.Entity<Models.FileToRequest>().ToTable(ActiveDatabase.RewriteName("GIBSDesignRequest_FileToRequest"));
            builder.Entity<Models.NoteToRequest>().ToTable(ActiveDatabase.RewriteName("GIBSDesignRequest_NoteToRequest"));
            builder.Entity<Models.NotificationToRequest>().ToTable(ActiveDatabase.RewriteName("GIBSDesignRequest_NotificationToRequest"));

            // Start new Table Mappings
            builder.Entity<Models.UserCredit>().ToTable(ActiveDatabase.RewriteName("GIBSDesignRequest_UserCredit"));
            builder.Entity<Models.CreditPackage>().ToTable(ActiveDatabase.RewriteName("GIBSDesignRequest_CreditPackage"));
            builder.Entity<Models.CreditTransaction>().ToTable(ActiveDatabase.RewriteName("GIBSDesignRequest_CreditTransaction"));
            builder.Entity<Models.PaymentRecord>().ToTable(ActiveDatabase.RewriteName("GIBSDesignRequest_PaymentRecord"));
            // End new Table Mappings

            //NotificationToRequest -> DesignRequest (many-to-one)
            builder.Entity<Models.NotificationToRequest>()
                .HasOne(n => n.DesignRequest)
                .WithMany(d => d.Notifications)
                .HasForeignKey(n => n.DesignRequestId)
                .OnDelete(DeleteBehavior.Cascade);

            // NoteToRequest -> DesignRequest (many-to-one)
            builder.Entity<Models.NoteToRequest>()
                .HasOne(n => n.DesignRequest)
                .WithMany(d => d.Notes)
                .HasForeignKey(n => n.DesignRequestId)
                .OnDelete(DeleteBehavior.Cascade);
            //ApplianceToRequest -> DesignRequest (many-to-one)
            builder.Entity<Models.ApplianceToRequest>()
                .HasOne(a => a.DesignRequest)
                .WithMany(d => d.Appliances)
                .HasForeignKey(a => a.DesignRequestId)
                .OnDelete(DeleteBehavior.Cascade);
            // DetailToRequest -> DesignRequest (many-to-one)
            builder.Entity<Models.DetailToRequest>()
                .HasOne(d => d.DesignRequest)
                .WithMany(dr => dr.Details)
                .HasForeignKey(d => d.DesignRequestId)
                .OnDelete(DeleteBehavior.Cascade);
            // FileToRequest -> DesignRequest (many-to-one)
            builder.Entity<Models.FileToRequest>()
                .HasOne(f => f.DesignRequest)
                .WithMany(dr => dr.Files)
                .HasForeignKey(f => f.DesignRequestId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
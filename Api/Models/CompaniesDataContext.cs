using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Api.Models
{
    public partial class CompaniesDataContext : DbContext
    {
        public CompaniesDataContext()
        {
        }

        public CompaniesDataContext(DbContextOptions<CompaniesDataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Company> Companies { get; set; } = null!;
        public virtual DbSet<LeadManager> LeadManagers { get; set; } = null!;
        public virtual DbSet<SeniorManager> SeniorManagers { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(e => e.CompanyCode)
                    .HasName("PK__company__F4E508EB9377C61C");

                entity.ToTable("company");

                entity.Property(e => e.CompanyCode).HasColumnName("company_code");

                entity.Property(e => e.Founder)
                    .HasMaxLength(50)
                    .HasColumnName("founder");
            });

            modelBuilder.Entity<LeadManager>(entity =>
            {
                entity.HasKey(e => e.LeadManagerCode)
                    .HasName("PK__Lead_Man__61168D49896B54CB");

                entity.ToTable("Lead_Manager");

                entity.Property(e => e.LeadManagerCode).HasColumnName("lead_manager_code");

                entity.Property(e => e.CompanyCode).HasColumnName("company_code");

                entity.HasOne(d => d.CompanyCodeNavigation)
                    .WithMany(p => p.LeadManagers)
                    .HasForeignKey(d => d.CompanyCode)
                    .HasConstraintName("fk_company_code");
            });

            modelBuilder.Entity<SeniorManager>(entity =>
            {
                entity.HasKey(e => e.SeniorManagerCode)
                    .HasName("PK__Senior_M__AF50F6373D9187EA");

                entity.ToTable("Senior_Manager");

                entity.Property(e => e.SeniorManagerCode).HasColumnName("senior_manager_code");

                entity.Property(e => e.LeadManagerCode).HasColumnName("lead_manager_code");

                entity.HasOne(d => d.LeadManagerCodeNavigation)
                    .WithMany(p => p.SeniorManagers)
                    .HasForeignKey(d => d.LeadManagerCode)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_lead_manager_code");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

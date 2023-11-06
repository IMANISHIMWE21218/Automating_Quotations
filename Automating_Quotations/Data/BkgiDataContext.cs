using System;
using System.Collections.Generic;
using Automating_Quotations.Models.Travel;
using Microsoft.EntityFrameworkCore;

namespace Automating_Quotations.Data;

public partial class BkgiDataContext : DbContext
{
    public BkgiDataContext()
    {
    }

    public BkgiDataContext(DbContextOptions<BkgiDataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CoverageArea> CoverageAreas { get; set; }

    public virtual DbSet<CoveragePeriod> CoveragePeriods { get; set; }

    public virtual DbSet<PremiumDetail> PremiumDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DataConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CoverageArea>(entity =>
        {
            entity.HasKey(e => e.CoverageAreaId).HasName("PK__Coverage__049B6DA0AC9DB24C");
        });

        modelBuilder.Entity<CoveragePeriod>(entity =>
        {
            entity.HasKey(e => e.CoveragePeriodId).HasName("PK__Coverage__20EC768AFB2E6F26");

            entity.HasOne(d => d.CoverageArea).WithMany(p => p.CoveragePeriods).HasConstraintName("FK__CoverageP__Cover__398D8EEE");
        });

        modelBuilder.Entity<PremiumDetail>(entity =>
        {
            entity.HasKey(e => e.PremiumId).HasName("PK__PremiumD__86B646E5C31B0BA8");

            entity.HasOne(d => d.CoveragePeriod).WithMany(p => p.PremiumDetails).HasConstraintName("FK__PremiumDe__Cover__3C69FB99");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

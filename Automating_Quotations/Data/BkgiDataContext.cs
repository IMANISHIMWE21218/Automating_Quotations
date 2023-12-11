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

    public virtual DbSet<TravelCoverperiod> TravelCoverperiods { get; set; }

    public virtual DbSet<TravelRate> TravelRates { get; set; }

    public virtual DbSet<TravelRegion> TravelRegions { get; set; }

    public virtual DbSet<TravelInsuranceService> TravelInsuranceServices { get; set; }
    public object TravelCoverperiod { get; internal set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DataConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TravelCoverperiod>(entity =>
        {
            entity.HasKey(e => e.Cpid).HasName("PK_CPID");
        });

        modelBuilder.Entity<TravelRate>(entity =>
        {
            entity.HasKey(e => new { e.Rid, e.Cpid }).HasName("pk_TRAVEL_RATES");

            entity.HasOne(d => d.Cp).WithMany(p => p.TravelRates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TRAVEL_COVERPERIOD");

            entity.HasOne(d => d.RidNavigation).WithMany(p => p.TravelRates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TRAVEL_REGIONS");
        });

        modelBuilder.Entity<TravelRegion>(entity =>
        {
            entity.HasKey(e => e.Rid).HasName("PK_RID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

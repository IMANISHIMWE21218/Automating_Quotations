/*using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Automating_Quotations.Models.Motor;

public partial class BkgiDataContext : DbContext
{
    public BkgiDataContext()
    {
    }

    public BkgiDataContext(DbContextOptions<BkgiDataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MtDuration> MtDurations { get; set; }

    public virtual DbSet<MtFire> MtFires { get; set; }

    public virtual DbSet<MtMotoQuotationTable> MtMotoQuotationTables { get; set; }

    public virtual DbSet<MtMotorType> MtMotorTypes { get; set; }

    public virtual DbSet<MtOwnDamage> MtOwnDamages { get; set; }

    public virtual DbSet<MtTarifOccupant> MtTarifOccupants { get; set; }

    public virtual DbSet<MtTerritorialCoverLimit> MtTerritorialCoverLimits { get; set; }

    public virtual DbSet<MtTheft> MtThefts { get; set; }

    public virtual DbSet<MtThirdparty> MtThirdparties { get; set; }

    public virtual DbSet<MtTypeOfClient> MtTypeOfClients { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DataConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MtDuration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MT_durat__3214EC2749FA3AC5");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<MtFire>(entity =>
        {
            entity.HasKey(e => e.CodeType).HasName("PK__MT_Fire__D07465C1A2054F73");

            entity.Property(e => e.CodeType).ValueGeneratedNever();
        });

        modelBuilder.Entity<MtMotoQuotationTable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MT_MOTO___3214EC278F52EEBE");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.MtMotorTypeNavigation).WithMany(p => p.MtMotoQuotationTables).HasConstraintName("FK__MT_MOTO_Q__MT_Mo__3A4CA8FD");

            entity.HasOne(d => d.OccupantNavigation).WithMany(p => p.MtMotoQuotationTables).HasConstraintName("FK__MT_MOTO_Q__Occup__3B40CD36");

            entity.HasOne(d => d.PeriodOfInsuranceNavigation).WithMany(p => p.MtMotoQuotationTables).HasConstraintName("FK__MT_MOTO_Q__Perio__3D2915A8");

            entity.HasOne(d => d.TerritoryLimitsNavigation).WithMany(p => p.MtMotoQuotationTables).HasConstraintName("FK__MT_MOTO_Q__Terri__3C34F16F");

            entity.HasOne(d => d.TypeOfClientNavigation).WithMany(p => p.MtMotoQuotationTables).HasConstraintName("FK__MT_MOTO_Q__TypeO__3E1D39E1");
        });

        modelBuilder.Entity<MtMotorType>(entity =>
        {
            entity.HasKey(e => e.CodeType).HasName("PK__MT_Motor__D07465C1C74097E6");

            entity.Property(e => e.CodeType).ValueGeneratedNever();
        });

        modelBuilder.Entity<MtOwnDamage>(entity =>
        {
            entity.HasKey(e => e.CodeType).HasName("PK__MT_OwnDa__D07465C1F9779C4E");

            entity.Property(e => e.CodeType).ValueGeneratedNever();
        });

        modelBuilder.Entity<MtTarifOccupant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MT_TARIF__3214EC278E60209D");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<MtTerritorialCoverLimit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MT_Terri__3213E83FA0713CC3");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<MtTheft>(entity =>
        {
            entity.HasKey(e => e.CodeType).HasName("PK__MT_Theft__D07465C1BA92D6B8");

            entity.Property(e => e.CodeType).ValueGeneratedNever();
        });

        modelBuilder.Entity<MtThirdparty>(entity =>
        {
            entity.HasKey(e => e.CodeType).HasName("PK__MT_Third__D07465C1C5628B9C");

            entity.Property(e => e.CodeType).ValueGeneratedNever();
        });

        modelBuilder.Entity<MtTypeOfClient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MT_TypeO__3213E83F7CAE6BA6");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
*/
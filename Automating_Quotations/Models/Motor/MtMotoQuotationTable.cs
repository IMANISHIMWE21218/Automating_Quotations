using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Automating_Quotations.Models.Motor;

[Table("MT_MOTO_QUOTATION_TABLE")]
public partial class MtMotoQuotationTable
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("MT_MotorType")]
    public int? MtMotorType { get; set; }

    [Column(TypeName = "date")]
    public DateTime? ManufactureDate { get; set; }

    public int? SeatCapacity { get; set; }

    public int? Occupant { get; set; }

    public bool? ThirdParty { get; set; }

    public bool? OwnDamage { get; set; }

    public bool? Theft { get; set; }

    public bool? Fire { get; set; }
    public int? sumInsuredPerOccupant { get; set; }

    public int? TerritoryLimits { get; set; }

    public int? PeriodOfInsurance { get; set; }

    [Column(TypeName = "decimal(9, 2)")]
    public decimal? ValueOfVehicle { get; set; } = 0;

    public int? TypeOfClient { get; set; } //ownershipType

    [ForeignKey("MtMotorType")]
    [InverseProperty("MtMotoQuotationTables")]
    public virtual MtMotorType? MtMotorTypeNavigation { get; set; }

    [ForeignKey("Occupant")]
    [InverseProperty("MtMotoQuotationTables")]
    public virtual MtTarifOccupant? OccupantNavigation { get; set; }

    [ForeignKey("PeriodOfInsurance")]
    [InverseProperty("MtMotoQuotationTables")]
    public virtual MtDuration? PeriodOfInsuranceNavigation { get; set; }

    [ForeignKey("TerritoryLimits")]
    [InverseProperty("MtMotoQuotationTables")]
    public virtual MtTerritorialCoverLimit? TerritoryLimitsNavigation { get; set; }

    [ForeignKey("TypeOfClient")]
    [InverseProperty("MtMotoQuotationTables")]
    public virtual MtTypeOfClient? TypeOfClientNavigation { get; set; }
}

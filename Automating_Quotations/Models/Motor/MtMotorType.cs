using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Automating_Quotations.Models.Motor;

[Table("MT_MotorType")]
public partial class MtMotorType
{
    [Column("Type_of_Motor")]
    [StringLength(255)]
    [Unicode(false)]
    public string? TypeOfMotor { get; set; }

    [Key]
    public int CodeType { get; set; }

    [Column("Vehicle_Usage_Code")]
    public int? VehicleUsageCode { get; set; }

    [Column("Vehicle_Usage_Label")]
    [StringLength(255)]
    [Unicode(false)]
    public string? VehicleUsageLabel { get; set; }

    [Column("SEATS")]
    public int? Seats { get; set; }

    [Column("Glass_Breakage_Premium", TypeName = "decimal(10, 4)")]
    public decimal? GlassBreakagePremium { get; set; }

    [Column("Theft_Comesa", TypeName = "decimal(10, 4)")]
    public decimal? TheftComesa { get; set; }

    [InverseProperty("MtMotorTypeNavigation")]
    public virtual ICollection<MtMotoQuotationTable> MtMotoQuotationTables { get; set; } = new List<MtMotoQuotationTable>();
}

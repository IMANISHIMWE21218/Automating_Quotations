using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Automating_Quotations.Models.Travel;

[PrimaryKey("Rid", "Cpid")]
[Table("TRAVEL_RATES")]
public partial class TravelRate
{
    [Key]
    [Column("RID")]
    [StringLength(5)]
    [Unicode(false)]
    public string Rid { get; set; } = null!;

    [Key]
    [Column("CPID")]
    [StringLength(5)]
    [Unicode(false)]
    public string Cpid { get; set; } = null!;

    [Column("amount", TypeName = "decimal(20, 2)")]
    public decimal? Amount { get; set; }

    [ForeignKey("Cpid")]
    [InverseProperty("TravelRates")]
    public virtual TravelCoverperiod Cp { get; set; } = null!;

    [ForeignKey("Rid")]
    [InverseProperty("TravelRates")]
    public virtual TravelRegion RidNavigation { get; set; } = null!;
}

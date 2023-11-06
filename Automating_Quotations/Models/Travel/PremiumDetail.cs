using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Automating_Quotations.Models.Travel;

public partial class PremiumDetail
{
    [Key]
    [Column("PremiumID")]
    public int PremiumId { get; set; }

    [Column("CoveragePeriodID")]
    public int? CoveragePeriodId { get; set; }

    [Column(TypeName = "decimal(9, 2)")]
    public decimal Reinsurance { get; set; }

    [Column(TypeName = "decimal(9, 2)")]
    public decimal ReinsurancePremium { get; set; }

    [StringLength(20)]
    public string AdminFees { get; set; } = null!;

    [ForeignKey("CoveragePeriodId")]
    [InverseProperty("PremiumDetails")]
    public virtual CoveragePeriod? CoveragePeriod { get; set; }
}

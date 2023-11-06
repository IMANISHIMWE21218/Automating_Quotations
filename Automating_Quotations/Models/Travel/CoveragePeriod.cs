using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Automating_Quotations.Models.Travel;

[Table("CoveragePeriod")]
public partial class CoveragePeriod
{
    [Key]
    [Column("CoveragePeriodID")]
    public int CoveragePeriodId { get; set; }

    [Column("CoverageAreaID")]
    public int? CoverageAreaId { get; set; }

    [StringLength(255)]
    public string CoveragePeriodName { get; set; } = null!;

    [ForeignKey("CoverageAreaId")]
    [InverseProperty("CoveragePeriods")]
    public virtual CoverageArea? CoverageArea { get; set; }
    [JsonIgnore]
    [InverseProperty("CoveragePeriod")]
    public virtual ICollection<PremiumDetail> PremiumDetails { get; set; } = new List<PremiumDetail>();
}

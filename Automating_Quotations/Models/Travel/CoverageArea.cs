using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Automating_Quotations.Models.Travel;

[Table("CoverageArea")]
public partial class CoverageArea
{
    [Key]
    [Column("CoverageAreaID")]
    public int CoverageAreaId { get; set; }

    [StringLength(255)]
    public string CoverageAreaName { get; set; } = null!;

    [JsonIgnore]
    [InverseProperty("CoverageArea")]
    public virtual ICollection<CoveragePeriod> CoveragePeriods { get; set; } = new List<CoveragePeriod>();
}

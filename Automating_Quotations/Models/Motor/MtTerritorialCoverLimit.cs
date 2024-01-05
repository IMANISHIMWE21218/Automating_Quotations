using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Automating_Quotations.Models.Motor;

[Table("MT_TerritorialCoverLimits")]
public partial class MtTerritorialCoverLimit
{
    [StringLength(50)]
    [Unicode(false)]
    public string Description { get; set; } = null!;

    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("rate", TypeName = "decimal(3, 1)")]
    public decimal Rate { get; set; }

    [InverseProperty("TerritoryLimitsNavigation")]
    public virtual ICollection<MtMotoQuotationTable> MtMotoQuotationTables { get; set; } = new List<MtMotoQuotationTable>();
}

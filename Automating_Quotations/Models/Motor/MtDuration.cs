using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Automating_Quotations.Models.Motor;

[Table("MT_duration")]
public partial class MtDuration
{
    [Column("DURATION")]
    [StringLength(50)]
    [Unicode(false)]
    public string Duration { get; set; } = null!;

    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("RATE", TypeName = "decimal(5, 3)")]
    public decimal Rate { get; set; }

    [InverseProperty("PeriodOfInsuranceNavigation")]
    public virtual ICollection<MtMotoQuotationTable> MtMotoQuotationTables { get; set; } = new List<MtMotoQuotationTable>();
}

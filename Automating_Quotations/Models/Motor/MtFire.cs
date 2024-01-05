using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Automating_Quotations.Models.Motor;

[Table("MT_Fire")]
public partial class MtFire
{
    [Key]
    public int CodeType { get; set; }

    [Column("INCENDIE_less_than_5_years", TypeName = "decimal(10, 4)")]
    public decimal? IncendieLessThan5Years { get; set; }

    [Column("INCENDIE_5_to_10_years", TypeName = "decimal(10, 4)")]
    public decimal? Incendie5To10Years { get; set; }

    [Column("INCENDIE_greater_than_10_years", TypeName = "decimal(10, 4)")]
    public decimal? IncendieGreaterThan10Years { get; set; }
}

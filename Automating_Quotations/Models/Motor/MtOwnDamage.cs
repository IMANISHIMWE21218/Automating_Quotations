using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Automating_Quotations.Models.Motor;

[Table("MT_OwnDamage")]
public partial class MtOwnDamage
{
    [Key]
    public int CodeType { get; set; }

    [Column("DM_less_than_5_years", TypeName = "decimal(10, 4)")]
    public decimal? DmLessThan5Years { get; set; }

    [Column("DM_5_to_10_years", TypeName = "decimal(10, 4)")]
    public decimal? Dm5To10Years { get; set; }

    [Column("DM_greater_than_10_years", TypeName = "decimal(10, 4)")]
    public decimal? DmGreaterThan10Years { get; set; }
}

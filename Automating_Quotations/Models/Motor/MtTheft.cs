using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Automating_Quotations.Models.Motor;

[Table("MT_Theft")]
public partial class MtTheft
{
    [Key]
    public int CodeType { get; set; }

    [Column("VOL_less_than_5_years", TypeName = "decimal(10, 4)")]
    public decimal? VolLessThan5Years { get; set; }

    [Column("VOL_5_to_10_years", TypeName = "decimal(10, 4)")]
    public decimal? Vol5To10Years { get; set; }

    [Column("VOL_greater_than_10_years", TypeName = "decimal(10, 4)")]
    public decimal? VolGreaterThan10Years { get; set; }
}

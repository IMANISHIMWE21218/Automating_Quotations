using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Automating_Quotations.Models.Motor;

[Table("MT_Thirdparty")]
public partial class MtThirdparty
{
    [Column("Type_of_Motor")]
    [StringLength(255)]
    [Unicode(false)]
    public string? TypeOfMotor { get; set; }

    [Key]
    public int CodeType { get; set; }

    [Column("Rc_less_than_5_years", TypeName = "decimal(10, 2)")]
    public decimal? RcLessThan5Years { get; set; }

    [Column("Rc_5_to_10_years", TypeName = "decimal(10, 2)")]
    public decimal? Rc5To10Years { get; set; }

    [Column("Rc_greater_than_10_years", TypeName = "decimal(10, 2)")]
    public decimal? RcGreaterThan10Years { get; set; }
}

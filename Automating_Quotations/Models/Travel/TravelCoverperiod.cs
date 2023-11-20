using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Automating_Quotations.Models.Travel;

[Table("TRAVEL_COVERPERIOD")]
public partial class TravelCoverperiod
{
    [Key]
    [Column("CPID")]
    [StringLength(5)]
    [Unicode(false)]
    public string Cpid { get; set; } = null!;

    [Column("DESCRIPTION")]
    [StringLength(30)]
    [Unicode(false)]
    public string? Description { get; set; }

    [InverseProperty("Cp")]
    public virtual ICollection<TravelRate> TravelRates { get; set; } = new List<TravelRate>();
}

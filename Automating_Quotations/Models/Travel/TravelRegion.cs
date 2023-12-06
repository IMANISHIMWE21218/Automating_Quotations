using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Automating_Quotations.Models.Travel;

[Table("TRAVEL_REGIONS")]
public partial class TravelRegion
{
    [Key]
    [Column("RID")]
    [StringLength(5)]
    [Unicode(false)]
    public string Rid { get; set; } = null!;

    [Column("REGION")]
    [StringLength(30)]
    [Unicode(false)]
    public string? Region { get; set; }
    [JsonIgnore]
    [InverseProperty("RidNavigation")]
    public virtual ICollection<TravelRate> TravelRates { get; set; } = new List<TravelRate>();
}

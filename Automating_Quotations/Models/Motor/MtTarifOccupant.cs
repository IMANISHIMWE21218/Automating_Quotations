using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Automating_Quotations.Models.Motor;

[Table("MT_TARIF_OCCUPANT")]
public partial class MtTarifOccupant
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("fees", TypeName = "decimal(10, 2)")]
    public decimal Fees { get; set; }

    [Column("death", TypeName = "decimal(10, 2)")]
    public decimal Death { get; set; }

    [Column("medical", TypeName = "decimal(10, 2)")]
    public decimal Medical { get; set; }

    [InverseProperty("OccupantNavigation")]
    public virtual ICollection<MtMotoQuotationTable> MtMotoQuotationTables { get; set; } = new List<MtMotoQuotationTable>();
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Automating_Quotations.Models.Motor;

[Table("MT_TypeOfClient")]
public partial class MtTypeOfClient
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Description { get; set; } = null!;
    public int Rates { get; set; }

    [InverseProperty("TypeOfClientNavigation")]
    public virtual ICollection<MtMotoQuotationTable> MtMotoQuotationTables { get; set; } = new List<MtMotoQuotationTable>();
}

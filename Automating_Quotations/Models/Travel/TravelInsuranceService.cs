using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Automating_Quotations.Models.Travel
{
    [Table("TRAVEL_INSURANCE_SERVICES")]
    public partial class TravelInsuranceService
    {
        [Key]
        [Column("ID")]
        public Guid Id { get; set; }

        [Column("DOB")]
        public string? Dob { get; set; }

        [Column("StartDate")]
        public string? StartDate { get; set; }

        [Column("EndDate")]
        public string? EndDate { get; set; }

        [Column("REGION_ID")]
        [ForeignKey("Region")]
        public string? RegionId { get; set; }

        [Column("COVER_PERIOD_ID")]
        [ForeignKey("CoverPeriod")]
        public string? CoverPeriodId { get; set; }

        // Navigation properties
        [JsonIgnore]
        public virtual TravelRegion? Region { get; set; }
        [JsonIgnore]
        public virtual TravelCoverperiod? CoverPeriod { get; set; }
    }
}

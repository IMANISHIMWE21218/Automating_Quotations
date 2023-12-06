using System.ComponentModel.DataAnnotations.Schema;

namespace Automating_Quotations.Models.Travel
{
    public class AddTravelInsuranceService
    {
      
        public string? Dob { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? RegionId { get; set; }
        public string? CoverPeriodId { get; set; }
        // Navigation properties
        public virtual TravelRegion? Region { get; set; }
        public virtual TravelCoverperiod? CoverPeriod { get; set; }
    }
}

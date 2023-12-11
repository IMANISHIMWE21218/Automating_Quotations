using System.ComponentModel.DataAnnotations.Schema;

using System.Text.Json.Serialization;
namespace Automating_Quotations.Models.Travel
{
    public class AddTravelInsuranceService
    {
        public string? Dob { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? RegionId { get; set; }
        public string? CoverPeriodId { get; set; }
    }
}


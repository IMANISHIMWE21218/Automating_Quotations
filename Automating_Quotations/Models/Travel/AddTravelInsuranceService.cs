using System.ComponentModel.DataAnnotations.Schema;

using System.Text.Json.Serialization;
namespace Automating_Quotations.Models.Travel
{
    public class AddTravelInsuranceService
    {
        public DateOnly? Dob { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public decimal? RateOfExchange { get; set; }= 0;
        public string? RegionId { get; set; }
        public string? CoverPeriodId { get; set; }

       
    }
}


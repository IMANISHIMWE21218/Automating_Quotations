using Automating_Quotations.Data;
using Automating_Quotations.Models.Travel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Automating_Quotations.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TravelInsuranceController : ControllerBase
    {
        private readonly BkgiDataContext _context;

        public TravelInsuranceController(BkgiDataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TravelInsuranceService>>> GetTravelInsuranceServices()
        {
            return Ok(await _context.TravelInsuranceServices.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> AddTravelInsuranceService([FromBody] AddTravelInsuranceService addTravelInsuranceService)
        {
            try
            {
                var travelRateData = await FetchTravelRateData(addTravelInsuranceService.RegionId, addTravelInsuranceService.CoverPeriodId);

                travelRateData = travelRateData
                    .Where(tr => tr.Rid == addTravelInsuranceService.RegionId && tr.Cpid == addTravelInsuranceService.CoverPeriodId)
                    .ToList();

                int daysDifference = (int)(addTravelInsuranceService.EndDate?.DayNumber - addTravelInsuranceService.StartDate?.DayNumber);

                var AdminFee = 4875;

                var currentDate = DateTime.Now;
                int age = currentDate.Year - addTravelInsuranceService.Dob.GetValueOrDefault().Year;

                var travelRateDataResponse = travelRateData.Any()
                    ? travelRateData.Select(tr => new
                      {
                          tr.Rid,
                          tr.Cpid,
                          tr.Amount,
                          currentDate = DateTime.Now,
                          age,
                          NetPrimium = (addTravelInsuranceService.RateOfExchange.GetValueOrDefault() * tr.Amount) + AdminFee,
                          GrossPremium = CalculateGrossPremium(tr.Amount, age, addTravelInsuranceService.RateOfExchange.GetValueOrDefault(), AdminFee),
                          tr.Cp,
                          tr.RidNavigation
                      }).ToList()
                    : (object)null;

                var responseModel = new
                {
                    Dob = addTravelInsuranceService.Dob?.ToString("MM/dd/yyyy"),
                    StartDate = addTravelInsuranceService.StartDate?.ToString("MM/dd/yyyy"),
                    EndDate = addTravelInsuranceService.EndDate?.ToString("MM/dd/yyyy"),
                    addTravelInsuranceService.RegionId,
                    addTravelInsuranceService.CoverPeriodId,
                    DaysDifference = daysDifference,
                    TravelRateData = travelRateDataResponse
                };

                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                // Consider using ILogger for proper logging in a real-world scenario
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        private decimal CalculateGrossPremium(decimal? amount, int age, decimal rateOfExchange, int adminFee)
        {
            if (!amount.HasValue)
            {
                // Handle the case when amount is null (e.g., return a default value or throw an exception)
                throw new ArgumentException("Amount cannot be null.", nameof(amount));
            }

            // decimal grossPremium = (rateOfExchange * amount.Value) + adminFee;
            decimal grossPremium = ((rateOfExchange * amount.Value) + adminFee) +
               (((rateOfExchange * amount.Value) + adminFee) * 18 / 100);

            if (age >= 3 && age <= 18)
            {
                // Reduction of 50%
                grossPremium *= 0.5m;
            }
            else if (age >= 66 && age <= 75)
            {
                // Increase of 50%
                grossPremium *= 1.5m;
            }
            else if (age >= 76 && age <= 80)
            {
                // Increase of 100%
                grossPremium *= 2.0m;
            }
            else if (age >= 81)
            {
                // Increase of 300%
                grossPremium *= 4.0m;
            }

            return grossPremium;
        }


        private async Task<List<TravelRate>> FetchTravelRateData(string regionId, string coverPeriodId)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"https://localhost:7110/api/TravelRates?regionId={regionId}&coverPeriodId={coverPeriodId}");
                    response.EnsureSuccessStatusCode();
                    return JsonConvert.DeserializeObject<List<TravelRate>>(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                // Consider using ILogger for proper logging in a real-world scenario
                Console.WriteLine($"Error in FetchTravelRateData: {ex.Message}");
                throw;
            }
        }
    }
}

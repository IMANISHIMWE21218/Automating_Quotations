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
        public IActionResult AddTravelInsuranceService([FromBody] AddTravelInsuranceService addTravelInsuranceService)
        {
            try
            {
                // Create a response model without the Id
                var responseModel = new
                {
                    Dob = addTravelInsuranceService.Dob,
                    StartDate = addTravelInsuranceService.StartDate,
                    EndDate = addTravelInsuranceService.EndDate,
                    RegionId = addTravelInsuranceService.RegionId,
                    CoverPeriodId = addTravelInsuranceService.CoverPeriodId
                };

                // Do not save to the database, just respond with the data
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        private async Task<TravelRate> FetchTravelRateData(string regionId, string coverPeriodId)
        {
            // Implement logic to fetch data from Travel Rate API based on regionId and coverPeriodId
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"https://localhost:7110/api/TravelRates?regionId={regionId}&coverPeriodId={coverPeriodId}");
                Console.WriteLine(response);
                response.EnsureSuccessStatusCode();
                var travelRateData = JsonConvert.DeserializeObject<TravelRate>(await response.Content.ReadAsStringAsync());
                return travelRateData;
            }
        }
    }
}

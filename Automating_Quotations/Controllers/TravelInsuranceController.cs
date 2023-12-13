using Automating_Quotations.Data;
using Automating_Quotations.Models.Travel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
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
                // Fetch TravelRate data
                var travelRateData = await FetchTravelRateData(addTravelInsuranceService.RegionId, addTravelInsuranceService.CoverPeriodId);

                // Create a response model including the filtered TravelRate data
                var responseModel = new
                {
                    Dob = addTravelInsuranceService.Dob,
                    StartDate = addTravelInsuranceService.StartDate,
                    EndDate = addTravelInsuranceService.EndDate,
                    RegionId = addTravelInsuranceService.RegionId,
                    CoverPeriodId = addTravelInsuranceService.CoverPeriodId,
                    TravelRateData = travelRateData
                };

                // Do not save to the database, just respond with the data
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        private async Task<List<TravelRate>> FetchTravelRateData(string regionId, string coverPeriodId)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"https://localhost:7110/api/TravelRates?regionId={regionId}&coverPeriodId={coverPeriodId}");
                    response.EnsureSuccessStatusCode();
                    var travelRateData = JsonConvert.DeserializeObject<List<TravelRate>>(await response.Content.ReadAsStringAsync());

                    // Filter the data based on RegionId and CoverPeriodId
                    travelRateData = travelRateData
                    .Where(tr => tr.Rid == regionId && tr.Cpid == coverPeriodId)
                    .ToList();

                    return travelRateData;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in FetchTravelRateData: {ex.Message}");
                throw; // Rethrow the exception to propagate it up the call stack
            }
        }
    }
}


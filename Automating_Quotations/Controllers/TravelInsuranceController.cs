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

                // Filter TravelRate data based on posted IDs (RegionId and CoverPeriodId)
                travelRateData = travelRateData
                    .Where(tr => tr.Rid == addTravelInsuranceService.RegionId && tr.Cpid == addTravelInsuranceService.CoverPeriodId)
                    .ToList();

                // Determine how to include TravelRateData based on whether it's empty or not
                object travelRateDataResponse = travelRateData.Any()
                    ? (object)travelRateData // Include the filtered data if not empty
                    : (object)null; // Include null or an empty object if the list is empty

                // Create a response model including the filtered TravelRate data
                var responseModel = new
                {
                    Dob = addTravelInsuranceService.Dob,
                    StartDate = addTravelInsuranceService.StartDate,
                    EndDate = addTravelInsuranceService.EndDate,
                    RegionId = addTravelInsuranceService.RegionId,
                    CoverPeriodId = addTravelInsuranceService.CoverPeriodId,
                    TravelRateData = travelRateDataResponse
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

                    // Debugging: Print the retrieved data
                    Console.WriteLine($"Retrieved data from API: {JsonConvert.SerializeObject(travelRateData)}");

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


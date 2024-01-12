using Automating_Quotations.Data;
using Automating_Quotations.Models.Motor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Drawing;

namespace Automating_Quotations.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MtMotorInsuranceController : ControllerBase
    {
        private readonly BkgiDataContext _context;

        public MtMotorInsuranceController(BkgiDataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MtMotoQuotationTable>>> GetMtMotoQuotationTables()
        {
            return Ok(await _context.MtMotoQuotationTables.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> PostMotorInsurance([FromBody] MotorInsurance motorInsurance)
        {
            try
            {
                // Fetch data from MotorTypes API based on the posted MtMotorType
                var motorType = await FetchMotorTypeData((int)motorInsurance.MtMotorType);


                // Fetch data from MtTarifOccupant API
                var occupant = await FetchOccupantData((int)motorInsurance.Occupant);

                // Fetch data from MtTerritorialCoverLimit API
                var territorialCoverLimit = await FetchTerritorialCoverLimitData((int)motorInsurance.TerritoryLimits);

                // Fetch data from MtDuration API
                var duration = await FetchDurationData((int)motorInsurance.PeriodOfInsurance);

                // Fetch data from MtTypeOfClient API
                var typeOfClient = await FetchTypeOfClientData((int)motorInsurance.TypeOfClient);

                // Process the retrieved data as needed

                return Ok(new
                {
                    MotorType = motorType,
                    Occupant = occupant,
                    TerritorialCoverLimit = territorialCoverLimit,
                    Duration = duration,
                    TypeOfClient = typeOfClient,
                    PostedData = motorInsurance
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        private async Task<List<MtMotorType>> FetchMotorTypeData(int MtMotorType)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"https://localhost:7110/api/MotorTypes?MtMotorType={MtMotorType}");

                    // Log the API response content
                    Console.WriteLine($"API Response: {await response.Content.ReadAsStringAsync()}");

                    response.EnsureSuccessStatusCode();
                    return JsonConvert.DeserializeObject<List<MtMotorType>>(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                // Consider using ILogger for proper logging in a real-world scenario
                Console.WriteLine($"Error in FetchMotorTypeData: {ex.Message}");
                throw;
            }
        }




        private async Task<MtTarifOccupant> FetchOccupantData(int Id)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"https://localhost:7110/api/MtTarifOccupant/{Id}");
                    response.EnsureSuccessStatusCode();
                    return JsonConvert.DeserializeObject<MtTarifOccupant>(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in FetchOccupantData: {ex.Message}");
                throw;
            }
        }


        private async Task<MtTerritorialCoverLimit> FetchTerritorialCoverLimitData(int Id)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"https://localhost:7110/api/MtTerritorialCoverLimit/{Id}");
                    response.EnsureSuccessStatusCode();
                    return JsonConvert.DeserializeObject<MtTerritorialCoverLimit>(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in FetchTerritorialCoverLimitData: {ex.Message}");
                throw;
            }
        }

        private async Task<MtDuration> FetchDurationData(int Id)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"https://localhost:7110/api/MtDuration/{Id}");
                    response.EnsureSuccessStatusCode();
                    return JsonConvert.DeserializeObject<MtDuration>(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in FetchDurationData: {ex.Message}");
                throw;
            }
        }

        private async Task<MtTypeOfClient> FetchTypeOfClientData(int Id)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"https://localhost:7110/api/MtTypeOfClient/{Id}");
                    response.EnsureSuccessStatusCode();
                    return JsonConvert.DeserializeObject<MtTypeOfClient>(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in FetchTypeOfClientData: {ex.Message}");
                throw;
            }
        }



    }
}

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
                Console.WriteLine($"Posted MtMotorType: {motorInsurance.MtMotorType}");

                // Fetch data from MotorTypes API based on the posted MtMotorType
                var thirdparty = await FetchMotorTypeData((int)motorInsurance.MtMotorType);
                // Fetch data from MotorTypes API based on the posted MtMotorType
                var rcLessThan5Years = thirdparty.FirstOrDefault()?.RcLessThan5Years ?? 0; // Access the specific element

                // Perform your calculation
                var sum = rcLessThan5Years + 500;

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
                    Thirdparty = thirdparty,
                    Occupant = occupant,
                    TerritorialCoverLimit = territorialCoverLimit,
                    Duration = duration,
                    TypeOfClient = typeOfClient,
                    Sum = sum,
                    PostedData = motorInsurance

                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        private async Task<List<MtThirdparty>> FetchMotorTypeData(int MtMotorType)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"https://localhost:7110/api/Thirdparty");
                    response.EnsureSuccessStatusCode();

                    var responseData = await response.Content.ReadAsStringAsync();
                    var thirdparties = JsonConvert.DeserializeObject<List<MtThirdparty>>(responseData);

                    // Filter the results based on codeType
                    var filteredthirdparties = thirdparties.Where(mt => mt.CodeType == MtMotorType).ToList();
                    Console.WriteLine("Dattaaaa"+ filteredthirdparties);

                    return filteredthirdparties;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in FetchMotorTypeData: {ex.Message}");
                throw;
            }
        }





        private async Task<List<MtTarifOccupant>> FetchOccupantData(int Occupant)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"https://localhost:7110/api/MtTarifOccupant");
                    response.EnsureSuccessStatusCode();

                    var responseData = await response.Content.ReadAsStringAsync();
                    var occupants = JsonConvert.DeserializeObject<List<MtTarifOccupant>>(responseData);

                    // Filter the results based on occupantId
                    var filteredOccupants = occupants.Where(occupant => occupant.Id == Occupant).ToList();
                    Console.WriteLine("Occupant Data: " + JsonConvert.SerializeObject(filteredOccupants));

                    return filteredOccupants;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in FetchOccupantData: {ex.Message}");
                throw;
            }
        }


        private async Task<List<MtTerritorialCoverLimit>> FetchTerritorialCoverLimitData(int territoryLimits)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"https://localhost:7110/api/MtTerritorialCoverLimit");
                    response.EnsureSuccessStatusCode();

                    var responseData = await response.Content.ReadAsStringAsync();
                    var territorialCoverLimits = JsonConvert.DeserializeObject<List<MtTerritorialCoverLimit>>(responseData);

                    // Filter the results based on territoryLimitsId
                    var filteredTerritorialCoverLimits = territorialCoverLimits
                        .Where(territorialCoverLimit => territorialCoverLimit.Id == territoryLimits)
                        .ToList();

                    Console.WriteLine("Territorial Cover Limit Data: " + JsonConvert.SerializeObject(filteredTerritorialCoverLimits));

                    return filteredTerritorialCoverLimits;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in FetchTerritorialCoverLimitData: {ex.Message}");
                throw;
            }
        }



        private async Task<List<MtDuration>> FetchDurationData(int periodOfInsurance)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"https://localhost:7110/api/MtDuration");
                    response.EnsureSuccessStatusCode();

                    var responseData = await response.Content.ReadAsStringAsync();
                    var durations = JsonConvert.DeserializeObject<List<MtDuration>>(responseData);

                    // Filter the results based on PeriodOfInsurance Id
                    var filteredDurations = durations.Where(d => d.Id == periodOfInsurance).ToList();

                    Console.WriteLine("Duration Data: " + JsonConvert.SerializeObject(filteredDurations));

                    return filteredDurations;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in FetchDurationData: {ex.Message}");
                throw;
            }
        }




        private async Task<List<MtTypeOfClient>> FetchTypeOfClientData(int typeOfClient)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"https://localhost:7110/api/MtTypeOfClient");
                    response.EnsureSuccessStatusCode();

                    var responseData = await response.Content.ReadAsStringAsync();
                    var typeOfClients = JsonConvert.DeserializeObject<List<MtTypeOfClient>>(responseData);

                    // Filter the results based on TypeOfClient Id
                    var filteredTypeOfClients = typeOfClients.Where(tc => tc.Id == typeOfClient).ToList();

                    Console.WriteLine("TypeOfClient Data: " + JsonConvert.SerializeObject(filteredTypeOfClients));

                    return filteredTypeOfClients;
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

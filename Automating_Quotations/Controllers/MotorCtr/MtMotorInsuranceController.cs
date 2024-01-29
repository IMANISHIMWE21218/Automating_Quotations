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
                // Check if manufactureDate is provided
                if (motorInsurance.ManufactureDate == null)
                {
                    return BadRequest("ManufactureDate is required.");
                }

                DateOnly manufactureDate = motorInsurance.ManufactureDate.Value;
                int vehicleAge = DateTime.Now.Year - manufactureDate.Year;

                // Fetch data from MotorTypes API based on the posted MtMotorType
                var thirdparty = await FetchMotorTypeData((int)motorInsurance.MtMotorType);

                var ownDamageData = await FetchOwnDamageData((int)motorInsurance.MtMotorType);

                var theftData = await FetchTheftData((int)motorInsurance.MtMotorType);

                var fireData = await FetchFireData((int)motorInsurance.MtMotorType);

                // Fetch data from MtTarifOccupant API
                var occupant = await FetchOccupantData((int)motorInsurance.Occupant);

                var MotorTypesSeats = await Fetch_SeatLoadData((int)motorInsurance.MtMotorType);
               


                // Fetch data from MotorTypes API based on the posted MtMotorType
                // Access the specific element based on vehicleAge

                // valiables declared
                decimal NpBthirdpartyDvalue = 0;
                decimal NpmaterialDamage = 0;
                decimal Nptheft = 0;
                decimal Npfire = 0;
                decimal Npoccupant = 0;
                decimal vehicleValue = (decimal)motorInsurance.ValueOfVehicle;
               
                decimal sumInsuredPerOccupant = (decimal)motorInsurance.sumInsuredPerOccupant;
                decimal occupantRate = 0.005m;   //0.5%
                var seats = 0;

                if (vehicleAge < 5)
                {
                    NpBthirdpartyDvalue = thirdparty.FirstOrDefault()?.RcLessThan5Years ?? 0;
                    NpmaterialDamage = ownDamageData.FirstOrDefault()?.DmLessThan5Years ?? 0;
                    Nptheft = theftData.FirstOrDefault()?.VolLessThan5Years ?? 0;
                    Npfire = fireData.FirstOrDefault()?.IncendieLessThan5Years ?? 0;
                    Npoccupant = occupant.FirstOrDefault()?.Death ?? 0;
                    seats = MotorTypesSeats.FirstOrDefault()?.Seats ?? 0;
                }
                else if (vehicleAge >= 5 && vehicleAge <= 10)
                {
                    NpBthirdpartyDvalue = thirdparty.FirstOrDefault()?.Rc5To10Years ?? 0;
                    NpmaterialDamage = ownDamageData.FirstOrDefault()?.Dm5To10Years ?? 0;
                    Nptheft = theftData.FirstOrDefault()?.Vol5To10Years ?? 0;
                    Npoccupant = occupant.FirstOrDefault()?.Death ?? 0;
                    seats = MotorTypesSeats.FirstOrDefault()?.Seats ?? 0;
                }
                else if (vehicleAge > 10)
                {
                    NpBthirdpartyDvalue = thirdparty.FirstOrDefault()?.RcGreaterThan10Years ?? 0;
                    NpmaterialDamage = ownDamageData.FirstOrDefault()?.DmGreaterThan10Years ?? 0;
                    Nptheft = theftData.FirstOrDefault()?.VolGreaterThan10Years ?? 0;
                    Npoccupant = occupant.FirstOrDefault()?.Death ?? 0;
                    seats = MotorTypesSeats.FirstOrDefault()?.Seats ?? 0;

                }

                Console.WriteLine("seatinnnnnnnnnnnnng    .........." + seats);


                decimal T_Np_thirdparty = 0;
                decimal T_Np_materialDamage = 0;
                decimal T_Np_theft = 0;
                decimal T_Np_fire = 0;
                decimal T_Np_occupant = 0;
                decimal T_Np_seatsLoads = 0;

                //decimal loading = 25/100;

                decimal loading;

                if (vehicleAge <= 5)
                {
                    loading = 0;
                }
                else
                {
                    loading = 0.25m;
                }
                Console.WriteLine("Da .      ... .!" + loading);

                T_Np_thirdparty = NpBthirdpartyDvalue;
                //T_Np_materialDamage = ((vehicleValue* loading)+ vehicleValue) * NpmaterialDamage ;
                //T_Np_theft = ((vehicleValue * loading) + vehicleValue) * Nptheft;
                //T_Np_fire = ((vehicleValue * loading) + vehicleValue) * Npfire;

                // ...

                bool ownDamage = motorInsurance.OwnDamage ?? false;
                bool theft = motorInsurance.Theft ?? false;
                bool fire = motorInsurance.Fire ?? false;

                T_Np_materialDamage = ownDamage ? ((vehicleValue * loading) + vehicleValue) * NpmaterialDamage : 0;
                T_Np_theft = theft ? ((vehicleValue * loading) + vehicleValue) * Nptheft : 0;
                T_Np_fire = fire ? ((vehicleValue * loading) + vehicleValue) * Npfire : 0;

                // ...


                decimal vehicle_type = (decimal)motorInsurance.MtMotorType;

                if (vehicle_type == 12)
                {
                    T_Np_occupant = ((Npoccupant * occupantRate) * sumInsuredPerOccupant);
                }
                else
                {
                    T_Np_occupant = (Npoccupant * occupantRate);
                }

                decimal vehicle_SeatCapacity = (int)motorInsurance.SeatCapacity;

                T_Np_seatsLoads = seats * vehicle_SeatCapacity;






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
                    PostedData = motorInsurance,
                    VehicleAge = vehicleAge,
                    total_Np_thirdparty = T_Np_thirdparty,
                    total_Np_materialDamage = T_Np_materialDamage,
                    total_T_Np_theft = T_Np_theft,
                    total_T_Np_fire = T_Np_fire,
                    total_T_Np_occupant = T_Np_occupant,
                    seats = T_Np_seatsLoads


                }) ;
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



        private async Task<List<MtOwnDamage>> FetchOwnDamageData(int MtMotorType)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"https://localhost:7110/api/MtOwnDamage");
                    response.EnsureSuccessStatusCode();

                    var responseData = await response.Content.ReadAsStringAsync();
                    var ownDamageData = JsonConvert.DeserializeObject<List<MtOwnDamage>>(responseData);

                    // Filter the results based on MtMotorType
                    var filteredOwnDamageData = ownDamageData.Where(damage => damage.CodeType == MtMotorType).ToList();
                    Console.WriteLine("OwnDamage Data: " + JsonConvert.SerializeObject(filteredOwnDamageData));

                    return filteredOwnDamageData;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in FetchOwnDamageData: {ex.Message}");
                throw;
            }
        }


        
        
        private async Task<List<MtTheft>> FetchTheftData(int MtMotorType)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"https://localhost:7110/api/MtTheft");
                    response.EnsureSuccessStatusCode();

                    var responseData = await response.Content.ReadAsStringAsync();
                    var theftData = JsonConvert.DeserializeObject<List<MtTheft>>(responseData);

                    // Filter the results based on MtMotorType
                    var filteredTheftData = theftData.Where(data => data.CodeType == MtMotorType).ToList();
                    Console.WriteLine("Theft Data: " + JsonConvert.SerializeObject(filteredTheftData));

                    return filteredTheftData;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in FetchTheftData: {ex.Message}");
                throw;
            }
        }

        private async Task<List<MtFire>> FetchFireData(int MtMotorType)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"https://localhost:7110/api/MtFire");
                    response.EnsureSuccessStatusCode();

                    var responseData = await response.Content.ReadAsStringAsync();
                    var fireData = JsonConvert.DeserializeObject<List<MtFire>>(responseData);

                    // Filter the results based on MtMotorType
                    var filteredFireData = fireData.Where(data => data.CodeType == MtMotorType).ToList();
                    Console.WriteLine("Fire Data: " + JsonConvert.SerializeObject(filteredFireData));

                    return filteredFireData;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in FetchFireData: {ex.Message}");
                throw;
            }
        }

        private async Task<List<MtMotorType>> Fetch_SeatLoadData(int MtMotorType)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"https://localhost:7110/api/MotorTypes");
                    response.EnsureSuccessStatusCode();

                    var responseData = await response.Content.ReadAsStringAsync();
                    var MotorTypesSeats = JsonConvert.DeserializeObject<List<MtMotorType>>(responseData);

                    // Filter the results based on MtMotorType
                    var filteredMotorTypesSeats = MotorTypesSeats.Where(mt => mt.CodeType == MtMotorType).ToList();
                    Console.WriteLine("Motor Types Seats Data: " + JsonConvert.SerializeObject(filteredMotorTypesSeats));

                    return filteredMotorTypesSeats;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Fetch_SeatLoadData: {ex.Message}");
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

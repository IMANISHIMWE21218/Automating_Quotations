using Automating_Quotations.Data;
using Automating_Quotations.Models.Motor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

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

                // Fetch data from MtTerritorialCoverLimit API
                var territorialCoverLimits = await FetchTerritorialCoverLimitData((int)(motorInsurance.TerritoryLimits ?? 0));

               




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
                var vehicleUsage = "";
                decimal territorialCover = 0;



                if (vehicleAge < 5)
                {
                    NpBthirdpartyDvalue = thirdparty.FirstOrDefault()?.RcLessThan5Years ?? 0;
                    NpmaterialDamage = ownDamageData.FirstOrDefault()?.DmLessThan5Years ?? 0;
                    Nptheft = theftData.FirstOrDefault()?.VolLessThan5Years ?? 0;
                    Npfire = fireData.FirstOrDefault()?.IncendieLessThan5Years ?? 0;
                    Npoccupant = occupant.FirstOrDefault()?.Death ?? 0;
                    seats = MotorTypesSeats.FirstOrDefault()?.Seats ?? 0;
                    vehicleUsage = MotorTypesSeats.FirstOrDefault()?.VehicleUsageLabel ?? "";
                    territorialCover = territorialCoverLimits.FirstOrDefault()?.Rate ?? 0;

                }
                else if (vehicleAge >= 5 && vehicleAge <= 10)
                {
                    NpBthirdpartyDvalue = thirdparty.FirstOrDefault()?.Rc5To10Years ?? 0;
                    NpmaterialDamage = ownDamageData.FirstOrDefault()?.Dm5To10Years ?? 0;
                    Nptheft = theftData.FirstOrDefault()?.Vol5To10Years ?? 0;
                    Npoccupant = occupant.FirstOrDefault()?.Death ?? 0;
                    seats = MotorTypesSeats.FirstOrDefault()?.Seats ?? 0;
                    vehicleUsage = MotorTypesSeats.FirstOrDefault()?.VehicleUsageLabel ?? "";
                    territorialCover = territorialCoverLimits.FirstOrDefault()?.Rate ?? 0;

                }
                else if (vehicleAge > 10)
                {
                    NpBthirdpartyDvalue = thirdparty.FirstOrDefault()?.RcGreaterThan10Years ?? 0;
                    NpmaterialDamage = ownDamageData.FirstOrDefault()?.DmGreaterThan10Years ?? 0;
                    Nptheft = theftData.FirstOrDefault()?.VolGreaterThan10Years ?? 0;
                    Npoccupant = occupant.FirstOrDefault()?.Death ?? 0;
                    seats = MotorTypesSeats.FirstOrDefault()?.Seats ?? 0;
                    vehicleUsage = MotorTypesSeats.FirstOrDefault()?.VehicleUsageLabel ?? "";
                    territorialCover = territorialCoverLimits.FirstOrDefault()?.Rate ?? 0;


                }

                // Assuming Rate is a property of MtTerritorialCoverLimit and is of a numeric type
                

                Console.WriteLine("Territorial Cover Rate ---------: " + territorialCover);





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


                ///  Net Premium/Base 
                ///Total
                var Net_Premium_Base = T_Np_thirdparty + T_Np_materialDamage + T_Np_theft + T_Np_fire + T_Np_occupant + T_Np_seatsLoads;


                ///    Net Premium
                var PhirdParty_Liability = T_Np_thirdparty;
                var MaterialDamage = T_Np_materialDamage;
                var Theft = T_Np_theft;
                var Fire = T_Np_fire;
                var Occupants = T_Np_occupant;
                var SeatLoad = T_Np_seatsLoads;
                ///Total
                var Net_Premium = PhirdParty_Liability + MaterialDamage + Theft + Fire + Occupants + SeatLoad;


                /// Territorial cover extension
                var PhirdParty_Territorial = PhirdParty_Liability * territorialCover;
                var MaterialDamage_Territorial = MaterialDamage * territorialCover;

                var Theft_Territorial = 0;

                if (vehicleUsage.Equals("private", StringComparison.OrdinalIgnoreCase))
                {
                    Theft_Territorial = (int)(vehicleValue * (1 / 100)); // C22*1/100 when vehicleUsage is private
                }
                else
                {
                    Theft_Territorial = (int)(vehicleValue * (0.6m / 100)); // C22*0.6/100 for other cases
                }

                var Fire_Territorial = Fire * territorialCover;
                var Occupants_Territorial = Occupants * territorialCover;
                var SeatLoad_Territorial = SeatLoad * territorialCover * 0;

                ///Total
                var Territorialcoverextension = PhirdParty_Territorial + MaterialDamage_Territorial + Theft_Territorial + Fire_Territorial + Occupants_Territorial + SeatLoad_Territorial;


                ///Motor Guaranty Fund
                ///Total
                var MotorGuarantyFund = PhirdParty_Liability * (10 / 100);

                ///Total
                var AdminFee = 12500;

                ///TAXABLE PREMIUM
                var TaxablePrem_PhirdParty = PhirdParty_Liability + PhirdParty_Territorial + AdminFee;
                var TaxablePrem_MaterialDamage = MaterialDamage + MaterialDamage_Territorial + 0;
                var TaxablePrem_Theft = Theft + Theft_Territorial + 0 ;
                var TaxablePrem_Fire = Fire + Fire_Territorial + 0;
                var TaxablePrem_Occupant = Occupants + Occupants_Territorial + 0;
                var TaxablePrem_SeatLoad = SeatLoad + SeatLoad_Territorial + 0;
                ///Total 
                var TAXABLE_PREMIUM = TaxablePrem_PhirdParty + TaxablePrem_MaterialDamage + TaxablePrem_Theft + TaxablePrem_Fire + TaxablePrem_Occupant + TaxablePrem_SeatLoad;

                ///VAT@18 %
                var vat_PhirdParty = TaxablePrem_PhirdParty * 0.18m;
                var vat_MaterialDamage = TaxablePrem_MaterialDamage * 0.18m;
                var vat_Theft = TaxablePrem_Theft * 0.18m;
                var vat_Fire = TaxablePrem_Fire * 0.18m;
                var vat_Occupant = TaxablePrem_Occupant * 0.18m;
                var vat_SeatLoad = TaxablePrem_SeatLoad * 0.18m;
                ///Total
                var vatTotal = vat_PhirdParty + vat_MaterialDamage + vat_Theft + vat_Fire + vat_Occupant + vat_SeatLoad;

                ///TOTAL PREMIUM
                var thirdparty_TotalPrem = MotorGuarantyFund + TaxablePrem_PhirdParty + vat_PhirdParty;
                var MaterialDamage_TotalPrem = TaxablePrem_MaterialDamage + vat_MaterialDamage;
                var Theft_TotalPrem = TaxablePrem_Theft + vat_Theft;
                var Fire_TotalPrem = TaxablePrem_Fire + vat_Fire;
                var Occupant_TotalPrem = TaxablePrem_Occupant + vat_Occupant;
                var SeatLoad_TotalPrem = TaxablePrem_SeatLoad + vat_SeatLoad;

                var TotalPremium = thirdparty_TotalPrem + MaterialDamage_TotalPrem + Theft_TotalPrem + Fire_TotalPrem + Occupant_TotalPrem + SeatLoad_TotalPrem;


                // Fetch data from MtDuration API
                var duration = await FetchDurationData((int)motorInsurance.PeriodOfInsurance);

                // Fetch data from MtTypeOfClient API
                var typeOfClient = await FetchTypeOfClientData((int)motorInsurance.TypeOfClient);

                List<string> insuranceItems = new List<string>
                {
                    "tpd",
                    "medical expenses",
                    "material damage",
                    "own damage",
                    "theft",
                    "fire"
                };


                // Process the retrieved data as needed

                return Ok(new
                {
                    // Thirdparty = thirdparty,
                    //Occupant = occupant,
                    //TerritorialCoverLimit = territorialCoverLimit,
                    netPremium = Net_Premium,
                    documentFees = AdminFee,
                    sgf = MotorGuarantyFund,
                    vat = vatTotal,
                    totalPremium = TotalPremium,
                    installments = "",
                    covers = insuranceItems

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

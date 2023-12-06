using Automating_Quotations.Data;
using Automating_Quotations.Models.Travel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            return  Ok(await _context.TravelInsuranceServices.ToListAsync());
        }
        [HttpPost]
       //async Task<IActionResult> AddTravelInsuranceService(AddTravelInsuranceService addTravelInsuranceService)
        public async Task<IActionResult> AddTravelInsuranceService([FromBody] AddTravelInsuranceService addTravelInsuranceService)
        {
            var travelService = new TravelInsuranceService()
            {
                Id = Guid.NewGuid(),
                Dob = addTravelInsuranceService.Dob,
                StartDate = addTravelInsuranceService.StartDate,
                EndDate = addTravelInsuranceService.EndDate,
                Region = addTravelInsuranceService.Region,
                CoverPeriod = addTravelInsuranceService.CoverPeriod

            };
            await _context.TravelInsuranceServices.AddAsync(travelService);
            await _context.SaveChangesAsync();  
            return Ok(travelService);

        }
    }
}

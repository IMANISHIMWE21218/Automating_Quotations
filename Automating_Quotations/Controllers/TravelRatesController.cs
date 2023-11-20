using Automating_Quotations.Data;
using Automating_Quotations.Models.Travel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Automating_Quotations.Controllers
{
    [Route("api/TravelRates")]
    [ApiController]
    public class TravelRatesController : ControllerBase
    {
        private readonly BkgiDataContext _context;

        public TravelRatesController(BkgiDataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TravelRate>>> GetTravelRates()
        {
            return await _context.TravelRates.ToListAsync();
        }
    }
}

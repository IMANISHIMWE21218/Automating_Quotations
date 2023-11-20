using Automating_Quotations.Data;
using Automating_Quotations.Models.Travel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Automating_Quotations.Controllers
{
    [Route("api/TravelRegions")]
    [ApiController]
    public class TravelRegionController : ControllerBase
    {
        private readonly BkgiDataContext _context;

        public TravelRegionController(BkgiDataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TravelRegion>>> GetTravelRegions()
        {
            return await _context.TravelRegions.ToListAsync();
        }
    }
}

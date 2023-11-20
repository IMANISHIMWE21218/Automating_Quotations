using Automating_Quotations.Data;
using Automating_Quotations.Models.Travel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Automating_Quotations.Controllers
{
    [Route("api/TravelCoverPeriods")]
    [ApiController]
    public class TravelPeriodsController : ControllerBase
    {
        private readonly BkgiDataContext _context;

        public TravelPeriodsController(BkgiDataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TravelCoverperiod>>> GetTravelCoverperiods()
        {
            return await _context.TravelCoverperiods.ToListAsync();
        }
    }
}

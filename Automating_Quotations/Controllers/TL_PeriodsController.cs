using Automating_Quotations.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Automating_Quotations.Controllers
{
    [Route("api/CoveredPeriods")]
    [ApiController]
    public class TL_PeriodsController : ControllerBase
    {
        private readonly BkgiDataContext dbcontext;

        public TL_PeriodsController(BkgiDataContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var periods = await dbcontext.CoveragePeriods.Include(cp => cp.CoverageArea).ToListAsync();
            return Ok(periods);
        }
    }
}

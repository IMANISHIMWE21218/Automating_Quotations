using Automating_Quotations.Data;
using Automating_Quotations.Models.Motor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Automating_Quotations.Controllers
{
    [Route("api/MtTerritorialCoverLimit")]
    [ApiController]
    public class MtTerritorialCoverLimitController : ControllerBase
    {
        private readonly BkgiDataContext _context;

        public MtTerritorialCoverLimitController(BkgiDataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MtTerritorialCoverLimit>>> GetMtTerritorialCoverLimits()
        {
            return await _context.MtTerritorialCoverLimits.ToListAsync();
        }
    }
}

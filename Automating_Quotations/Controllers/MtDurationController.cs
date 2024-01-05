using Automating_Quotations.Data;
using Automating_Quotations.Models.Motor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Automating_Quotations.Controllers
{
    [Route("api/MtDuration")]
    [ApiController]
    public class MtDurationController : ControllerBase
    {
        private readonly BkgiDataContext _context;

        public MtDurationController(BkgiDataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MtDuration>>> GetMtDurations()
        {
            return await _context.MtDurations.ToListAsync();
        }
    }
}

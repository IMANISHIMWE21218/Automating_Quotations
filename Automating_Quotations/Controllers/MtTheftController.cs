using Automating_Quotations.Data;
using Automating_Quotations.Models.Motor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Automating_Quotations.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MtTheftController : ControllerBase
    {
        private readonly BkgiDataContext _context;

        public MtTheftController(BkgiDataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MtTheft>>> GetMtThefts()
        {
            return await _context.MtThefts.ToListAsync();
        }


    }
}

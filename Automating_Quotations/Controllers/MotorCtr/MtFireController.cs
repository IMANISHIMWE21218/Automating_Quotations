using Automating_Quotations.Data;
using Automating_Quotations.Models.Motor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Automating_Quotations.Controllers.MotorCtr
{
    [Route("api/MtFire")]
    [ApiController]
    public class MtFireController : ControllerBase
    {
        private readonly BkgiDataContext _context;

        public MtFireController(BkgiDataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MtFire>>> GetMtFires()
        {
            return await _context.MtFires.ToListAsync();
        }
    }
}

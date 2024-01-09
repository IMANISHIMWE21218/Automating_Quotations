using Automating_Quotations.Data;
using Automating_Quotations.Models.Motor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Automating_Quotations.Controllers.MotorCtr
{
    [Route("api/Thirdparty")]
    [ApiController]
    public class MtThirdpartyController : ControllerBase
    {
        private readonly BkgiDataContext _context;

        public MtThirdpartyController(BkgiDataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MtThirdparty>>> GetMtThirdparties()
        {
            return await _context.MtThirdparties.ToListAsync();
        }
    }
}

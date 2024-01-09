using Automating_Quotations.Data;
using Automating_Quotations.Models.Motor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Automating_Quotations.Controllers.MotorCtr
{
    [Route("api/MotorTypes")]
    [ApiController]
    public class MtMotorTypeController : ControllerBase
    {
        private readonly BkgiDataContext _context;

        public MtMotorTypeController(BkgiDataContext context)
        {

            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MtMotorType>>> GetMtMotorTypes()
        {
            return await _context.MtMotorTypes.ToListAsync();
        }
    }
}

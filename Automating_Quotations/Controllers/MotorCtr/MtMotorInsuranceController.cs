using Automating_Quotations.Data;
using Automating_Quotations.Models.Motor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Automating_Quotations.Controllers.MotorCtr
{
    [Route("api/[controller]")]
    [ApiController]
    public class MtMotorInsuranceController : ControllerBase
    {
        private readonly BkgiDataContext _context;

        public MtMotorInsuranceController(BkgiDataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MtMotoQuotationTable>>> GetMtMotoQuotationTables()
        {
            return Ok(await _context.MtMotoQuotationTables.ToListAsync());
        }

        [HttpPost]
        public IActionResult PostMotorInsurance([FromBody] MotorInsurance motorInsurance)
        {
            try
            {
                // 





                return Ok("MotorInsurance data posted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}

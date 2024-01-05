using Automating_Quotations.Data;
using Automating_Quotations.Models.Motor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Automating_Quotations.Controllers
{
    [Route("api/MtTarifOccupant")]
    [ApiController]
    public class MtTarifOccupantController : ControllerBase
    {
        private readonly BkgiDataContext _context;

        public MtTarifOccupantController(BkgiDataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MtTarifOccupant>>> GetMtTarifOccupants()
        {
            return await _context.MtTarifOccupants.ToListAsync();
        }
    }
}

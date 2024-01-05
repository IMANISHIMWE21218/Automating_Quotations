using Automating_Quotations.Data;
using Automating_Quotations.Models.Motor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Automating_Quotations.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MtTypeOfClientController : ControllerBase
    {
        private readonly BkgiDataContext _context;

        public MtTypeOfClientController(BkgiDataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MtTypeOfClient>>> GetMtTypeOfClients()
        {
            return await _context.MtTypeOfClients.ToListAsync();
        }
    }
}

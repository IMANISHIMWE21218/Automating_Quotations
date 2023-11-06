using Automating_Quotations.Data;
using Automating_Quotations.Models.Travel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Automating_Quotations.Controllers
{
    [Route("api/CoverageAreas")]
    [ApiController]
    public class TL_CoverageAreasController : ControllerBase
    {
        private readonly BkgiDataContext dbcontext;

        public TL_CoverageAreasController(BkgiDataContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var areas = await dbcontext.CoverageAreas.ToListAsync();

            return Ok(areas);
        }
    }
}

using Automating_Quotations.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Automating_Quotations.Controllers
{
    [Route("api/TravelPremiumDetails")]
    [ApiController]
    public class TL_PremiumDetailsController : ControllerBase
    {
        private readonly BkgiDataContext dbcontext;

        public TL_PremiumDetailsController(BkgiDataContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
           var  premiums = dbcontext.PremiumDetails.Include(pd => pd.CoveragePeriod).ThenInclude(navigationPropertyPath: cp => cp.CoverageArea).ToList();

            return Ok(premiums);
        }
    }
}

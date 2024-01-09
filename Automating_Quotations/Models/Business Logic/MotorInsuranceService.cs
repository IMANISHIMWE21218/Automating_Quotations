namespace Automating_Quotations.Models.Business_Logic
{
    public class MotorInsuranceService : IMotorInsuranceService

    {

        private readonly IConfiguration _configuration;

        public MotorInsuranceService(IConfiguration configuration) {
        _configuration = configuration;
        }

     

    }
}

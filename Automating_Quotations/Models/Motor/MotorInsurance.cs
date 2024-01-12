namespace Automating_Quotations.Models.Motor
{
    public class MotorInsurance
    {
        public int? MtMotorType { get; set; }
        public DateOnly? ManufactureDate { get; set; }
        public int? SeatCapacity { get; set; }
        public int? Occupant { get; set; }
        public bool? ThirdParty { get; set; }
        public bool? OwnDamage { get; set; }
        public bool? Theft { get; set; }
        public bool? Fire { get; set; }
        public int? TerritoryLimits { get; set; }
        public int? PeriodOfInsurance { get; set; }
        public decimal? ValueOfVehicle { get; set; } = 0;
        public int? TypeOfClient { get; set; }
    }
}

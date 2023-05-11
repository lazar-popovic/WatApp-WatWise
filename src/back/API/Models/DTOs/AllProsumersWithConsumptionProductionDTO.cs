using API.Models.Entity;

namespace API.Models.DTOs
{
    public class AllProsumersWithConsumptionProductionDTO
    {
        public int? Id { get; set; }
        public string? Email { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public bool? Verified { get; set; }
        public int? LocationId { get; set; }
        public Location? Location { get; set; }
        public double? CurrentConsumption { get; set; }
        public double? PredictedCurrentConsumption { get; set; }
        public double? CurrentProduction { get; set; }
        public double? PredictedCurrentProduction { get; set; }
        public int? ConsumingDevicesTurnedOn { get; set; }
        public int? ProducingDevicesTurnedOn { get; set; }
    }
}

namespace WebApi.Models
{
    public class CreateOrEditSpotRequest
    {
        public string Name { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public Guid ActivityId { get; set; }
        public string? Note { get; set; }
        public bool IsActive { get; set; }
    }
}


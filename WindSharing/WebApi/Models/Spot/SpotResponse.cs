using Core.Domain;

namespace WebApi.Models
{
    public class SpotResponse : BaseEntity
    {
        public string? Name { get; set; }
        public Guid ActivityId { get; set; }
        public string? Note { get; set; }
        public DateTime CreateDataTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsActive { get; set; }

        public SpotResponse(Spot spot)
        {
            Id=spot.Id;
            ActivityId = spot.ActivityId;
            Name = spot.Name;
            Note = spot.Note;
            Latitude = spot.Latitude;
            Longitude = spot.Longitude;
            CreateDataTime = spot.CreateDataTime;
            IsActive = spot.IsActive;
        }
    }

}

using Core.Domain;

namespace WebApi.Models
{

    public class SpotByIdResponse: BaseEntity
    {
        public string? Name { get; set; }
        public Guid ActivityId { get; set; }
        public string? Note { get; set; }
        public DateTime CreateDataTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ActivityName { get; set; }
        public SpotPhotoResponse[]? Photos { get; set; }
        public UserSpotDto[]? UserSpots { get; set; }
        public bool IsActive { get; set; }
        public SpotByIdResponse(Spot spot)
        {
            Id = spot.Id;
            ActivityId = spot.ActivityId;
            ActivityName = spot.Activity.Name;
            Name = spot.Name;
            Note = spot.Note;
            Latitude =spot.Latitude;
            Longitude = spot.Longitude;
            CreateDataTime = spot.CreateDataTime;
            Photos = spot.SpotPhotos?.Select(x => new SpotPhotoResponse(x)).ToArray();
            UserSpots = spot.UserSpots?.OrderByDescending(x => x.CreateDataTime).Select(x => new UserSpotDto(x)).ToArray();
            IsActive = spot.IsActive;
        }
    }
}

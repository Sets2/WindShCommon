namespace Core.Domain
{
    public class Spot: BaseEntity
    {
        public string? Name { get; set; }
        public string? Note { get; set; }
        public DateTime CreateDataTime { get; set; }
        public bool IsActive { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Guid ActivityId { get; set; }
        public virtual Activity Activity { get; set; } = null!;
        public virtual ICollection<UserSpot>? UserSpots { get; set; }
        public virtual ICollection<SpotPhoto>? SpotPhotos { get; set; }

    }
}

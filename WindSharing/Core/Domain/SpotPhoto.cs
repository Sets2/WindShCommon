namespace Core.Domain
{
    public class SpotPhoto : BaseEntity
    {
        public string FileName { get; set; } = null!;
        public string? Comment { get; set; }
        public DateTime CreateDataTime { get; set; }

        public Guid SpotId { get; set; }
        public virtual Spot Spot { get; set; } = null!;
    }
}

namespace Core.Domain;

public class UserSpot : BaseEntity
{
    public Guid UserWindId { get; set; }
    public virtual UserWind UserWind { get; set; } = null!;
    public Guid SpotId { get; set; }
    public virtual Spot Spot { get; set; }=null!;
    public DateTime CreateDataTime { get; set; }
    public string? Comment { get; set; }
}
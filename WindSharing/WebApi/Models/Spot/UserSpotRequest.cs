namespace WebApi.Models
{
    public class UserSpotRequest
    {
        public Guid SpotId { get; set; }
        public Guid UserId { get; set; }
        public string? Comment { get; set; }
    }
}


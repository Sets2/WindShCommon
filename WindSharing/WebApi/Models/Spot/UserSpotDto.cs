using Core.Domain;

namespace WebApi.Models
{

    public class UserSpotDto : BaseEntity
    {
        public string? UserName { get; set; }
        public string? Comment { get; set; }
        public DateTime CreateDataTime { get; set; }
       
        public UserSpotDto(UserSpot spot)
        {
            UserName = spot.UserWind.UserName;
            Comment = spot.Comment;
            CreateDataTime = spot.CreateDataTime;
        }
    }
}

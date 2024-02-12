using Core.Domain;

namespace WebApi.Models
{
    public class UserWindResponse : BaseEntity
    {
        public string UserName { get; set; } = null!;
        public string? Fio { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int? Age { get; set; }
        public string? About { get; set; }
        public string? FotoFileName { get; set; }
        public List<string>? Roles { get; set; }
        public bool? IsActive { get; set; }

        public UserWindResponse(){}

        public UserWindResponse(UserWind userWind,List<string>? roles = null)
        {
            Id = userWind.Id;
            UserName = userWind.UserName;
            Fio = userWind.Fio;
            Email = userWind.Email;
            Age = userWind.Age;
            About = userWind.About;
            FotoFileName = userWind.FotoFileName;
            Roles = roles;
            IsActive = userWind.IsActive;
        }
    }

}

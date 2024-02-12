using Core.Domain;
using Microsoft.AspNetCore.Identity;

namespace WebApi.Models
{
    public class UserRolesResponse : BaseEntity
    {
        public string UserName { get; set; }
        public string Fio { get; set; }
        public IList<string>? Roles { get; set; }

        public UserRolesResponse(UserWind user, IList<string>? roles)
        {
            Id = user.Id;
            UserName = user.UserName;
            Fio = user.Fio;
            Roles = roles;
        }
    }

}

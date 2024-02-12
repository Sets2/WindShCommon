using Microsoft.AspNetCore.Identity;

namespace Core.Domain
{
    public class UserWind: IdentityUser<Guid>, IBaseEntity
    {
        public string? Fio { get; set; } = null!;
//        public string Email { get; set; } = null!;
        public int? Age { get; set; }
        public string? About { get; set; }
        public string? FotoFileName { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<UserSpot>? UserSpots { get; set; }
    }
}

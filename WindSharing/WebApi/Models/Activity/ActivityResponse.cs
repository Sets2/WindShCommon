using Core.Domain;

namespace WebApi.Models
{
    public class ActivityResponse : BaseEntity
    {
        public string Name { get; set; }
        public string? IconName { get; set; }

        public ActivityResponse(Activity activity)
        {
            Id = activity.Id;
            Name = activity.Name;
            IconName = activity.IconName;           
        }
    }

}

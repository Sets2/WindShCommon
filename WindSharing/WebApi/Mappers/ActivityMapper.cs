using Core.Domain;
using WebApi.Models;

namespace WebApi.Mappers
{
    public static class ActivityMapper
    {
        public static Activity MapFromModel(CreateOrEditActivityRequest request, Activity? activity = null)
        {
            if (activity == null)
            {
                activity = new Activity
                {
                    Id = Guid.NewGuid(),
                };
            }
            activity.Name = request.Name;
            activity.IconName = request.IconName;

            return activity;
        }
    }
}
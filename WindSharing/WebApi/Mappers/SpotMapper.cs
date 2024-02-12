using Core.Domain;
using WebApi.Models;

namespace WebApi.Mappers
{
    public static class SpotMapper
    {
        public static Spot MapFromModel(CreateOrEditSpotRequest request, Spot? spot = null)
        {
            if (spot == null)
            {
                spot = new Spot
                {
                    Id = Guid.NewGuid(),
                };
            }
            spot.Name = request.Name;
            spot.ActivityId = request.ActivityId;
            spot.Note = request.Note;
            spot.CreateDataTime = DateTime.Now;
            spot.IsActive = request.IsActive;
            spot.Latitude= request.Latitude;
            spot.Longitude= request.Longitude;
            return spot;
        }
    }
}

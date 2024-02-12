using Core.Domain;
using WebApi.Models;

namespace WebApi.Mappers
{
    public static class SpotPhotoMapper
    {
        public static SpotPhoto MapFromModel(CreateOrEditSpotPhotoRequest request, SpotPhoto? spotPhoto = null)
        {
            if (spotPhoto == null)
            {
                spotPhoto = new SpotPhoto
                {
                    Id = Guid.NewGuid(),
                };
            }
            spotPhoto.FileName = request.FileName;
            spotPhoto.Comment = request.Comment;
            spotPhoto.CreateDataTime = request.CreateDataTime.HasValue ? request.CreateDataTime.Value : 
                DateTime.Now;
            spotPhoto.SpotId = request.SpotId;

            return spotPhoto;
        }
    }
}
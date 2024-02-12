using Core.Domain;
using Nest;
using System.Diagnostics;

namespace WebApi.Models
{
    public class SpotPhotoResponse : BaseEntity
    {
        public string FileName { get; set; } = null!;
        public string? Comment { get; set; }
        public DateTime CreateDataTime { get; set; }
        public Guid SpotId { get; set; }

        public SpotPhotoResponse(SpotPhoto spotPhoto)
        {
            Id = spotPhoto.Id;
            FileName = spotPhoto.FileName;
            Comment = spotPhoto.Comment;
            CreateDataTime = spotPhoto.CreateDataTime;
            SpotId = spotPhoto.SpotId;
        }
    }

}

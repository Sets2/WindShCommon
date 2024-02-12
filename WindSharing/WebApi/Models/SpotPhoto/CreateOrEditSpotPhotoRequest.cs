using Core.Domain;
using Nest;
using System.Diagnostics;

namespace WebApi.Models
{
    public class CreateOrEditSpotPhotoRequest
    {
        public string FileName { get; set; } = null!;
        public string? Comment { get; set; }
        public DateTime? CreateDataTime { get; set; }
        public Guid SpotId { get; set; }
    }

}

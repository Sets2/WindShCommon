namespace WebApi.Models
{
    public class CreateOrEditUserWindRequest
    {
        public Guid id { get; set; }
        public string UserName { get; set; } = null!;
        public string? Fio { get; set; }
        public string Email { get; set; } = null!;
        public int? Age { get; set; }
        public string? About { get; set; }
        public string? FotoFileName { get; set; }
        public bool? IsActive { get; set; }
    }

}


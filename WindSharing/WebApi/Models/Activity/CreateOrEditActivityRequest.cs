namespace WebApi.Models
{
    public class CreateOrEditActivityRequest
    {
        public string Name { get; set; } = null!;
        public string? IconName { get; set; }
    }

}

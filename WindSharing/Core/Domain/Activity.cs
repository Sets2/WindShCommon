namespace Core.Domain
{
    public class Activity: BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? IconName { get; set; }

        public virtual ICollection<Spot>? Spots { get; set; }
    }
}

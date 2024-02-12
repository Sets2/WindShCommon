namespace Core.Domain
{
    public class Log
    {
        public long Id { get; set; }
        public string Application { get; set; } = null!;
        public DateTime Logged { get; set; }
        public string Level { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string Logger { get; set; } = null!;
        public string? Callsite { get; set; }
        public string? Exception { get; set; }
        public string? Username { get; set; }
        public string? ClientIp { get; set; }
    }
}

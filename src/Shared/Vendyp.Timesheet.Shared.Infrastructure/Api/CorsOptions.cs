namespace Vendyp.Timesheet.Shared.Infrastructure.Api
{
    internal sealed class CorsOptions
    {
        public bool AllowCredentials { get; set; }
        public List<string> AllowedOrigins { get; } = new();
        public List<string> AllowedMethods { get; } = new();
        public List<string> AllowedHeaders { get; } = new();
        public List<string> ExposedHeaders { get; } = new();
    }
}
namespace Polemonium.Api.Web.Common
{
    public class PolemoniumOptions
    {
        public string DbConnectionString { get; set; }
        public string JwtIssuer { get; set; }
        public string JwtAudience { get; set; }
        public string JwtKey { get; set; }
    }
}

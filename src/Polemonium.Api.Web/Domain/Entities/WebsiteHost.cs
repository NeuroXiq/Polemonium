namespace Polemonium.Api.Web.Domain.Entities
{
    public class WebsiteHost : EntityBase
    {
        public string DnsName { get; set; }

        public WebsiteHost() { }

        public WebsiteHost(string dnsName)
        {
            DnsName = dnsName;
        }
    }
}

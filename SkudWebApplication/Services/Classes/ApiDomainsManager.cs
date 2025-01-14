using SkudWebApplication.Services.Interfaces;

namespace SkudWebApplication.Services.Classes
{
    public class ApiDomainsManager : IApiDomainsManager
    {
        private readonly Dictionary<string, string> _domains = new Dictionary<string, string>()
        {
            {"own-iis-local", "http://localhost:5665" },
            {"own-local", "http://localhost:5036" },
            {"own-prod", "http://192.168.1.20:5665" }
        };

        public string GetDomain(string domainName)
        {
            return _domains.First(x => x.Key.Equals(domainName)).Value;
        }
    }
}

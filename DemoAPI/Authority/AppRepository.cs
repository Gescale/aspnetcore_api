using Microsoft.AspNetCore.DataProtection;

namespace DemoAPI.Authority
{
    public class AppRepository
    {
        private static List<Application> _applications = new List<Application>()
        {
            new Application
            {
                ApplicationId = 1,
                ApplicationName = "MVCWebApp",
                ClientId = "F04CC7CB-0A4C-485A-8FFE-0F8BC3593A16",
                Secret = "BDF2A408-CED6-403F-A625-676E84078EB2",
                Scopes = "read, write, delete"
                //Used SQL Server to generate the above ClientId and Secret for better randomness and security
                //Command: SELECT NEWID() AS RandomGUID;
            }
        };

        public static bool Authenticate(string clientId, string secret)
        {
            return _applications.Any(a => a.ClientId == clientId && a.Secret == secret);
        }

        public static Application? GetApplicationByClientId(string clientId)
        {
            return _applications.FirstOrDefault(a => a.ClientId == clientId);
        }
    }
}

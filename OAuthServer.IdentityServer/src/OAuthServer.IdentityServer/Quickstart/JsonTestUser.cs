using System.Collections.Generic;

namespace OAuthServer.IdentityServer.Quickstart
{
    public class JsonTestUser
    {
        public string SubjectId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ProviderName { get; set; }
        public string ProviderSubjectId { get; set; }
        public bool IsActive { get; set; } = true;
        public IList<JsonClaim> Claims { get; set; }
    }
}
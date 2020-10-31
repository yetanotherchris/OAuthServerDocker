using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Text.Json;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace OAuthServer.Quickstart
{
    public static class JsonConfigReader
    {
        private static List<TestUser> _users;
        public static List<TestUser> Users
        {
            get
            {
                if (_users == null)
                {
                    string usersText = File.ReadAllText("users.json");
                    var testUsers = JsonSerializer.Deserialize<List<JsonTestUser>>(usersText, new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    _users = ConvertToTestUsers(testUsers);
                }

                return _users;
            }
        }

        private static JsonClientsContainer _container;

        private static List<ApiScope> _apiScopes;
        public static List<ApiScope> ApiScopes
        {
            get
            {
                if (_apiScopes == null)
                {
                    if (_container == null)
                    {
                        string containerText = File.ReadAllText("clients.json");
                        _container = JsonSerializer.Deserialize<JsonClientsContainer>(containerText, new JsonSerializerOptions()
                        {
                            PropertyNameCaseInsensitive = true
                        });
                    }

                    _apiScopes = new List<ApiScope>();
                    foreach (string name in _container.ApiScopes)
                    {
                        _apiScopes.Add(new ApiScope(name));
                    }
                }

                return _apiScopes;
            }
        }

        private static List<TestUser> ConvertToTestUsers(IList<JsonTestUser> jsonTestUsers)
        {
            var list = new List<TestUser>();
            foreach (var jsonUser in jsonTestUsers)
            {
                var testUser = new TestUser()
                {
                    SubjectId = jsonUser.SubjectId,
                    Username = jsonUser.Username,
                    Password = jsonUser.Password,
                    IsActive = jsonUser.IsActive,
                    ProviderName = jsonUser.ProviderName,
                    ProviderSubjectId = jsonUser.ProviderSubjectId
                };

                foreach (var jsonClaim in jsonUser.Claims)
                {
                    var claim = new Claim(jsonClaim.Type, jsonClaim.Value);
                    testUser.Claims.Add(claim);
                }
                list.Add(testUser);
            }

            return list;
        }
    }
}
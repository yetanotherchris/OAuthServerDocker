// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Text.Json;
using IdentityServer4.Test;

namespace OAuthServer.Quickstart
{
    public class TestUsers
    {
        private static List<TestUser> _users;
        public static List<TestUser> Users
        {
            get
            {
                if (_users == null)
                {
                    string usersText = File.ReadAllText("users.json");
                    var testUsers = JsonSerializer.Deserialize<List<JsonTestUser>>(usersText);
                    _users = ConvertToTestUsers(testUsers);
                }

                return _users;
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
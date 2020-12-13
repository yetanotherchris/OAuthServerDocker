using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace OAuthServer.OpenIdDict
{
    public static class CertHelper
    {
        // dotnet dev-certs https -ep $pwd/selfsigned.pem --format Pem -np
        public static X509Certificate2 GetCertificate()
        {
            byte[] publicPemBytes = File.ReadAllBytes("certs/selfsigned.pem");
            using var publicX509 = new X509Certificate2(publicPemBytes);
            var privateKeyText = File.ReadAllText("certs/selfsigned.key");
            var privateKeyBlocks = privateKeyText.Split("-", StringSplitOptions.RemoveEmptyEntries);
            var privateKeyBytes = Convert.FromBase64String(privateKeyBlocks[1]);
    
            using RSA rsa = RSA.Create();
            if (privateKeyBlocks[0] == "BEGIN PRIVATE KEY")
            {
                rsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);
            }
            else if (privateKeyBlocks[0] == "BEGIN RSA PRIVATE KEY")
            {
                rsa.ImportRSAPrivateKey(privateKeyBytes, out _);
            }
            var keyPair = publicX509.CopyWithPrivateKey(rsa);
            return keyPair;
        }
    }
}
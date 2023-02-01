using System;
using System.Security.Cryptography;
using GameStore.BusinessLogicLayer.Abstract.Infrastructure;

namespace GameStore.BusinessLogicLayer.Infrastructure
{
    public class HashPasswordManager : IHashPasswordManager
    {
        public string EncryptPassword(string password)
        {
            var salt = new byte[16];
            var cryptoServiceProvider = new RNGCryptoServiceProvider();
            cryptoServiceProvider.GetBytes(salt);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            var hash = pbkdf2.GetBytes(20);
            var hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            var passwordHash = Convert.ToBase64String(hashBytes);

            return passwordHash;
        }

        public bool VerifyPassword(string passwordInput, string passwordHash)
        {
            var hashBytes = Convert.FromBase64String(passwordHash);
            var salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            var pbkdf2 = new Rfc2898DeriveBytes(passwordInput, salt, 10000);
            var hash = pbkdf2.GetBytes(20);
            for (var i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
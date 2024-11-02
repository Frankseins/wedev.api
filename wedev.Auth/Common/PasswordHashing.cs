using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace wedev.Auth.Common;


    public class PasswordHashing
    {
        public byte[] CreateSalt()
        {
            var buffer = new byte[129];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(buffer);
            return buffer;
        }

        public byte[] HashPassword(string password, byte[] salt)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));
            argon2.Salt = salt;
            argon2.DegreeOfParallelism = 16;
            argon2.Iterations = 8;
            argon2.MemorySize = 8192;
            return argon2.GetBytes(128);
        }

        public bool VerifyHash(string password, byte[] salt, byte[] hash)
        {
            var newHash = HashPassword(password, salt);
            return hash.SequenceEqual(newHash);
        }
    }





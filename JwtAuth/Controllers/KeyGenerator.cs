using System;
using System.Security.Cryptography;

namespace JwtAuth.Utilities
{
    public static class KeyGenerator
    {
        public static byte[] GenerateKey(int keySizeInBits)
        {
            using var rng = RandomNumberGenerator.Create();
            var key = new byte[keySizeInBits / 8];
            rng.GetBytes(key);
            return key;
        }
    }
}

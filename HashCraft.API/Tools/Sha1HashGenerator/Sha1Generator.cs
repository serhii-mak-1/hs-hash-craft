using HashCraft.API.Tools.Sha1HashGenerator.Exceptions;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HashCraft.API.Tools.Sha1HashGenerator
{
    public class Sha1Generator : IHashGenerator
    {
        public string GenerateHash(EncodeStyle encode)
        {
            try
            {
                byte[] randomBytes = new byte[20];
                using RandomNumberGenerator rng = RandomNumberGenerator.Create();
                rng.GetBytes(randomBytes);

                using SHA1 sha1 = SHA1.Create();

                byte[] hashBytes = sha1.ComputeHash(randomBytes);

                if (encode.Equals(EncodeStyle.Base64))
                {
                    return Convert.ToBase64String(hashBytes);
                }

                StringBuilder hashString = new();
                foreach (byte b in hashBytes)
                {
                    hashString.Append(b.ToString(GetFormat(encode)));
                }

                return hashString.ToString();
            }
            catch (Exception ex)
            {
                throw new GenerateHashException(ex);
            }
        }

        public string[] GenerateHashBatch(int size, EncodeStyle encode)
        {
            string[] hashArray = new string[size];

            Parallel.For(0, size, i =>
            {
                hashArray[i] = GenerateHash(encode);
            });

            return hashArray;
        }

        private static string GetFormat(EncodeStyle encode)
        {
            return encode switch
            {
                EncodeStyle.Dig => "D3",
                EncodeStyle.Hex => "X2",
                _ => throw new FormatException(),
            };
        }
    }
}

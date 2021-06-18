using System;
using System.Security.Cryptography;
using System.Text;

namespace authentication_helper
{
    public interface IEncryptionHelper
    {
        string CreateHash(byte[] data, string hashAlgorithm = "SHA1");
        string CreatePasswordHash(string password, string saltkey, string passwordFormat = "SHA1");
    }

    public class EncryptionHelper : IEncryptionHelper
    {
        public virtual string CreatePasswordHash(string password, string saltkey, string passwordFormat = "SHA1")
        {
            return CreateHash(Encoding.UTF8.GetBytes(String.Concat(password, saltkey)), passwordFormat);
        }

        public virtual string CreateHash(byte[] data, string hashAlgorithm = "SHA1")
        {
            if (String.IsNullOrEmpty(hashAlgorithm))
                hashAlgorithm = "SHA1";

            var algorithm = HashAlgorithm.Create(hashAlgorithm);
            if (algorithm == null)
                throw new ArgumentException("Unrecognized hash name");

            var hashByteArray = algorithm.ComputeHash(data);
            return BitConverter.ToString(hashByteArray).Replace("-", "");
        }
    }
}

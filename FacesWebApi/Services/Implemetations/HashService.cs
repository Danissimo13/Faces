using FacesWebApi.Services.Abstractions;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FacesWebApi.Services.Implemetations
{
    public class HashService : IHashService
    {
        public byte[] Key { get; }

        private HMACSHA256 sha;

        public HashService()
        {
            using(FileStream reader = new FileStream("key.snk", FileMode.Open))
            {
                Key = new byte[reader.Length];
                reader.Read(Key, 0, Key.Length);
            }

            sha = new HMACSHA256(Key);
        }

        public byte[] GetHash(string str)
        {
            return sha.ComputeHash(Encoding.UTF8.GetBytes(str));
        }

        public byte[] GetHash(byte[] buffer)
        {
            return sha.ComputeHash(buffer);
        }
    }
}

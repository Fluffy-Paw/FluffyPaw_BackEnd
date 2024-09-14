using FluffyPaw_Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Infrastructure.Hashing
{
    public class Hash : IHashing
    {
        public string SHA512Hash(string text)
        {
            SHA512 sha512 = SHA512.Create();
            byte[] input = Encoding.ASCII.GetBytes(text);
            byte[] hashBytes = sha512.ComputeHash(input);
            return Convert.ToHexString(hashBytes);
        }
    }
}

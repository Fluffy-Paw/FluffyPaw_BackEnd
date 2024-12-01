using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Domain.Interfaces
{
    public interface IHashing
    {
        string SHA512Hash(string text);
        string GenerateCode();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.WalletRequest
{
    public class DenyWithdrawRequest
    {
        public long Id {  get; set; }

        public string Description { get; set; }
    }
}

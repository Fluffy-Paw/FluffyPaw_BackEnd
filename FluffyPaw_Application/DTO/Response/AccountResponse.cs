using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response
{
    public class AccountResponse
    {
        public long UserId {  get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public DateTimeOffset? Dob { get; set; }
        public string? Reputation { get; set; }
        public int Status { get; set; }
    }
}

using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response
{
    public class AccountResponse : IMapFrom<Account>, IMapFrom<Brand>, IMapFrom<Store>, IMapFrom<PetOwner>
    {
        public long AccountId {  get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Avatar { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public DateTimeOffset? Dob { get; set; }
        public string? Reputation { get; set; }
        public int Status { get; set; }
    }
}

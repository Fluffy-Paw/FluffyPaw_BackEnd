using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.NotificationRequest
{
    public class NotificationRequest : IMapFrom<Notification>, IMapFrom<Account>
    {
        [Required(ErrorMessage = "Vui lòng nhập thông tin user cần gửi.")]
        public long ReceiverId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên thông báo.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập loại thông báo.")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tin nhắn thông báo.")]
        public string Description { get; set; }

        public long ReferenceId { get; set; }
    }
}

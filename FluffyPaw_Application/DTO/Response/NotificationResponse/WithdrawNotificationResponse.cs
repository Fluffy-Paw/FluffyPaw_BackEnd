using AutoMapper;
using FluffyPaw_Application.DTO.Response.ServiceResponse;
using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response.NotificationResponse
{
    public class WithdrawNotificationResponse : IMapFrom<Notification>
    {
        public long Id { get; set; }

        public string SenderName { get; set; }

        public double amount { get; set; }

        public DateTimeOffset CreateDate { get; set; }

        public string bankName { get; set; }

        public string number { get; set; }

        public string? qr {  get; set; }

        public string Status { get; set; }

    }
}

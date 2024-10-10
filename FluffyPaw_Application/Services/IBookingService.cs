using FluffyPaw_Application.DTO.Response.BookingResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.Services
{
    public interface IBookingService
    {
        Task<BookingResponse> GetBookingById(long id);
    }
}

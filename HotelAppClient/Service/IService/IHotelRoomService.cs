using DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelAppClient.Service.IService
{
    public interface IHotelRoomService
    {
        public Task<IEnumerable<HotelRoomDTO>> GetHotelRooms(string CheckInDate, string CheckOutDate);
        public Task<HotelRoomDTO> GetHotelRoomDetails(int roomId, string CheckInDate, string CheckOutDate);
    }
}

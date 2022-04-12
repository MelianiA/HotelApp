using DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.IRepository
{
    public interface IHotelRoomRepository
    {
        public Task<HotelRoomDTO> CreateHotelRoom(HotelRoomDTO hotelRoomDTO);
        public Task<HotelRoomDTO> UpdateHotelRoom(int roomId, HotelRoomDTO hotelRoomDTO);
        public Task<HotelRoomDTO> GetHotelRoom(int roomId, string CheckInDate = null,
                                                                string CheckOutDate = null);
        public Task<IEnumerable<HotelRoomDTO>> GetAllHotelRooms(string CheckInDate = null, 
                                                                string CheckOutDate = null);
        public Task<bool> IsRoomUnique(string name, int roomId = 0);
        public Task<int> DeleteHotelRoom(int roomId);
    }
}

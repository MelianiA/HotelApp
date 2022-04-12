using DTOS;
using HotelAppClient.Service.IService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HotelAppClient.Service
{
    public class HotelRoomService : IHotelRoomService
    {
        private readonly HttpClient httpClient;

        public HotelRoomService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<HotelRoomDTO> GetHotelRoomDetails(int roomId, string CheckInDate, string CheckOutDate)
        {
            var response = await httpClient
               .GetAsync(
                $"api/hotelroom/{roomId}?checkInDate={CheckInDate}&checkOutDate={CheckOutDate}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var room = JsonConvert.DeserializeObject<HotelRoomDTO>(content);
                return room;
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                var error = JsonConvert.DeserializeObject<ErrorDTO>(content);
                throw new Exception(error.ErrorMessage);
            }
        }

        public async Task<IEnumerable<HotelRoomDTO>> GetHotelRooms(string CheckInDate, string CheckOutDate)
        {
            var response = await httpClient.
                GetAsync($"api/hotelroom?checkInDate={CheckInDate}&checkOutDate={CheckOutDate}");
            var content = await response.Content.ReadAsStringAsync();
            var rooms = JsonConvert.DeserializeObject<IEnumerable<HotelRoomDTO >>(content);
            return rooms;
        }
    }
}

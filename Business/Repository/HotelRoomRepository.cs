using AutoMapper;
using Business.Repository.IRepository;
using DataAccess.Data;
using DataAccess.Data.Models;
using DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository
{
    internal class HotelRoomRepository : IHotelRoomRepository
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public HotelRoomRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        public async Task<HotelRoomDTO> CreateHotelRoom(HotelRoomDTO hotelRoomDTO)
        {
            HotelRoom hotelRoom = mapper.Map<HotelRoomDTO, HotelRoom>(hotelRoomDTO);
            hotelRoom.CreatedDate = DateTime.Now;
            hotelRoom.CreatedBy = "";
            var AddedHotelRoom = await dbContext.HotelRooms.AddAsync(hotelRoom);
            await dbContext.SaveChangesAsync();
            return mapper.Map<HotelRoom, HotelRoomDTO>(AddedHotelRoom.Entity);
        }

        public Task<int> DeleteHotelRoom(int roomId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<HotelRoomDTO>> GetAllHotelRooms()
        {
            throw new NotImplementedException();
        }

        public Task<HotelRoomDTO> GetHotelRoom(int roomId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsRoomUnique(string name)
        {
            throw new NotImplementedException();
        }

        public Task<HotelRoomDTO> UpdateHotelRoom(int roomId, HotelRoomDTO hotelRoomDTO)
        {
            throw new NotImplementedException();
        }
    }
}

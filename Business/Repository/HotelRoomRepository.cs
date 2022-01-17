using AutoMapper;
using Business.Repository.IRepository;
using DataAccess.Data;
using DataAccess.Data.Models;
using DTOS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository
{
    public class HotelRoomRepository : IHotelRoomRepository
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
            var addedHotelRoom = await dbContext.HotelRooms.AddAsync(hotelRoom);
            await dbContext.SaveChangesAsync();
            return mapper.Map<HotelRoom, HotelRoomDTO>(addedHotelRoom.Entity);
        }

        //Update Room
        public async Task<HotelRoomDTO> UpdateHotelRoom(int roomId, HotelRoomDTO hotelRoomDTO)
        {
            try
            {
                if (roomId == hotelRoomDTO.Id)
                {
                    //valid
                    HotelRoom roomToUpdate = await dbContext.HotelRooms.FindAsync(roomId);
                    HotelRoom room = mapper.Map<HotelRoomDTO, HotelRoom>(hotelRoomDTO, roomToUpdate);
                    room.UpdatedBy = "";
                    room.UpdatedDate = DateTime.Now;
                    var updatedRoom = dbContext.HotelRooms.Update(room);
                    await dbContext.SaveChangesAsync();
                    return mapper.Map<HotelRoom, HotelRoomDTO>(updatedRoom.Entity);
                }
                else
                {
                    //invalid
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //Get Room Details
        public async Task<HotelRoomDTO> GetHotelRoom(int roomId)
        {
            try
            {
                HotelRoomDTO roomDetailsDTO = mapper.Map<HotelRoom, HotelRoomDTO>(
                    await dbContext.HotelRooms.FirstOrDefaultAsync(r => r.Id == roomId));

                return roomDetailsDTO;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //Get All Rooms Details
        public async Task<IEnumerable<HotelRoomDTO>> GetAllHotelRooms()
        {
            try
            {
                IEnumerable<HotelRoomDTO> roomsDetailsDTOs =
                            mapper.Map<IEnumerable<HotelRoom>, IEnumerable<HotelRoomDTO>>
                            (dbContext.HotelRooms);

                return roomsDetailsDTOs;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //Check If Room Name Is Unique
        public async Task<bool> IsRoomUnique(string name, int roomId=0)
        {
            if (roomId == 0)
            {
                HotelRoom hotelRoom = await dbContext.HotelRooms.FirstOrDefaultAsync(r => r.Name.ToLower() == name.ToLower());
                if (hotelRoom == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                HotelRoom hotelRoom = await dbContext.HotelRooms.FirstOrDefaultAsync(
                    r => r.Name.ToLower() == name.ToLower() && r.Id != roomId);
                if (hotelRoom == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            
        }

        //Delete Room
        public async Task<int> DeleteHotelRoom(int roomId)
        {
            var roomToDelete = await dbContext.HotelRooms.FindAsync(roomId);
            if (roomToDelete != null)
            {
                dbContext.HotelRooms.Remove(roomToDelete);
                return await dbContext.SaveChangesAsync();//return number of deleted records 
            }
            return 0;
        }
    }
}

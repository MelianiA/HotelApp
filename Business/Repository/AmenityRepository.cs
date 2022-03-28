using AutoMapper;
using DataAccess.Data.Models;
using DataAccess.Data;
using DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Business.Repository
{
    public class AmenityRepository : IAmenityRepository
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;

        public AmenityRepository(ApplicationDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<HotelAmenityDTO> CreateHotelAmenity(HotelAmenityDTO hotelAmenity)
        {
            var amenity = mapper.Map<HotelAmenityDTO, HotelAmenity>(hotelAmenity);
            amenity.CreatedBy = "";
            amenity.CreatedDate = DateTime.Now;
            var addedHotelAmenity = await db.HotelAmenities.AddAsync(amenity);
            await db.SaveChangesAsync();
            return mapper.Map<HotelAmenity, HotelAmenityDTO>(addedHotelAmenity.Entity);
        }

        public async Task<HotelAmenityDTO> UpdateHotelAmenity(int amenityId, HotelAmenityDTO hotelAmenity)
        {
            var amenityDetails = await db.HotelAmenities.FindAsync(amenityId);
            var amenity = mapper.Map<HotelAmenityDTO, HotelAmenity>(hotelAmenity, amenityDetails);
            amenity.UpdatedBy = "";
            amenity.UpdatedDate = DateTime.UtcNow;
            var updatedamenity = db.HotelAmenities.Update(amenity);
            await db.SaveChangesAsync();
            return mapper.Map<HotelAmenity, HotelAmenityDTO>(updatedamenity.Entity);
        }

        public async Task<int> DeleteHotelAmenity(int amenityId)
        {
            var amenityDetails = await db.HotelAmenities.FindAsync(amenityId);
            if (amenityDetails != null)
            {
                db.HotelAmenities.Remove(amenityDetails);
                return await db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<IEnumerable<HotelAmenityDTO>> GetAllHotelAmenity()
        {
            return mapper.Map<IEnumerable<HotelAmenity>, IEnumerable<HotelAmenityDTO>>(await db.HotelAmenities.ToListAsync());
        }

        public async Task<HotelAmenityDTO> GetHotelAmenity(int amenityId)
        {
            var amenityData = await db.HotelAmenities
                .FirstOrDefaultAsync(x => x.Id == amenityId);

            if (amenityData == null)
            {
                return null;
            }
            return mapper.Map<HotelAmenity, HotelAmenityDTO>(amenityData);
        }

        //Check If Amenity Name Is Unique
        public async Task<bool> IsAmenityUnique(string name, int amenityId = 0)
        {
            if (amenityId == 0)
            {
                //Create
                HotelAmenity hotelAmenity = await db.HotelAmenities
                .FirstOrDefaultAsync(a => a.Name.ToLower() == name.ToLower());
                if (hotelAmenity == null)
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
                //Update
                HotelAmenity hotelAmenity = await db.HotelAmenities
               .FirstOrDefaultAsync(a => a.Name.ToLower() == name.ToLower()
                && a.Id != amenityId
                );
                if (hotelAmenity == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

        }
    }
}

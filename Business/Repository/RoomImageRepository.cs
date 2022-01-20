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
    public class RoomImageRepository : IRoomImageRepository
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;

        public RoomImageRepository(ApplicationDbContext context, IMapper mapper) 
        {
            this.db = context;
            this.mapper = mapper;
        }

        public async Task<int> CreateRoomImage(RoomImageDTO imageDTO)
        {
            var image = mapper.Map<RoomImageDTO, RoomImage>(imageDTO);
            await db.RoomImages.AddAsync(image);
            return await db.SaveChangesAsync();
        }

        public async Task<int> DeleteImageByImageUrl(string imageUrl)
        {
            var allImages = await db.RoomImages.FirstOrDefaultAsync(i => i.ImageUrl.ToLower() == imageUrl.ToLower());
            if (allImages == null)
            {
                return 0;
            }
            db.RoomImages.Remove(allImages);
            return await db.SaveChangesAsync();

        }

        public async Task<int> DeleteRoomImageByImageId(int imageId)
        {
            var image = await db.RoomImages.FindAsync(imageId);
            db.RoomImages.Remove(image);
            return await db.SaveChangesAsync();
        }

        public async Task<int> DeleteRoomImageByRoomId(int roomId)
        {
            var imageList = await db.RoomImages.Where(i => i.RoomId == roomId).ToListAsync();
            db.RoomImages.RemoveRange(imageList);
            return await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<RoomImageDTO>> GetRoomImages(int roomId)
        {
            return mapper.Map<IEnumerable<RoomImage>, IEnumerable<RoomImageDTO>>(await db.RoomImages.Where(i => i.RoomId == roomId).ToListAsync());
        }
    }
}

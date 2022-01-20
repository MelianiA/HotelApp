using DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.IRepository
{
    public interface IRoomImageRepository
    {
        public Task<int> CreateRoomImage(RoomImageDTO image);
        public Task<int> DeleteRoomImageByImageId(int imageId);
        public Task<int> DeleteRoomImageByRoomId(int roomId);
        public Task<int> DeleteImageByImageUrl(string imageUrl);
        public Task<IEnumerable<RoomImageDTO>> GetRoomImages(int roomId);
    }
}

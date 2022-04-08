using Business.Repository.IRepository;
using DTOS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HotelAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelRoomController : ControllerBase
    {
        private readonly IHotelRoomRepository hotelRoomRepository;

        public HotelRoomController(IHotelRoomRepository hotelRoomRepository)
        {
            this.hotelRoomRepository = hotelRoomRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetHotelRooms()
        {
            var allRooms = await hotelRoomRepository.GetAllHotelRooms();
            return Ok(allRooms);
        }

        [HttpGet("{roomId}")]
        public async Task<IActionResult> GetHotelRoom(int? roomId)
        {
            if (roomId is null || roomId.Equals(0))
            {
                return BadRequest(new ErrorDTO
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "Invalid Room Id"
                });
            }
            var roomDetails = await hotelRoomRepository.GetHotelRoom(roomId.Value);
            if (roomDetails is null || roomDetails.Equals(0))
            {
                return BadRequest(new ErrorDTO
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorMessage = "Room Not found"
                });
            }

            return Ok(roomDetails);
        }

    }
}

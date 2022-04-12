using Business.Repository.IRepository;
using Common;
using DTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
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
        public async Task<IActionResult> GetHotelRooms(string checkInDate = null,
                                                                string checkOutDate = null)
        {
            if (string.IsNullOrEmpty(checkInDate) || string.IsNullOrEmpty(checkOutDate))
            {
                return BadRequest(new ErrorDTO()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "All parameters need to be supplied"
                });
            }
            if (!DateTime.TryParseExact(checkInDate, "MM/dd/yyyy",
                                                    CultureInfo.InvariantCulture,
                                                    DateTimeStyles.None,
                                                    out DateTime dtCheckInDate)
                )
            {
                return BadRequest(new ErrorDTO()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "Invalid CheckIn date format. valid format is MM/dd/yyyy"
                });
            }

            if (!DateTime.TryParseExact(checkOutDate, "MM/dd/yyyy",
                                        CultureInfo.InvariantCulture,
                                        DateTimeStyles.None,
                                        out DateTime dtCheckOutDate)
                )
            {
                return BadRequest(new ErrorDTO()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "Invalid CheckOut date format. valid format is MM/dd/yyyy"
                });
            }
            var allRooms = await hotelRoomRepository
                                                            .GetAllHotelRooms(checkInDate, checkOutDate);
            return Ok(allRooms);
        }

        [HttpGet("{roomId}")]
        public async Task<IActionResult> GetHotelRoom(int? roomId, string checkInDate = null,
                                                                string checkOutDate = null)
        {
            if (roomId is null || roomId.Equals(0))
            {
                return BadRequest(new ErrorDTO
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "Invalid Room Id"
                });
            }
            if (string.IsNullOrEmpty(checkInDate) || string.IsNullOrEmpty(checkOutDate))
            {
                return BadRequest(new ErrorDTO()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "All parameters need to be supplied"
                });
            }
            if (!DateTime.TryParseExact(checkInDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dtCheckInDate))
            {
                return BadRequest(new ErrorDTO()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "Invalid CheckIn date format. valid format is MM/dd/yyyy"
                });
            }
            if (!DateTime.TryParseExact(checkOutDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dtCheckOutDate))
            {
                return BadRequest(new ErrorDTO()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "Invalid CheckOut date format. valid format is MM/dd/yyyy"
                });
            }

            var roomDetails = await hotelRoomRepository
                                                .GetHotelRoom(roomId.Value, checkInDate, checkOutDate);
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

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOS
{
    public class HotelRoomDTO
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter room name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Enter occupancy name")]
        public int Occupancy { get; set; }
        [Range(1, 2000, ErrorMessage = "Price must be between 1€ and 2000€")]
        public double Price { get; set; }
        public string Details { get; set; }
        public string Area { get; set; }
        public virtual ICollection<RoomImageDTO> RoomImages { get; set; }
        public List<string> ImagesUrls { get; set; }

    }
}

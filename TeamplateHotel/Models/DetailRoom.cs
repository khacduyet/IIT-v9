using ProjectLibrary.Database;
using System.Collections.Generic;

namespace TeamplateHotel.Models
{
    public class DetailRoom
    {
        public Room Room { get; set; }
        public List<Room> Rooms { get; set; }
        public List<RoomGallery> RoomGalleries { get; set; }
    }
}
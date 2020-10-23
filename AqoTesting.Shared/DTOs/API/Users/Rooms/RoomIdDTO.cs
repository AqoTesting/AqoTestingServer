using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace AqoTesting.Shared.DTOs.API.Users.Rooms
{
    public class RoomIdDTO
    {
        [Required]
        public string Id { get; set; }
    }
}

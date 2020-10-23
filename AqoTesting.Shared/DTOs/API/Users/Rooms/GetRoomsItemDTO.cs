namespace AqoTesting.Shared.DTOs.API.Users.Rooms
{
    public class GetRoomsItemDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public string OwnerId { get; set; }
        public bool IsDataRequired { get; set; }
        public bool IsActive { get; set; }
    }
}

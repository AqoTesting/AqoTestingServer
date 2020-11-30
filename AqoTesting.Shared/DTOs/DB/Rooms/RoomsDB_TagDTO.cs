using AqoTesting.Shared.Enums;

namespace AqoTesting.Shared.DTOs.DB.Rooms
{
    public class RoomsDB_TagDTO
    {
        public string Title { get; set; }
        public string? BackgroundColor { get; set; }
        public string? TextColor { get; set; }
        public FontStyles FontStyle { get; set; }
    }
}

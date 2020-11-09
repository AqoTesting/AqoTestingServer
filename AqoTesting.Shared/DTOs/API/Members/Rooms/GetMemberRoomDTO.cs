namespace AqoTesting.Shared.DTOs.API.Members.Rooms
{
    public class GetMemberRoomDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public string Description { get; set; }
        public string OwnerId { get; set; }
        public GetMemberRoomFieldDTO[] Fields { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproveManually { get; set; }
        public bool IsRegistrationEnabled { get; set; }
    }
}
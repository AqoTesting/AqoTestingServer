namespace AqoTesting.Shared.DTOs.API.UserAPI.Tests
{
    public struct UserAPI_Section_DTO
    {
        public string Id { get; set; }

        public UserAPI_Question_DTO[] Questions { get; set; }
    }
}

namespace AqoTesting.Shared.DTOs.API.UserAPI.Attempts.Options
{
    public class UserAPI_AttemptOptions_DTO
    {
        public UserAPI_AttemptCommonOption_DTO[]? Correct { get; set; }
        public UserAPI_AttemptCommonOption_DTO[] Answer { get; set; }
    }
}

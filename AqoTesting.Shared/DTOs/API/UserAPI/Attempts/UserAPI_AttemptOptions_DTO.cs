namespace AqoTesting.Shared.DTOs.API.UserAPI.Attempts.Options
{
    public class UserAPI_AttemptOptions_DTO
    {
        public UserAPI_AttemptCommonOption_DTO[]? CorrectOptions { get; set; }
        public UserAPI_AttemptCommonOption_DTO[] AnswerOptions { get; set; }
    }
}

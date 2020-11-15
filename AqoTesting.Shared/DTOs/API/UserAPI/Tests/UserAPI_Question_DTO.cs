using AqoTesting.Shared.Enums;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Tests
{
    public class UserAPI_Question_DTO
    {
        public int Id { get; set; }
        public QuestionTypeEnum Type { get; set; }
        public string? Text { get; set; }
        public bool Shuffle { get; set; }
        public string? OptionsJson { get; set; }
    }
}

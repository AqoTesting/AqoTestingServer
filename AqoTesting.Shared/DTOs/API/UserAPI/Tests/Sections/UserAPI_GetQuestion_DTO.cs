using AqoTesting.Shared.DTOs.API.UserAPI.Tests.Sections.Options;
using AqoTesting.Shared.Enums;

namespace AqoTesting.Shared.DTOs.API.UserAPI.Tests.Sections
{
    public class UserAPI_GetQuestion_DTO
    {
        public QuestionTypes Type { get; set; }
        public string? Text { get; set; }
        public bool? Shuffle { get; set; }
        public UserAPI_CommonOption_DTO[]? Options { get; set; }
        public int? Cost { get; set; }
        public int? Weight { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.CommonAPI.Identifiers
{
    public class CommonAPI_TestQuestionIdDTO
    {
        [Required]
        public string QuestionId { get; set; }
    }
}

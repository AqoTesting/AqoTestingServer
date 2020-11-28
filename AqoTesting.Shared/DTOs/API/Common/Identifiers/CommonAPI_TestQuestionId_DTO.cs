using System.ComponentModel.DataAnnotations;

namespace AqoTesting.Shared.DTOs.API.Common.Identifiers
{
    public class CommonAPI_TestQuestionId_DTO
    {
        [Required]
        public string QuestionId { get; set; }
    }
}

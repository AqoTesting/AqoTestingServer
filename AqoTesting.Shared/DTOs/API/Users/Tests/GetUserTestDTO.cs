using System;

namespace AqoTesting.Shared.DTOs.API.Users.Tests
{
    public class GetUserTestDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string OwnerId { get; set; }
        public bool IsActive { get; set; }
        public SectionDTO[] Sections { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ActivationDate { get; set; }
        public DateTime? DeactivationDate { get; set; }
        public bool Shuffle { get; set; }
    }
}
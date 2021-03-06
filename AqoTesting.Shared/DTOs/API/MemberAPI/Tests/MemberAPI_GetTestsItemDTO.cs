﻿using System;

namespace AqoTesting.Shared.DTOs.API.MemberAPI.Tests
{
    public class MemberAPI_GetTestsItemDTO
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public string? UserId { get; set; }
        public string? RoomId { get; set; }
        public int TimeLimit { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ActivationDate { get; set; }
        public DateTime? DeactivationDate { get; set; }
    }
}

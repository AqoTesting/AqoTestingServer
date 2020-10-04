﻿using System;

namespace AqoTesting.DTOs.BDModels
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string? Name { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}

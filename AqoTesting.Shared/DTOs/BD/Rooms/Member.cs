﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AqoTesting.Shared.DTOs.BD.Rooms
{
    public class Member
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public Attempt[] Attempts { get; set; }
        public object UserData { get; set; }
    }
}
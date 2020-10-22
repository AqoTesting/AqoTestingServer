using System;
using System.Collections.Generic;
using System.Text;

namespace AqoTesting.Shared.DTOs.DB.Rooms
{
    public class Member
    {
        public string Token { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public Attempt[] Attempts { get; set; }
        public object UserData { get; set; }
    }
}

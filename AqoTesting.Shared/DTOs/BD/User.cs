using System;

namespace AqoTesting.Shared.DTOs.BD
{
    public struct User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string? Name { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}

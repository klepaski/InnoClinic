﻿using AuthAPI.Models;

namespace AuthAPI.Contracts.Requests
{
    public class RegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}

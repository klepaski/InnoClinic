﻿using AuthAPI.Models;
using JuliaChistyakovaPackage;

namespace AuthAPI.Contracts.Responses
{
    public class RegisterResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public User? NewUser { get; set; }
    }
}

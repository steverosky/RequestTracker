﻿using System.ComponentModel.DataAnnotations;

namespace RequestTracker.Models.BaseModels.ResponseModels
{
    public class TokenResponse
    {       
        public string Token { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; 
        public string Email { get; set; } = string.Empty;
        public double ExpiryTime { get; set; }
    }
}

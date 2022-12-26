﻿using System.ComponentModel.DataAnnotations;

namespace RequestTracker.Models.BaseModels.RequestModels
{
    public class ChangePassModel
    {
        [Required]
        public string OldPassword { get; set; } = string.Empty;
        
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}

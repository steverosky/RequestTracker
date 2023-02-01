using System.ComponentModel.DataAnnotations;

namespace RequestTracker.Models.BaseModels.RequestModels
{
    public class ChangePassModel
    {
        public string Email { get; set; } = string.Empty;
        [Required]
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}

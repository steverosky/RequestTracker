using System.ComponentModel.DataAnnotations;

namespace RequestTracker.Models.BaseModels.RequestModels
{
    public class ChangePassModel
    {
        public string Email { get; set; } = string.Empty;
        [Required]
        public string CurrentPassword { get; set; } = string.Empty;
        [Required, MinLength(6, ErrorMessage ="Please Enter At Least 6 Characters")]
        public string NewPassword { get; set; } = string.Empty;
        [Required, Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}

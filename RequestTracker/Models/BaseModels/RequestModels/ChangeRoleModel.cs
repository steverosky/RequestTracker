using System.ComponentModel.DataAnnotations;

namespace RequestTracker.Models.BaseModels.RequestModels
{
    public class ChangeRoleModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Role { get; set; } = string.Empty;
    }
}

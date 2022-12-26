using System.ComponentModel.DataAnnotations;

namespace RequestTracker.Models.BaseModels.RequestModels
{
    public class DeleteUserModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}

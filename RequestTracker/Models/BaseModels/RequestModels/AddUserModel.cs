using System.ComponentModel.DataAnnotations;

namespace RequestTracker.Models.BaseModels.RequestModels
{
    public class AddUserModel
    {
       
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Valid Email is Required")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        public int DepartmentId { get; set; }
        public int RoleId { get; set; }
        public int ManagerId { get; set; }

    }
}

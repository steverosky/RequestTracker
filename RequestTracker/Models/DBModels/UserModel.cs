using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RequestTracker.Models
{
    [Table("users")]
    public class EmployeeModel
    {
        [Key, Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public int RoleId { get; set; }
        public int ManagerId { get; set; }

    }

    [Table("department")]
    public class DepartmentModel
    {
        [Key, Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
    }

    [Table("requests")]
    public class RequestModel
    {
        [Key, Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Status { get; set; } = string.Empty;
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public int DepartmentId { get; set; }
        public int CategoryId { get; set; }
    }

    [Table("category")]
    public class CategoryModel
    {
        [Key, Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
    }

    [Table("roles")]
    public class RoleModel
    {
        [Key, Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
    }

    [Table("managers")]
    public class ManagerModel
    {
        [Key, Required]
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
    }

    [Table("status")]
    public class StatusModel
    {
        [Key, Required]
        public int Id { get; set; }
        [Required]
        public string ManagerDesc { get; set; } = string.Empty;
        public string AdminDesc { get; set; } = string.Empty;

    }
}



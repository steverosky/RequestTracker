using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RequestTracker.Models.DBModels
{
    [Table("users", Schema = "backend")]
    public class EmployeeModel
    {
        [Key, Required]
        public int UserId { get; set; }       
        public string Name { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int DeptId { get; set; }
        public int RoleId { get; set; }
        public int ManagerId { get; set; }
        

    }

    [Table("department", Schema = "backend")]
    public class DepartmentModel
    {
        [Key, Required]
        public int DeptId { get; set; }
        [Required]
        public string DeptName { get; set; } = string.Empty;
    }

    [Table("requests", Schema = "backend")]
    public class RequestModel
    {
        [Key, Required]
        public int RequestId { get; set; }
        [Required]
        public string RequestDesc { get; set; } = string.Empty;
        public DateTime DateTime { get; set; }
        [Required]
        public string ManagerReview { get; set; } = string.Empty;
        public string AdminReview { get; set; } = string.Empty;
        public int DeptId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }

    [Table("category", Schema = "backend")]
    public class CategoryModel
    {
        [Key, Required]
        public int CategoryId { get; set; }
        [Required]
        public string CategoryName { get; set; } = string.Empty;
    }

    [Table("roles", Schema = "backend")]
    public class RoleModel
    {
        [Key, Required]
        public int RoleId { get; set; }
        [Required]
        public string RoleName { get; set; } = string.Empty;
    }

    [Table("managers", Schema = "backend")]
    public class ManagerModel
    {
        [Key, Required]
        public int ManagerId { get; set; }
        public int DeptId { get; set; }
        [Required]
        public string ManagerName { get; set; } = string.Empty;
    }

    [Table("status", Schema = "backend")]
    public class StatusModel
    {
        [Key, Required]
        public int StatusId { get; set; }
        [Required]
        public string StatusName { get; set; } = string.Empty;

    }
}



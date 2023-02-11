using System.ComponentModel.DataAnnotations;

namespace RequestTracker.Models.BaseModels.ResponseModels
{
    public class TokenResponse
    {       
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; }
        public string Token { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; 
        public string Status { get; set; } = string.Empty;
        public double ExpiryTime { get; set; }
        public int DeptId { get; set; }
        public int RoleId { get; set; }
        public int? ManagerId { get; set; }
    }
}

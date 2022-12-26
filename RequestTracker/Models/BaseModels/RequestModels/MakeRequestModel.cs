using System.ComponentModel.DataAnnotations;

namespace RequestTracker.Models.BaseModels.RequestModels
{
    public class MakeRequestModel
    {
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string Description { get; set; } = string.Empty;

    }
}

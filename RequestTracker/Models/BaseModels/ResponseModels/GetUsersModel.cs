namespace RequestTracker.Models.BaseModels.ResponseModels
{
    public class GetUsersModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Status { get; set; }
        public string Manager { get; set; } = string.Empty;

    }
}

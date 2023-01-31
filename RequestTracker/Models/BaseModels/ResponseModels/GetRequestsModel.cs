namespace RequestTracker.Models.BaseModels.ResponseModels
{
    public class GetRequestsModel
    {
        public string RequestId { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DateTime { get; set; }
        public string ManangerReview { get; set; } = string.Empty;
        public string AdminReview { get; set; } = string.Empty;
        public string Manager { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public DateTime? MangApprovedDate { get; set; }
        public DateTime? AdminApprovedDate { get; set; }
        public string? Reason { get; set; } = string.Empty;
    }


    public class GetRequestsModelAdmin
    {
        public string RequestId { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DateTime { get; set; }
        public string Department { get; set; } = string.Empty;
        public string ManangerReview { get; set; } = string.Empty;
        public string AdminReview { get; set; } = string.Empty;
        public string Manager { get; set; } = string.Empty;
        public DateTime? MangApprovedDate { get; set; }
        public DateTime? AdminApprovedDate { get; set; }
        public string? Reason { get; set; } = string.Empty;
    }

    public class GetRequestsModelEmployee
    {
        public string  RequestId { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DateTime { get; set; }
        public string ManangerReview { get; set; } = string.Empty;
        public string AdminReview { get; set; } = string.Empty;
        public string Manager { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public DateTime? MangApprovedDate { get; set; }
        public DateTime? AdminApprovedDate { get; set; }
        public string? Reason { get; set; } = string.Empty;
    }
}

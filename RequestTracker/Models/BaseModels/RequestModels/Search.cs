namespace RequestTracker.Models.BaseModels.RequestModels
{
    public class Search
    {
        public string? Keyword { get; set; } = string.Empty;
        public string? BeginDate { get; set; }
        public string? EndDate { get; set; } 

    }
}

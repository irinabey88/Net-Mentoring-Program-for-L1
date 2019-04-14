namespace ConfigurationManager.Models
{
    public class RuleConfiguration
    {
        public string Pattern { get; set; }

        public string DestinationDirectory { get; set; }

        public bool IsAddNumber { get; set; }

        public bool IsAddDate { get; set; }
    }
}
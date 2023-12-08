namespace SFA.DAS.FAA.Domain.Configuration
{
    public class FindApprenticeshipsApiConfiguration
    {
        public string ElasticSearchUsername { get; set; }
        public string ElasticSearchPassword { get; set; }
        public string ElasticSearchServerUrl { get; set; }
        public string AzureSearchBaseUrl { get; set; }
        public string AzureSearchResource { get; set; }
        public string AzureSearchKey { get; set; }
    }
}
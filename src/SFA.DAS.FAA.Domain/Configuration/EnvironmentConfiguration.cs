namespace SFA.DAS.FAA.Domain.Configuration
{
    public class EnvironmentConfiguration(string environmentName)
    {
        public string EnvironmentName { get; } = environmentName;
    }
}
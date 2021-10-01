namespace SFA.DAS.FAA.Domain.Configuration
{
    public class FindApprenticeshipsApiEnvironment
    {
        public virtual string EnvironmentName { get; }

        public FindApprenticeshipsApiEnvironment(string environmentName)
        {
            EnvironmentName = environmentName.ToLower();
        }
    }
}

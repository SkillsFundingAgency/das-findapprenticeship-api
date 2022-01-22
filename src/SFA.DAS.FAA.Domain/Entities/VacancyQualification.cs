namespace SFA.DAS.FAA.Domain.Entities
{
    public class VacancyQualification
    {
        public string QualificationType { get; set; }
        public string Subject { get; set; }
        public string Grade { get; set; }
        public Weighting Weighting { get; set; }
    }
    
    public enum Weighting
    {
        Essential,
        Desired
    }
}
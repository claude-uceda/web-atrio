namespace Webatrio.Employee.Core.Entities
{

    public class Person : Entity
    {
        public required string LastName { get; init; }
        public required string FirstName { get; init; }
        public required DateOnly DateOfBirth { get; init; }

        private List<JobExperience> _jobExperiences = null;

        public List<JobExperience> JobExperiences 
        { 
            get 
            {
                _jobExperiences ??= new List<JobExperience>();
                return _jobExperiences;
            } 
            set 
            {
                _jobExperiences = value;
            } 
        }
    }
}

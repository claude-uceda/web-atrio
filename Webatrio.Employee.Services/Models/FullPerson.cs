using Webatrio.Employee.Core.Entities;
using Webatrio.Employee.Core.Util;

namespace Webatrio.Employee.Services.Models
{
    public record FullPerson
    {        
        public required string LastName { get; init; }
        public required string FirstName { get; init; }
        public required DateOnly DateOfBirth { get; init; }

        public int Age 
        { 
            get 
            {
                return DateOfBirth.GetAge();
            } 
        }
        public required JobExperience[] JobExperiences { get; init; }

        public required JobExperience[] CurrentJobExperiences { get; init; }
    }
}

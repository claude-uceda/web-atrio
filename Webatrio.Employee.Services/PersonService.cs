
using Webatrio.Employee.Entities;
using Webatrio.Employee.Core.Repositories;
using Webatrio.Employee.Core.Util;
using Webatrio.Employee.Core.Entities;
using Webatrio.Employee.Services.Models;

namespace Webatrio.Employee.Services
{
    public class PersonService : IService 
    {
        private const int _AGE = 150;

        private readonly IRepository<Person> _personRepository;

        public PersonService(IRepository<Person> personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<OperationResult<Person>> Add(Person person)
        {
            if (person == null)
                return new ArgumentNullException(nameof(person));
            if (person.DateOfBirth.GetAge() >= _AGE)
                return new ApplicationException($"Age exceeding: { _AGE }");


            if (person.Id == Guid.Empty)
            {
                await _personRepository.InsertAsync(person);
            }
            else 
            {
                await _personRepository.OverwriteAsync(person);
            }

            return person;
        }

        public async Task<OperationResult<JobExperience>> AddJobExperience(Guid personId, JobExperience jobExperience)
        {
            var person = await _personRepository.GetOneAsync(personId, CancellationToken.None);
            if (person == null)
                return new Exception("User not found");

            person.JobExperiences.Add(jobExperience);

            return jobExperience;
        }

        public async Task<IEnumerable<FullPerson>> GetAll(CancellationToken cancellationToken)
        {
            var persons = await _personRepository.GetAsync(x=> true, -1, -1,new SortMember<Person, object> {MemberExpression = x=>x.LastName, Order = SortOrder.Ascending } , cancellationToken);

            return persons.Select(item => new FullPerson
            {
                FirstName = item.FirstName,
                CurrentJobExperiences = item.JobExperiences.Where(x => x.End == null).ToArray(),
                JobExperiences = item.JobExperiences.ToArray(),
                DateOfBirth = item.DateOfBirth,
                LastName = item.LastName,
            });
        }
    }
}

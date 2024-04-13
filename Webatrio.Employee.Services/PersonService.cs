
using Webatrio.Employee.Entities;
using Webatrio.Employee.Core.Repositories;
using Webatrio.Employee.Core.Util;
using Webatrio.Employee.Core.Entities;

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
    }
}

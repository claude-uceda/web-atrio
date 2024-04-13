
using Webatrio.Employee.Entities;
using Webatrio.Employee.Entities.Repositories;

namespace Webatrio.Employee.Services
{
    public class PersonService
    {
        private readonly IRepository<Person> _personRepository;

        public PersonService(IRepository<Person> personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<OperationResult<Person>> Add(Person person)
        {
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

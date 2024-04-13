using Webatrio.Employee.Entities;
using Webatrio.Employee.Entities.Repositories.InMemory;

namespace Webatrio.Employee.Services.Test
{
    [TestClass]
    public class PersonServiceTests
    {
        [TestMethod]
        public async Task AddPerson()
        {
            var person = new Person { DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow), FirstName = "Juan", LastName = "Perez" };

            var service = new PersonService(new InMemoryRepository<Person>());
            var result = await service.Add(person);

            Assert.IsTrue(result.Successful);
            Assert.AreNotEqual(Guid.Empty, result.Value!.Id);
        }

        [TestMethod]
        public async Task AddEmptyPerson()
        {
            var service = new PersonService(new InMemoryRepository<Person>());
            var result = await service.Add(null);

            Assert.IsFalse(result.Successful);
        }

        [TestMethod]
        public async Task AddExtraOldPerson()
        {
            var person = new Person { DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-200)), FirstName = "Juan", LastName = "Perez" };

            var service = new PersonService(new InMemoryRepository<Person>());
            var result = await service.Add(person);

            Assert.IsFalse(result.Successful);
        }
    }
}
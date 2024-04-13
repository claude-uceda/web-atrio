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
    }
}
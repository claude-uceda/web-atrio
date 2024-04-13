using Webatrio.Employee.Core;
using Webatrio.Employee.Core.Entities;
using Webatrio.Employee.Core.Repositories.InMemory;

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

        [TestMethod]
        public async Task AddJob()
        {
            var person = new Person { DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow), FirstName = "Juan", LastName = "Perez" };
            var job = new JobExperience { Start = DateOnly.FromDateTime(DateTime.UtcNow), Name = "Job1" };

            var service = new PersonService(new InMemoryRepository<Person>());
            var pResult = await service.Add(person);
            var result = await service.AddJobExperience(pResult.Value.Id, job);

            Assert.IsTrue(result.Successful);
        }


        [TestMethod]
        public async Task GetAllPersons()
        {
            var person = new Person { DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow), FirstName = "Juan", LastName = "Perez" };

            var service = new PersonService(new InMemoryRepository<Person>());
            var _= await service.Add(person);
            var items = await service.GetAll(CancellationToken.None);

            Assert.IsTrue(items.Count() > 0);
        }
    }
}
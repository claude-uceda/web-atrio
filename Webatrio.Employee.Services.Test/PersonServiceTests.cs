using Webatrio.Employee.Entities;

namespace Webatrio.Employee.Services.Test
{
    [TestClass]
    public class PersonServiceTests
    {
        [TestMethod]
        public void AddPerson()
        {
            var person = new Person { DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow), FirstName = "Juan", LastName = "Perez" };

            var service = new PersonService();
            var result = service.Add(person);

            Assert.IsTrue(result.Successful);
        }
    }
}
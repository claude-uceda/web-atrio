using Webatrio.Employee.Core.Entities;
using Webatrio.Employee.Core.Repositories.InMemory;

namespace Webatrio.Employee.Services.Test
{
    [TestClass]
    public class JobServiceTests
    {
        [TestMethod]
        public async Task AddJob()
        {
            var job = new JobExperience { PersonId = Guid.NewGuid(), Start = DateOnly.FromDateTime(DateTime.UtcNow), Name = "Job1" };

            var service = new JobService(new InMemoryRepository<JobExperience>());
            var result = await service.Add(job);

            Assert.IsTrue(result.Successful);
            Assert.AreNotEqual(Guid.Empty, result.Value!.Id);
        }
    }
}

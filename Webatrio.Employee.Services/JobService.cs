using Webatrio.Employee.Core.Entities;
using Webatrio.Employee.Core.Repositories.InMemory;
using Webatrio.Employee.Entities;

namespace Webatrio.Employee.Services
{
    public class JobService
    {
        private readonly InMemoryRepository<JobExperience> _jobExperienceRepository;

        public JobService(InMemoryRepository<JobExperience> jobExperienceRepository)
        {
            _jobExperienceRepository = jobExperienceRepository;
        }

        public async Task<OperationResult<JobExperience>> Add(JobExperience jobExperience)
        {
            if (jobExperience.Id == Guid.Empty)
            {
                await _jobExperienceRepository.InsertAsync(jobExperience);
            }
            else
            {
                await _jobExperienceRepository.OverwriteAsync(jobExperience);
            }

            return jobExperience;
        }
    }
}
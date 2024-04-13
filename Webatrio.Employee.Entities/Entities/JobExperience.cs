namespace Webatrio.Employee.Core.Entities
{
    public class JobExperience : Entity
    {
        public required Guid PersonId { get; init; }

        public required DateOnly Start { get; init; }

        public DateOnly? End { get; set; }

        public string? Name { get; set; }
    }
}

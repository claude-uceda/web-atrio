namespace Webatrio.Employee.Core.Entities
{
    public class JobExperience
    {
        public required DateOnly Start { get; init; }

        public DateOnly? End { get; set; }

        public string? Name { get; set; }
    }
}

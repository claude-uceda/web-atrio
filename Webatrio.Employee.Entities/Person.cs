namespace Webatrio.Employee.Entities
{
    public class Person
    {
        public required string LastName { get; init; }
        public required string FirstName { get; init; }
        public required DateOnly DateOfBirth { get; init; }
    }
}

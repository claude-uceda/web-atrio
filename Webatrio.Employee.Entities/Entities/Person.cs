namespace Webatrio.Employee.Core.Entities
{

    public class Person : Entity
    {
        public required string LastName { get; init; }
        public required string FirstName { get; init; }
        public required DateOnly DateOfBirth { get; init; }
    }
}

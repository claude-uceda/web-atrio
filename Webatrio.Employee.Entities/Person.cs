namespace Webatrio.Employee.Entities
{

    public interface IEntity
    {
        Guid Id { get; set; }
    }

    public class Entity : IEntity
    {
        public Guid Id { get; set; }
    }

    public class Person : Entity
    {
        public required string LastName { get; init; }
        public required string FirstName { get; init; }
        public required DateOnly DateOfBirth { get; init; }
    }
}

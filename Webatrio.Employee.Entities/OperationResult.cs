
namespace Webatrio.Employee.Entities
{
    public struct OperationResult<T>
    {
        public bool Successful { get; private set; }

        public Exception? Error { get; private set; }

        public T? Value { get; private set; }

        public static implicit operator OperationResult<T>(T value) => new () { Successful = true, Value = value };

        public static implicit operator OperationResult<T>(Exception ex) => new () { Successful = false, Error = ex };
    }
}

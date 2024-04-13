namespace Webatrio.Employee.Api.Modules
{
    public interface IModule
    {
        IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints);
    }
}

using Microsoft.AspNetCore.Mvc;
using Webatrio.Employee.Entities;
using Webatrio.Employee.Services;

namespace Webatrio.Employee.Api.Modules.Persons
{
    public class PersonModule : IModule
    {
        public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost($"/persons", AddPerson)
                .WithName("AddPerson")
                .WithOpenApi()
            ;          

            return endpoints;
        }

        internal virtual async Task<IResult> AddPerson([FromServices] PersonService service, [FromBody] Person model, CancellationToken cancellationToken)
        {
            var result = await service.Add(model);

            if (result.Successful)
                return Results.Ok();

            return Results.Problem(result.Error!.Message, statusCode: 500);
        }
    }
}

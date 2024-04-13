using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Webatrio.Employee.Core.Entities;
using Webatrio.Employee.Services;
using Webatrio.Employee.Services.Models;

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

            endpoints.MapGet($"/persons", GetPersons)
                .WithName("GetPerson")
                .WithMetadata(new SwaggerResponseAttribute(200, type: typeof(FullPerson[])))
                .WithOpenApi()
            ;

            return endpoints;
        }

        internal virtual async Task<IResult> AddPerson([FromServices] PersonService service, [FromBody] Person model, CancellationToken cancellationToken)
        {
            var result = await service.Add(model);

            if (result.Successful)
                return Results.Ok(new { id = result!.Value!.Id});

            return Results.Problem(result.Error!.Message, statusCode: 500);
        }

        internal virtual async Task<IResult> GetPersons([FromServices] PersonService service, CancellationToken cancellationToken)
        {
            var result = await service.GetAll(cancellationToken);

            return Results.Ok(result);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Webatrio.Employee.Core.Entities;
using Webatrio.Employee.Services;

namespace Webatrio.Employee.Api.Modules.JobExperiences
{
    public class JobExperienceModule : IModule
    {
        public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("/persons/{id}/experiences", AddJobExperience)
                .WithName("AddJobExperience")
                .WithOpenApi()
            ;

            return endpoints;
        }

        internal virtual async Task<IResult> AddJobExperience([FromServices] PersonService service, [FromBody] JobExperience model, [FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await service.AddJobExperience(id, model);

            if (result.Successful)
                return Results.Ok();

            return Results.Problem(result.Error!.Message, statusCode: 500);
        }
    }
}

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

        internal virtual async Task<IResult> AddJobExperience([FromServices] JobService service, [FromBody] JobExperience model, [FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await service.Add(model);

            if (result.Successful)
                return Results.Ok(new { id = result!.Value!.Id });

            return Results.Problem(result.Error!.Message, statusCode: 500);
        }
    }
}

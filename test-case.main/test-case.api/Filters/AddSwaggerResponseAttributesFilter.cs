using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace test_case.api.Filters
{
    public class AddSwaggerResponseAttributesFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var controllerActionDescriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;

            if (controllerActionDescriptor != null)
            {
                var swaggerResponseAttributes = controllerActionDescriptor.MethodInfo.GetCustomAttributes<SwaggerResponseAttribute>();

                foreach (var attr in swaggerResponseAttributes)
                {
                    var statusCode = attr.StatusCode.ToString();
                    var response = new OpenApiResponse { Description = attr.Description };

                    if (attr.Type != null)
                    {
                        var schema = context.SchemaGenerator.GenerateSchema(attr.Type, context.SchemaRepository);
                        response.Content.Add("application/json", new OpenApiMediaType { Schema = schema });
                    }

                    if (operation.Responses.ContainsKey(statusCode))
                    {
                        operation.Responses[statusCode] = response;
                    }
                    else
                    {
                        operation.Responses.Add(statusCode, response);
                    }
                }
            }
        }
    }
}

using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LINK_UI.App_start
{
    public class CustomSwaggerParametersFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "lang",
                In = ParameterLocation.Query,
                Required = false
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "EntityName",
                In = ParameterLocation.Query,
                Required = true,
            });
        }
    }
}

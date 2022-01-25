using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace ToDoAppWeb.Filters
{
    public class UserHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            ControllerActionDescriptor descriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;

            if (descriptor != null && (descriptor.ControllerName.Contains("User") || 
                descriptor.ControllerName.Contains("TodoList") || 
                descriptor.ControllerName.Contains("ToDoTask")))
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "Authenticate_UserId",
                    In = ParameterLocation.Header,
                    Required = true
                }); 
            }

            if (descriptor != null && descriptor.ControllerName.StartsWith("Login"))
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "Username",
                    In = ParameterLocation.Header,
                    Required = true
                }); ;

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "Password",
                    In = ParameterLocation.Header,
                    Required = true
                });
            }
        }
    }
}

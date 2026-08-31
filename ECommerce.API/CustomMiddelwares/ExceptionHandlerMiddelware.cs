using ECommerce.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.CustomMiddelwares
{
    public class ExceptionHandlerMiddelware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddelware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);

                //call endpoint doesn't exist 
                await HandelNotFoundEndPointAsync(context);
            }
            catch (Exception ex) //Handling Server Error Done ! + 
            {
                var problem = new ProblemDetails()
                {
                    Title = "UnExcepeted Error IS Occurred",
                    Detail = ex.Message,
                    Instance = context.Request.Path,
                    Status = ex switch
                    {
                        NotFoundException => StatusCodes.Status404NotFound,
                        _ => StatusCodes.Status500InternalServerError,
                    }
                    
                };
                context.Response.StatusCode = problem.Status.Value;
                await context.Response.WriteAsJsonAsync(problem);
            }
        }
        private static async Task HandelNotFoundEndPointAsync(HttpContext context)
        {
            if(context.Response.StatusCode == StatusCodes.Status404NotFound 
                && !context.Response.HasStarted)
            {
                var problem = new ProblemDetails()
                {
                    Title = "Error While Processing HTTP Request EndPoint Not Found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = $"Endpoint With This Path '{context.Request.Path}' Is Not Found",
                    Instance = context.Request.Path,
                };
                context.Response.StatusCode = problem.Status.Value;
                await context.Response.WriteAsJsonAsync(problem);
            }
        }
    }
}

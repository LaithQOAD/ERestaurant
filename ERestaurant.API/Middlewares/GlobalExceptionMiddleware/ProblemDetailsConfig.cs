using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace ERestaurant.API.Middlewares.GlobalExceptionMiddleware
{
    public static class ProblemDetailsConfig
    {
        public static IServiceCollection AddAppProblemDetails(
            this IServiceCollection services,
            IHostEnvironment enviroment)
        {
            services.AddProblemDetails(options =>
            {
                options.IncludeExceptionDetails = (httpContext, exception) => enviroment.IsDevelopment();

                options.Map<ArgumentNullException>(ex => NewProblem(400, "Bad Request", enviroment, ex));
                options.Map<ArgumentException>(ex => NewProblem(400, "Bad Request", enviroment, ex));
                options.Map<BadHttpRequestException>(ex => NewProblem(400, "Bad Request", enviroment, ex));
                options.Map<JsonException>(ex => NewProblem(400, "Bad Request", enviroment, ex));
                options.Map<BadRequestException>(ex => NewProblem(400, "Bad Request", enviroment, ex));

                options.Map<UnauthorizedAccessException>(ex => NewProblem(401, "Unauthorized", enviroment, ex));

                options.Map<ForbiddenException>(ex => NewProblem(403, "Forbidden", enviroment, ex));

                options.Map<KeyNotFoundException>(ex => NewProblem(404, "Not Found", enviroment, ex));
                options.Map<NotFoundException>(ex => NewProblem(404, "Not Found", enviroment, ex));

                options.Map<ConflictException>(ex => NewProblem(409, "Conflict", enviroment, ex));
                options.Map<DbUpdateConcurrencyException>(ex => NewProblem(409, "Concurrency Conflict", enviroment, ex));
                options.Map<DbUpdateException>(ex => NewProblem(409, "Database Update Error", enviroment, ex));

                options.Map<TaskCanceledException>(ex => NewProblem(408, "Request Timeout", enviroment, ex));
                options.Map<OperationCanceledException>(ex => NewProblem(408, "Request Timeout", enviroment, ex));
                options.Map<TimeoutException>(ex => NewProblem(504, "Gateway Timeout", enviroment, ex));

                options.Map<ValidationException>(ex => new ProblemDetails
                {
                    Status = StatusCodes.Status422UnprocessableEntity,
                    Title = "Validation Failed",
                    Detail = enviroment.IsDevelopment() ? ex.ToString() : ex.Message,
                    Type = null
                });
                options.Map<AppValidationException>(ex =>
                {
                    var vpd = new ValidationProblemDetails(ex.Errors)
                    {
                        Status = StatusCodes.Status422UnprocessableEntity,
                        Title = "Validation Failed",
                        Detail = enviroment.IsDevelopment() ? ex.ToString() : ex.Message,
                        Type = null
                    };
                    return vpd;
                });

                options.Map<NotImplementedException>(ex => NewProblem(501, "Not Implemented", enviroment, ex));

                options.Map<HttpRequestException>(ex => NewProblem(503, "Service Unavailable", enviroment, ex));

                options.Map<Exception>(ex => new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Server Error",
                    Detail = enviroment.IsDevelopment() ? ex.ToString() : ex.Message,
                    Type = null
                });

                options.OnBeforeWriteDetails = (http, details) =>
                {
                    if (details is ProblemDetails problemDetails)
                    {
                        problemDetails.Type = null;
                        problemDetails.Extensions["traceId"] = http.TraceIdentifier;

                        if (enviroment.IsDevelopment())
                        {
                            problemDetails.Extensions["request"] = new
                            {
                                method = http.Request.Method,
                                path = http.Request.Path.ToString(),
                                query = http.Request.Query.ToDictionary(k => k.Key, v => v.Value.ToString()),
                                routeValues = http.Request.RouteValues
                            };
                        }
                        else
                        {
                            problemDetails.Instance = null;
                            if (problemDetails.Extensions.ContainsKey("exception")) problemDetails.Extensions.Remove("exception");
                            if (problemDetails.Extensions.ContainsKey("errors")) problemDetails.Extensions.Remove("errors");
                            if (problemDetails.Extensions.ContainsKey("request")) problemDetails.Extensions.Remove("request");

                            var contact = "L.Amro@QOAD.com";

                            problemDetails.Extensions["support"] = new
                            {
                                message = "An unexpected error occurred. Please contact the issuer and include the (traceId)",
                                contact,
                                traceId = http.TraceIdentifier
                            };
                        }
                    }
                };
            });

            services.Configure<ApiBehaviorOptions>(o =>
            {
                o.SuppressMapClientErrors = true;
                o.ClientErrorMapping[StatusCodes.Status400BadRequest] = new ClientErrorData { Title = "Bad Request" };
                o.ClientErrorMapping[StatusCodes.Status401Unauthorized] = new ClientErrorData { Title = "Unauthorized" };
                o.ClientErrorMapping[StatusCodes.Status403Forbidden] = new ClientErrorData { Title = "Forbidden" };
                o.ClientErrorMapping[StatusCodes.Status404NotFound] = new ClientErrorData { Title = "Not Found" };
                o.ClientErrorMapping[StatusCodes.Status409Conflict] = new ClientErrorData { Title = "Conflict" };
                o.ClientErrorMapping[StatusCodes.Status422UnprocessableEntity] = new ClientErrorData { Title = "Validation Failed" };
                o.ClientErrorMapping[StatusCodes.Status500InternalServerError] = new ClientErrorData { Title = "Server Error" };
            });

            services.AddMvc(o => o.Filters.Add<ProblemDetailsResultFilter>());

            return services;
        }

        public static IApplicationBuilder UseAppProblemDetails(this IApplicationBuilder app)
        {
            app.UseProblemDetails();
            return app;
        }

        private static ProblemDetails NewProblem(int status, string title, IHostEnvironment env, Exception ex) =>
            new ProblemDetails
            {
                Status = status,
                Title = title,
                Detail = env.IsDevelopment() ? ex.ToString() : ex.Message,
                Type = null
            };
    }
}

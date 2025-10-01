using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace ERestaurant.API.Middlewares.GlobalExceptionMiddleware
{
    public sealed class ProblemDetailsResultFilter : IAsyncResultFilter
    {
        private readonly ProblemDetailsFactory _factory;
        private readonly IOptions<ApiBehaviorOptions> _apiBehavior;
        private readonly IHostEnvironment _env;

        public ProblemDetailsResultFilter(
            ProblemDetailsFactory factory,
            IOptions<ApiBehaviorOptions> apiBehavior,
            IHostEnvironment env)
        {
            _factory = factory;
            _apiBehavior = apiBehavior;
            _env = env;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            ProblemDetails? problem = null;
            int? status = null;
            string? detail = null;

            if (context.Result is ObjectResult obj && obj.StatusCode is >= 400)
            {
                status = obj.StatusCode!.Value;

                if (obj.Value is ProblemDetails existing)
                {
                    problem = existing;
                    if (string.IsNullOrWhiteSpace(problem.Title))
                        problem.Title = GetTitle(status.Value);
                }
                else
                {
                    detail = obj.Value?.ToString();
                    problem = _factory.CreateProblemDetails(
                        context.HttpContext,
                        statusCode: status.Value,
                        title: GetTitle(status.Value),
                        type: null,
                        detail: detail);
                }
            }
            else if (context.Result is StatusCodeResult scr && scr.StatusCode >= 400)
            {
                status = scr.StatusCode;
                problem = _factory.CreateProblemDetails(
                    context.HttpContext,
                    statusCode: status.Value,
                    title: GetTitle(status.Value),
                    type: null);
            }
            else if (context.Result is ForbidResult)
            {
                status = StatusCodes.Status403Forbidden;
                problem = _factory.CreateProblemDetails(
                    context.HttpContext,
                    statusCode: status.Value,
                    title: GetTitle(status.Value),
                    type: null);
            }
            else if (context.Result is ChallengeResult)
            {
                status = StatusCodes.Status401Unauthorized;
                problem = _factory.CreateProblemDetails(
                    context.HttpContext,
                    statusCode: status.Value,
                    title: GetTitle(status.Value),
                    type: null);
            }

            if (problem is not null)
            {
                problem.Type = null;
                problem.Extensions["traceId"] = context.HttpContext.TraceIdentifier;

                if (_env.IsDevelopment())
                {
                    problem.Extensions["request"] = new
                    {
                        method = context.HttpContext.Request.Method,
                        path = context.HttpContext.Request.Path.ToString(),
                        query = context.HttpContext.Request.Query.ToDictionary(k => k.Key, v => v.Value.ToString()),
                        routeValues = context.HttpContext.Request.RouteValues
                    };
                }
                else
                {
                    problem.Instance = null;

                    var contact = "L.Amro@QOAD.com";

                    problem.Extensions["support"] = new
                    {
                        message = "An unexpected error occurred. Please contact the issuer and include the (traceId)",
                        contact,
                        traceId = context.HttpContext.TraceIdentifier
                    };
                }

                context.Result = new ObjectResult(problem)
                {
                    StatusCode = status,
                    DeclaredType = typeof(ProblemDetails),
                    ContentTypes = { "application/problem+json" }
                };

                await next(); return;
            }


            await next();
        }

        private string GetTitle(int statusCode)
        {
            if (_apiBehavior.Value.ClientErrorMapping.TryGetValue(statusCode, out var map) &&
                !string.IsNullOrWhiteSpace(map.Title))
            {
                return map.Title!;
            }
            return ReasonPhrases.GetReasonPhrase(statusCode) ?? "Error";
        }
    }
}

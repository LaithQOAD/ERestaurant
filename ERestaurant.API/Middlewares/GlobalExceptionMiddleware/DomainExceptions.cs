namespace ERestaurant.API.Middlewares.GlobalExceptionMiddleware
{
    public sealed class NotFoundException : Exception
    {
        public NotFoundException(string resource, object key)
            : base($"{resource} with key '{key}' was not found.") { }
    }

    public sealed class ConflictException : Exception
    {
        public ConflictException(string message) : base(message) { }
    }

    public sealed class ForbiddenException : Exception
    {
        public ForbiddenException(string message) : base(message) { }
    }

    public sealed class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) { }
    }

    public sealed class AppValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }
        public AppValidationException(IDictionary<string, string[]> errors, string? message = null)
            : base(message ?? "Validation failed") => Errors = errors;
    }
}

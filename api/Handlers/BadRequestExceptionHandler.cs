using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;

namespace api.Handlers
{
    internal sealed class BadRequestExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<BadRequestExceptionHandler> _logger;

        public BadRequestExceptionHandler(ILogger<BadRequestExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            // Check if the exception is of type BadRequestException
            if (exception is not BadRequestException badRequestException)
            {
                return false; // If not, let other handlers handle the exception
            }

            // Log the exception
            _logger.LogError(
                badRequestException,
                "Exception occurred: {Message}",
                badRequestException.Message);

            // Prepare the ProblemDetails response
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Bad Request",
                Detail = badRequestException.Message
            };

            // Set the response status code and write the response
            httpContext.Response.StatusCode = (int)problemDetails.Status;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true; // Indicate that the exception was handled
        }
    }

    public class BadRequestException : Exception
    {
        public BadRequestException() { }
        public BadRequestException(string message) : base(message) { }
        public BadRequestException(string message, Exception inner) : base(message, inner) { }
    }
}
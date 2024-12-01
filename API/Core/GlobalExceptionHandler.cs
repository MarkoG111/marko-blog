using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Exceptions;
using FluentValidation;
using Newtonsoft.Json;

using Microsoft.Extensions.Logging;

namespace API.Core
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                httpContext.Response.ContentType = "application/json";

                object response = null;

                var statusCode = StatusCodes.Status500InternalServerError;

                string errorMessage = "An unexpected error occurred. Please try again later.";

                switch (ex)
                {
                    case AlreadyAddedException alreadyAddedException:
                        statusCode = StatusCodes.Status400BadRequest;
                        errorMessage = alreadyAddedException.Message;
                        response = new { message = errorMessage };
                        break;
                    case UserLikeException userLikeException:
                        statusCode = StatusCodes.Status400BadRequest;
                        errorMessage = userLikeException.Message;
                        response = new { message = errorMessage };
                        break;
                    case UnauthorizedUseCaseException _:
                        statusCode = StatusCodes.Status403Forbidden;
                        errorMessage = "You are not allowed to execute this operation.";
                        response = new { message = errorMessage };
                        break;
                    case EntityNotFoundException entityNotFoundException:
                        statusCode = StatusCodes.Status404NotFound;
                        errorMessage = "Content not found.";
                        response = new { message = errorMessage, details = entityNotFoundException.Message };
                        break;
                    case ValidationException validationException:
                        statusCode = StatusCodes.Status422UnprocessableEntity;
                        errorMessage = "Failed due to validation errors.";
                        response = new
                        {
                            message = errorMessage,
                            errors = validationException.Errors.Select(x => new
                            {
                                x.PropertyName,
                                x.ErrorMessage
                            })
                        };
                        break;
                    case ConflictException conflictException:
                        statusCode = StatusCodes.Status409Conflict;
                        errorMessage = conflictException.Message;
                        response = new { message = errorMessage };
                        break;
                    default:
                        _logger.LogError(ex, "Unhandled exception occurred.");
                        break;
                }

                LogError(ex, httpContext, statusCode);

                httpContext.Response.StatusCode = statusCode;

                if (response == null)
                {
                    response = new { message = errorMessage };
                }

                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
        }

        private void LogError(Exception ex, HttpContext context, int statusCode)
        {
            // Example: Log to a database, file system, or monitoring tool
            var errorDetails = new
            {
                Timestamp = DateTime.UtcNow,
                Path = context.Request.Path,
                StatusCode = statusCode,
                ExceptionType = ex.GetType().Name,
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                User = context.User.Identity?.Name
            };

            // Log to console or use a logging framework like Serilog or ELK
            _logger.LogError($"Error: {JsonConvert.SerializeObject(errorDetails)}");

            // Optional: Save to database or external service
            // _dbContext.ErrorLogs.Add(new ErrorLog { ... });
            // _dbContext.SaveChanges();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Exceptions;
using FluentValidation;
using Newtonsoft.Json;

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
                    case UnauthorizedUseCaseException _:
                        statusCode = StatusCodes.Status403Forbidden;
                        errorMessage = "You are not allowed to execute this operation.";
                        response = new { message = errorMessage };
                        break;
                    case AuthenticationException authException:
                        statusCode = StatusCodes.Status401Unauthorized;
                        errorMessage = authException.Message;
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
                    case UnauthorizedUserAccessException unauthorizedUserAccessException:
                        statusCode = StatusCodes.Status403Forbidden;
                        errorMessage = unauthorizedUserAccessException.Message;
                        response = new { message = errorMessage };
                        break;
                    default:
                        _logger.LogError(ex, "Unhandled exception occurred.");
                        statusCode = StatusCodes.Status500InternalServerError;
                        break;
                }

                httpContext.Response.StatusCode = statusCode;

                if (response == null)
                {
                    response = new { message = errorMessage };
                }

                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
        }
    }
}
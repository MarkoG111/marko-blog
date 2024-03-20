using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Exceptions;
using FluentValidation;
using Newtonsoft.Json;

namespace api.Core
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandler(RequestDelegate next)
        {
            _next = next;
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

                switch (ex)
                {
                    case UnauthorizedUseCaseException _:
                        statusCode = StatusCodes.Status403Forbidden;
                        response = new
                        {
                            message = "You are not allowed to execute this operation."
                        };
                        break;
                    case EntityNotFoundException prom:
                        statusCode = StatusCodes.Status404NotFound;
                        response = new 
                        {
                            message = "Content not found"
                        };
                        break;
                    case ValidationException validationException:
                        statusCode = StatusCodes.Status422UnprocessableEntity;
                        response = new 
                        {
                            message = "Failed due to validation errors.",
                            errors = validationException.Errors.Select(x => new 
                            {
                                x.PropertyName,
                                x.ErrorMessage
                            })
                        };
                        break;
                    case ConflictException exception:
                        statusCode = StatusCodes.Status409Conflict;
                        response = new 
                        {
                            message = exception.Message.ToString()
                        };
                        break;
                }

                httpContext.Response.StatusCode = statusCode;

                if (response != null)
                {
                    await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(response));
                    return;
                }

                await Task.FromResult(httpContext.Response);
            }
        }
    }
}
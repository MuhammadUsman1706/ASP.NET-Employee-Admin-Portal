using Microsoft.AspNetCore.Diagnostics;

namespace ASP.NET_EMPLOYEE_ADMIN_PORTAL.Exceptions
{
    public class ArgumentExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is ArgumentException)
            {
                var response = new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status422UnprocessableEntity,
                    ExceptionMessage = exception.Message,
                    Title = exception.GetType().Name
                };

                httpContext.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

                return true;
            }

            return false;
        }
    }
}

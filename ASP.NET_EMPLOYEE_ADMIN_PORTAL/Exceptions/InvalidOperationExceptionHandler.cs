using Microsoft.AspNetCore.Diagnostics;

namespace ASP.NET_EMPLOYEE_ADMIN_PORTAL.Exceptions
{
    public class InvalidOperationExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is InvalidOperationException)
            {
                var response = new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ExceptionMessage = exception.Message,
                    Title = exception.GetType().Name
                };

                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

                return true;
            }

            return false;
        }
    }
}

using Microsoft.AspNetCore.Diagnostics;

namespace ASP.NET_EMPLOYEE_ADMIN_PORTAL.Exceptions
{
    public class EntityNotFoundExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is EntityNotFoundException)
            {
                var response = new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ExceptionMessage = exception.Message,
                    Title = exception.GetType().Name
                };

                httpContext.Response.StatusCode = StatusCodes.Status404NotFound; // This line has to come first!
                await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

                return true;
            }

            return false;
        }
    }
}

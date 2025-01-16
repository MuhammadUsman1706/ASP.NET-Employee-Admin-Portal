namespace ASP.NET_EMPLOYEE_ADMIN_PORTAL.Exceptions
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }

        public required string Title { get; set; }

        public required string ExceptionMessage { get; set; }
    }
}

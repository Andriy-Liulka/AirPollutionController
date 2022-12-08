namespace AirController.Api.Middlewares
{
    public class ErrorHandleMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandleMiddleware> _logger;
        public ErrorHandleMiddleware(RequestDelegate next, ILogger<ErrorHandleMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Message: {ex.Message} \n StackTrace: {ex.StackTrace}");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Some error happened");
            }
        }
    }
}

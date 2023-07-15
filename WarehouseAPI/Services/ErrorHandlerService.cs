using WarehouseAPI.Exceptions;

namespace WarehouseAPI.Services
{
    public class ErrorHandlerService : IMiddleware
    {
        private readonly ILogger<ErrorHandlerService> _logger;
        public ErrorHandlerService(ILogger<ErrorHandlerService> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (AuthenticationException authRequestException)
            {
                var data = new { message = authRequestException.Message };

                context.Response.StatusCode = 401;
                await context.Response.WriteAsJsonAsync(data);
            }
            catch (TokenException tokenException)
            {
                var data = new { message = tokenException.Message };

                context.Response.StatusCode = 403;
                await context.Response.WriteAsJsonAsync(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                var data = new { message = ex.Message };

                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(data);
            }
        }
    }
}

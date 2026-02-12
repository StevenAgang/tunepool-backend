using Microsoft.AspNetCore.Mvc.Filters;

namespace tunepool.Repository.Configuration.AttributeExtender
{
    public class ControlAuthorization : IAsyncActionFilter
    {
        private IConfiguration _configuration;
        public ControlAuthorization(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext action, ActionExecutionDelegate next)
        {
            var context = action.HttpContext;

            if (!context.Request.Headers.ContainsKey("Handshake"))
            {
                await context.Response.WriteAsync("Unauthorize access of this endpoint is not allowed");
                return;
            }

            var key = _configuration.GetSection("APIKEY").Get<APIKEY>();

            context.Request.Headers.TryGetValue("Handshake", out var ApiKey).ToString();

            if(ApiKey != key?.CLIENTKEY)
            {
                throw new UnauthorizedAccessException("Unauthorize access of this endpoint is not allowed");
            }

            await next();
            return; // let access to controller
        }
    }
}

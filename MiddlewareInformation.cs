using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Settings
{
    public class MiddlewareInformation
    {
        private readonly RequestDelegate _next;

        public MiddlewareInformation(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
//            context.Request.

            await _next.Invoke(context);
            return;
        }
    }
}

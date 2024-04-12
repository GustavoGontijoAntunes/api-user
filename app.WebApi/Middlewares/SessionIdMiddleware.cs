using app.RepositoryAdapter;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace app.WebApi.Middlewares
{
    public class SessionIdMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, AppDbContext dbContext)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                var sessionId = context.User.FindFirst("Sessionid").Value;

                var user = await dbContext.Users.SingleOrDefaultAsync(u => u.SessionId == sessionId);
                if (user == null)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return;
                }
            }

            await _next(context);
        }
    }
}
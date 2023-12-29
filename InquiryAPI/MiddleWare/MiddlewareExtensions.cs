using InquiryAPI.Middleware;

namespace InquiryAPI.MiddleWare
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseUserIdMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserIdMiddleware>();
        }
    }
}

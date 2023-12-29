using InquiryAPI.Services.UserTokenService;

namespace InquiryAPI.Middleware
{
    public class UserIdMiddleware
    {
        private readonly RequestDelegate _next;

        public UserIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUserTokenService tokenService)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                var userIdClaim = context.User.Claims.Where(q => q.Type == "userId").FirstOrDefault();

                if (userIdClaim != null)
                {
                    tokenService.UserId = new Guid(userIdClaim.Value);
                }
            }

            await _next(context);
        }
    }
}

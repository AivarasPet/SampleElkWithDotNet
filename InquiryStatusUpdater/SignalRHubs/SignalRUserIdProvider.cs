using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace InquiryStatusUpdater.SignalRHubs
{
    public class SignalRUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            Claim claim = connection.User.Claims.FirstOrDefault(q => q.Type == "userId");
            Guid userId = new Guid(claim.Value);
            return userId.ToString();
        }
    }
}

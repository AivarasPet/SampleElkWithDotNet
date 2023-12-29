using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace InquiryStatusUpdater.SignalRHubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
    }
}

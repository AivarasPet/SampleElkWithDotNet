using InquiryAPI.Configuration;
using InquiryAPI.DomainObjects.Inquiries;
using InquiryAPI.Services.InquiriesService;
using InquiryStatusUpdater.SignalRHubs;
using Microsoft.AspNetCore.SignalR;

namespace InquiryStatusUpdater
{
    public class Worker : BackgroundService
    {
        private readonly IHubContext<NotificationHub> _notificationHubContext;
        private readonly IInquiriesService _inquiriesService;
        private readonly int _statusSetterWorkerDelayInMs;
        private readonly int _statusChangeDelayInS;
        private readonly string _notificationTopic;

        public Worker(AppConfiguration appConfiguration, IHubContext<NotificationHub> notificationHubContext, IInquiriesService inquiriesService)
        {
            _notificationHubContext = notificationHubContext;
            _statusSetterWorkerDelayInMs = appConfiguration.StatusSetterWorkerDelayInMs;
            _statusChangeDelayInS = appConfiguration.StatusChangeDelayInS;
            _notificationTopic = appConfiguration.SignalRConfiguration.InquiryListRefreshTopic;
            _inquiriesService = inquiriesService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                IEnumerable<Inquiry> issuedInquiries = _inquiriesService
                    .Search(new InquirySearchPredicate()
                    {
                        InquiryStatus = InquiryStatus.Issued
                    })
                    .OrderByDescending(q => q.CreationDate);

                if (issuedInquiries.Any())
                {
                    Inquiry inquiry = issuedInquiries.First();
                    if ((DateTimeOffset.UtcNow - inquiry.CreationDate).Seconds >= _statusChangeDelayInS)
                    {
                        inquiry.Status = InquiryStatus.Issued;
                        _inquiriesService.Save(inquiry);

                        await _notificationHubContext.Clients
                            .User(inquiry.UserId.ToString())
                            .SendAsync(_notificationTopic, inquiry.Id);
                    }
                }

                await Task.Delay(_statusSetterWorkerDelayInMs, stoppingToken);
            }
        }
    }
}
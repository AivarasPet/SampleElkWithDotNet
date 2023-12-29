namespace InquiryAPI.Configuration
{
    public class AppConfiguration
    {
        public string ConnectionString { get; set; }
        public string JwtSecretKey { get; set; }
        public int StatusSetterWorkerDelayInMs { get; set; }
        public int StatusChangeDelayInS { get; set; }
        public SignalRConfiguration SignalRConfiguration { get; set; }
    }
}

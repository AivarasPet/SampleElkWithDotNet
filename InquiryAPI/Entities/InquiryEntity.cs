using InquiryAPI.DomainObjects.Inquiries;

namespace InquiryAPI.Entities
{
    public class InquiryEntity : BaseDomainEntity
    {
        public DateTimeOffset CreationDate { get; set; }
        public string Message { get; set; }
        public InquiryType Type { get; set; }
        public InquiryStatus Status { get; set; }
        public Guid UserId { get; set; }
        public UserEntity User { get; set; }
    }
}

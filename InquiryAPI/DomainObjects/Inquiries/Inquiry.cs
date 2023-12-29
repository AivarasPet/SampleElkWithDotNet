namespace InquiryAPI.DomainObjects.Inquiries
{
    public enum InquiryType
    {
        Request,
        Proposal,
        Complaint
    }

    public enum InquiryStatus
    {
        Issued,
        Completed
    }

    public class Inquiry : BaseDomainObject
    {
        public Inquiry(InquiryType type, string data, Guid userId) : base()
        {
            CreationDate = DateTimeOffset.UtcNow;
            Status = InquiryStatus.Issued;
            Type = type;
            Message = data;
            UserId = userId;
        }

        public Inquiry(Guid id, DateTimeOffset creationDate, InquiryType type, InquiryStatus status, string data, Guid userId) : base(id)
        {
            CreationDate = creationDate;
            Type = type;
            Status = status;
            Message = data;
            UserId = userId;
        }

        public string Message { get; set; }
        public InquiryType Type { get; set; }
        public InquiryStatus Status { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public Guid UserId { get; private set; }
    }
}

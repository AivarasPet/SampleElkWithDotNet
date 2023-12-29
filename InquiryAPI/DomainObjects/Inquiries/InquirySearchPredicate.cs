namespace InquiryAPI.DomainObjects.Inquiries
{
    public class InquirySearchPredicate
    {
        public Guid? Id { get; set; }
        public InquiryType? InquiryType { get; set; }
        public InquiryStatus? InquiryStatus { get; set; }
        public Guid? UserId { get; set; }
    }
}

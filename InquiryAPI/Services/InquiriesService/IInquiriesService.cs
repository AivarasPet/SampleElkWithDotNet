using InquiryAPI.DomainObjects.Inquiries;

namespace InquiryAPI.Services.InquiriesService
{
    public interface IInquiriesService
    {
        IEnumerable<Inquiry> Search(InquirySearchPredicate predicate);
        Inquiry Save(Inquiry domainObject);
    }
}

using InquiryAPI.DomainObjects.Inquiries;

namespace InquiryAPI.Repositories
{
    public interface IInquiriesRepository : IRepository<Inquiry, InquirySearchPredicate>
    {
    }
}

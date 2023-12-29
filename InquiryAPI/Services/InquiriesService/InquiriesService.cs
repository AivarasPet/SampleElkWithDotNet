using InquiryAPI.DomainObjects.Inquiries;
using InquiryAPI.Repositories;

namespace InquiryAPI.Services.InquiriesService
{
    public class InquiriesService : IInquiriesService
    {
        private readonly IInquiriesRepository _inquiriesRepository;

        public InquiriesService(IInquiriesRepository inquiriesRepository)
        {
            _inquiriesRepository = inquiriesRepository;
        }

        public Inquiry Save(Inquiry domainObject)
        {
            return _inquiriesRepository.Save(domainObject);
        }

        public IEnumerable<Inquiry> Search(InquirySearchPredicate predicate)
        {
            return _inquiriesRepository.Search(predicate);
        }
    }
}

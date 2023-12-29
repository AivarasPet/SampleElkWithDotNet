using InquiryAPI.BaseDbContexts;
using InquiryAPI.DomainObjects.Inquiries;
using InquiryAPI.Entities;

namespace InquiryAPI.Repositories
{
    public class InquiriesRepository : IInquiriesRepository
    {

        private readonly InquiryDbContext _dbContext;

        public InquiriesRepository(InquiryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Inquiry GetById(Guid id)
        {
            return Search(new InquirySearchPredicate() { Id = id }).FirstOrDefault();
        }

        public Inquiry Save(Inquiry domainObject)
        {
            InquiryEntity entity = null;

            if (domainObject.IsNew)
            {
                entity = new InquiryEntity
                {
                    Id = domainObject.Id,
                };
            }
            else
            {
                entity = _dbContext
                    .InquiryEntities
                    .First(e => e.Id == domainObject.Id);
            }

            entity.Status = domainObject.Status;
            entity.Type = domainObject.Type;
            entity.Message = domainObject.Message;
            entity.UserId = domainObject.UserId;
            entity.CreationDate = domainObject.CreationDate;

            if (domainObject.IsNew)
            {
                _dbContext
                    .InquiryEntities
                    .Add(entity);
            }
            else
            {
                _dbContext
                     .InquiryEntities
                     .Update(entity);
            }

            _dbContext
                .SaveChanges();

            return GetById(entity.Id);
        }

        public IEnumerable<Inquiry> Search(InquirySearchPredicate predicate)
        {
            IQueryable<InquiryEntity> queryable = _dbContext
                .InquiryEntities
                .Where(q => (predicate.Id == null || predicate.Id == q.Id)
                    && (predicate.InquiryStatus == null || predicate.InquiryStatus.Value == q.Status)
                    && (predicate.UserId == null || predicate.UserId.Value == q.UserId)
                    && (predicate.InquiryType == null || predicate.InquiryType.Value == q.Type)
                );

            return queryable
                    .Select(entity => new Inquiry(entity.Id, entity.CreationDate, entity.Type, entity.Status, entity.Message, entity.UserId))
                    .ToList();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}

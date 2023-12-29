namespace InquiryAPI.Repositories
{
    public interface IRepository<T, TPredicate> where T : class
                                               where TPredicate : class
    {
        T GetById(Guid id);
        void Delete(Guid id);
        IEnumerable<T> Search(TPredicate predicate);
        T Save(T domainObject);
    }
}

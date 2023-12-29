using InquiryAPI.DomainObjects.Users;

namespace InquiryAPI.Repositories
{
    public interface IUsersRepository : IRepository<User, UserSearchPredicate>
    {
    }
}

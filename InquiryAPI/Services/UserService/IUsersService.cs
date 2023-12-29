using InquiryAPI.DomainObjects.Users;

namespace InquiryAPI.Services.UserService
{
    public interface IUsersService
    {
        public bool ValidateCredentials(string username, string password, out User user);
        public IEnumerable<User> Search(UserSearchPredicate predicate);
        public User Save(User domainObject);
    }
}

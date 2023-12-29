using InquiryAPI.DomainObjects.Users;
using InquiryAPI.Repositories;

namespace InquiryAPI.Services.UserService
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _userRepository;

        public UsersService(IUsersRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public IEnumerable<User> Search(UserSearchPredicate predicate)
        {
            return _userRepository.Search(predicate);
        }

        public User Save(User domainObject)
        {
            if (domainObject.IsNew)
            {
                domainObject.Password = HashPassword(domainObject.Password);
            }
            return _userRepository.Save(domainObject);
        }

        public bool ValidateCredentials(string username, string password, out User user)
        {
            IEnumerable<User> users = _userRepository.Search(new UserSearchPredicate()
            {
                Username = username,
            });

            user = users.First();

            return users.Any() && VerifyPassword(password, user.Password);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(inputPassword, hashedPassword);
        }
    }
}

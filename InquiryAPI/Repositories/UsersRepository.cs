using InquiryAPI.BaseDbContexts;
using InquiryAPI.DomainObjects.Users;
using InquiryAPI.Entities;

namespace InquiryAPI.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly InquiryDbContext _dbContext;

        public UsersRepository(InquiryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public User GetById(Guid id)
        {
            return Search(new UserSearchPredicate() { Id = id }).FirstOrDefault();
        }

        public User Save(User user)
        {
            UserEntity entity = null;

            if (user.IsNew)
            {
                entity = new UserEntity
                {
                    Id = user.Id,
                };
            }
            else
            {
                entity = _dbContext
                    .UserEntities
                    .First(e => e.Id == user.Id);
            }

            entity.Username = user.Username;
            entity.Password = user.Password;

            if (user.IsNew)
            {
                _dbContext
                    .UserEntities
                    .Add(entity);
            }
            else
            {
                _dbContext
                     .UserEntities
                     .Update(entity);
            }

            _dbContext
                .SaveChanges();

            return GetById(entity.Id);
        }

        public IEnumerable<User> Search(UserSearchPredicate predicate)
        {
            IQueryable<UserEntity> queryable = _dbContext
                .UserEntities
                .Where(q => (predicate.Id == null || predicate.Id == q.Id)
                    && (predicate.Username == null || predicate.Username == q.Username)
                    && (predicate.Password == null || predicate.Password == q.Password)
                );

            return queryable
                    .Select(entity => new User(entity.Id)
                    {
                        Username = entity.Username,
                        Password = entity.Password
                    })
                   .ToList();
        }
    }
}

namespace InquiryAPI.DomainObjects.Users
{
    public class UserSearchPredicate
    {
        public Guid? Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

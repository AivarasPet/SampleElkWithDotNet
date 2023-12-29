namespace InquiryAPI.DomainObjects.Users
{
    public class User : BaseDomainObject
    {
        public User() : base() { }
        public User(Guid id) : base(id) { }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

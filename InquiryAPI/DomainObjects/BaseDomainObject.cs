namespace InquiryAPI.DomainObjects
{
    public class BaseDomainObject
    {
        public Guid Id { get; private set; }
        protected BaseDomainObject()
        {
            Id = Guid.NewGuid();
            IsNew = true;
        }
        protected BaseDomainObject(Guid id)
        {
            Id = id;
            IsNew = false;
        }
        public bool IsNew { get; private set; }
    }
}

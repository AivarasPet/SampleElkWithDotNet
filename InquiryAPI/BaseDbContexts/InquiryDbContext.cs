using InquiryAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace InquiryAPI.BaseDbContexts
{
    public class InquiryDbContext : DbContext
    {
        public InquiryDbContext(DbContextOptions<InquiryDbContext> options) : base(options) { }
        public DbSet<InquiryEntity> InquiryEntities { get; set; }
        public DbSet<UserEntity> UserEntities { get; set; }
    }
}

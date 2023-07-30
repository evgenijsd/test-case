using Microsoft.EntityFrameworkCore;
using test_case.api.Models.Entities;

namespace test_case.api.Context
{
    public class TestCaseContext :DbContext
    {
        protected TestCaseContext() { }

        public TestCaseContext(DbContextOptions<TestCaseContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}

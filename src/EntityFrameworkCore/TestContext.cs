using Microsoft.EntityFrameworkCore;

namespace aspnetcore_outputcaching_with_redis;

public class TestContext : DbContext
{
    public TestContext(DbContextOptions<TestContext> options)
       : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }
}
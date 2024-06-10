using System.Data.Entity;

namespace Management_SYS
{
    internal class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public ApplicationContext() : base("DefaultConnection") { }

    }
}

using System.Data.Entity;

namespace Management_SYS
{
    public class ApplicationContext1 : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Contact_story> Contacts_story { get; set; }
        public ApplicationContext1() : base("DefaultConnection") { }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Management_SYS
{
    public class Contact_story
    {
        [Key]
        public int id_of_contact { get; set; }
        public int id_of_customer;
        private string time_of_contact, contact_info;
        public string Time_of_contact
        {
            get { return time_of_contact; }
            set { time_of_contact = value;}
        }
        public string Contact_info
        {
            get { return contact_info; }
            set { contact_info = value;}
        }
        public int Id_of_customer
        {
            get { return id_of_customer; }
            set { id_of_customer = value; }
        }
        public Contact_story() { }
        public Contact_story(string time_of_contact, string contact_info, int id_of_customer)
        {
            this.id_of_customer = id_of_customer;
            this.time_of_contact= time_of_contact;
            this.contact_info= contact_info;
        }
    }
}

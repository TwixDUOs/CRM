namespace Management_SYS
{
    public class Customer
    {
        public int customerID { get; set; }
        private string nameOfCustomer, phoneNumberOfCustomer, lastContact, nextContact;
        public string NameOfCustomer { 
            get { return nameOfCustomer; }
            set { nameOfCustomer = value; }
        }
        public string PhoneNumberOfCustomer { 
            get {return phoneNumberOfCustomer; } 
            set { phoneNumberOfCustomer = value; } 
        }
        public string LastContact { 
            get { return lastContact; } 
            set { lastContact = value; } 
        }
        public string NextContact
        {
            get { return nextContact; }
            set { nextContact = value; }
        }

        public Customer() { }

        public Customer(string nameOfCustomer, string phoneNumberOfCustomer, string lastContact, string nextContact)
        {
            this.NameOfCustomer = nameOfCustomer;
            this.PhoneNumberOfCustomer = phoneNumberOfCustomer;
            this.LastContact = lastContact;
            this.NextContact = nextContact;
        }
    }
}

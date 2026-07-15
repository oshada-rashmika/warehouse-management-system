using System;

namespace WareHouseApp
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string ContactNumber { get; set; }

        public Customer() { }

        public Customer(int id, string name, string contact)
        {
            this.CustomerId = id;
            this.CustomerName = name;
            this.ContactNumber = contact;
        }
    }
}

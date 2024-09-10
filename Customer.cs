using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company
{
    internal class Customer
    {

        public int Id { get; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public Customer() { }   
        public Customer(string name, string surname)
        {
            Name = name;
            Surname = surname;
        }
        public Customer(int id,string name, string surname)
        {
            Name = name;
            Surname = surname;
            Id = id;
        }
     
    }
}

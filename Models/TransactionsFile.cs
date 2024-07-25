
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AyalaMalls_Linking.Models
{
    public class TransactionDetails
    {
        public string OrderNum { get; set; }
        public string ItemID { get; set; }
        public string ItemName { get; set; }
    }
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public decimal Salary { get; set; }
        public List<EmployeeDetails> details {get;set;}
    }

    public class EmployeeDetails
    {
        public string Item { get; set; }
        public int Price { get; set; }
    }
}


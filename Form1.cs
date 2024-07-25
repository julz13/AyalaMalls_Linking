using AyalaMalls_Linking.Constants;
using AyalaMalls_Linking.Helpers;
using AyalaMalls_Linking.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AyalaMalls_Linking
{
    public partial class Form1 : Form
    {
        public List<TransactionDetails> salesDetailsList = new List<TransactionDetails>();
        public CSVHelper helper = null;
        DbAccess dbAccess = null;
        public Form1()
        {
            InitializeComponent();
            helper = new CSVHelper();
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            Employee arrayCustomers = new Employee();
            List<List<Employee>> employees = new List<List<Employee>>();
            EmployeeDetails arrayCustomersdet = new EmployeeDetails();
            List<Employee> records = new List<Employee>();
            List<EmployeeDetails> recordsdet = new List<EmployeeDetails>();
            List<List<Employee>> emp = new List<List<Employee>>();

            string filePath = Path.Combine("C:\\testAyala", "testing1.csv");

            for (int y = 1; y <= 5; y++)
            {
                recordsdet = new List<EmployeeDetails>();
                records = new List<Employee>();

                for (int x = 1; x <= y; x++)
                {

                    arrayCustomersdet = new EmployeeDetails
                    {
                        Item = String.Format("{0}-{1}", "Apple", x),
                        Price = x
                    };

                    recordsdet.Add(arrayCustomersdet);
                }

                arrayCustomers = new Employee
                {

                    Id = y,
                    Name = "John Doe",
                    Department = "Marketing",
                    Salary = 50000,
                    details = recordsdet
                };

                records.Add(arrayCustomers);
                employees.Add(records);

            }
           // helper.WriteCsv(filePath, employees);
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            string filename = @"C:\@Clients\mumuso\recent.mdb";
            string password = "";
            dbAccess = new DbAccess(filename, password);;
            string cmd = string.Format(Queries.MYOB_SALES,0, dtPicker.Value.ToString("MM/dd/yyyy"));
            DataTable dt = dbAccess.GetDataTable(cmd);
            if(dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show(dt.Rows.Count.ToSafeString());
                }
                else {
                    MessageBox.Show("No Transaction on that day....");
                }
            }
        }
    }
}

using AyalaMalls_Linking.Constants;
using AyalaMalls_Linking.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AyalaMalls_Linking.Views
{
    public partial class frmDateFilter : Form
    {

        private string salesPrinter;
        private string connectionString = null;

        public frmDateFilter()
        {
            InitializeComponent();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void PrintReceipt(string date)
        {
            try
            {
                // Restaurant name and address
                string today = date.ToString();
                string EOD = "END OF DAY RECEIPT";
                string BIRPRMT = "BIR Permit No: ";
                string SERIAL = "SERIAL No: ";
                string tDlySales = "TOTAL DLYSALE: ";
                string tDiscount = "TOTAL DISCOUNT: ";
                string tRefund = "TOTAL REFUND:";
                string tCancel = "TOTAL CANCELLATION:";
                string tSerChrg = "TOTAL SERVICE CHARGE:";
                string tNoTSale = "TOTAL NOTAXSALE:";
                string tVat = "TOTAL VAT:";
                string tCash = "TOTAL CASH:";
                string tDebit = "TOTAL DEBIT:";
                string tCredit = "TOTAL CREDIT:";
                string tMstrCard = "TOTAL MASTERCARD:";
                string tVisa = "TOTAL VISA:";

                // Create a PrintDocument object
                PrintDocument pd = new PrintDocument();

                // Set the printer name
                pd.PrinterSettings.PrinterName = salesPrinter;

                // Add event handler to PrintPage event
                pd.PrintPage += (sender, e) =>
                {
                    // Create a font Courier New
                    //Font font = new Font("Century Gothic", 10);
                    Font font = new Font("Courier New", 10);

                    // Create a brush
                    SolidBrush brush = new SolidBrush(Color.Black);

                    // Get the width of the page
                    float pageWidth = e.PageBounds.Width;

                    // Initialize y position
                    float y = 10;

                    // Print Content
                    e.Graphics.DrawString(today, font, brush, 10, y);
                    y += font.GetHeight();
                    e.Graphics.DrawString(EOD, font, brush, 10, y);
                    y += font.GetHeight();
                    e.Graphics.DrawString(BIRPRMT, font, brush, 10, y);
                    y += font.GetHeight();
                    e.Graphics.DrawString(SERIAL, font, brush, 10, y);
                    y += font.GetHeight();
                    e.Graphics.DrawString("", font, brush, 10, y);
                    y += font.GetHeight();
                    e.Graphics.DrawString(tDlySales, font, brush, 10, y);
                    y += font.GetHeight();
                    e.Graphics.DrawString(tDiscount, font, brush, 10, y);
                    y += font.GetHeight();
                    e.Graphics.DrawString(tRefund, font, brush, 10, y);
                    y += font.GetHeight();
                    e.Graphics.DrawString(tCancel, font, brush, 10, y);
                    y += font.GetHeight();
                    e.Graphics.DrawString(tSerChrg, font, brush, 10, y);
                    y += font.GetHeight();
                    e.Graphics.DrawString(tNoTSale, font, brush, 10, y);
                    y += font.GetHeight();
                    e.Graphics.DrawString(tVat, font, brush, 10, y);
                    y += font.GetHeight();
                    e.Graphics.DrawString(tCash, font, brush, 10, y);
                    y += font.GetHeight();
                    e.Graphics.DrawString(tDebit, font, brush, 10, y);
                    y += font.GetHeight();
                    e.Graphics.DrawString(tCredit, font, brush, 10, y);
                    y += font.GetHeight();
                    e.Graphics.DrawString(tMstrCard, font, brush, 10, y);
                    y += font.GetHeight();
                    e.Graphics.DrawString(tVisa, font, brush, 10, y);
                    y += font.GetHeight();

                };

                // Print the document to the default printer
                pd.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Printing error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
        }

        private void ReadData(string ConnectionString)
        {

            //if (dataGridEOD.Rows.Count == 0) {
            //    MessageBox.Show("No EOD");

            //}
            //else {

            //dataGridEOD.Rows.Clear();

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string sqlReprintEOD = string.Format(Queries.SELECT_TABLE, "dailydata");

                using (SQLiteCommand command = new SQLiteCommand(sqlReprintEOD, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            object[] row = {
                                reader["TRN_DATE"],
                                reader["STRANS"],
                                reader["ETRANS"],
                                DateTime.Parse(reader["date_EOD"].ToString())
                            };

                            dataGridEOD.Rows.Add(row);
                        }
                    }
                }
            }
            //}
        }
    }
}

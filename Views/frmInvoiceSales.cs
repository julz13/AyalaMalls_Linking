using AyalaMalls_Linking.LocalStorage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AyalaMalls_Linking.Views
{
    public partial class frmInvoiceSales : Form
    {
        public TransactionsDataStorage transactionDataStorage = null;
        public TransactionsDetailsStorage transactionDetailsStorage = null;
        private DateTime dateNow;
        public frmInvoiceSales()
        {
            InitializeComponent();
            transactionDataStorage = new TransactionsDataStorage();
            transactionDetailsStorage = new TransactionsDetailsStorage();
        }

        private void InvoiceSales_Load(object sender, EventArgs e)
        {
            dateNow = new DateTime(2023, 02, 14);
            DataTable dtInvoicesSuccess = transactionDataStorage.GetInvoiceTransactions(dateNow);
            InitializeData(dtInvoicesSuccess, dgvInvoice);
        }

        public void InitializeData(DataTable dt, DataGridView view)
        {
            if (dt == null)
            {
                return;
            }
            try
            {
                if (dt.Rows.Count > 0)
                {
                    if (dt != null)
                    {
                        view.Columns.Clear();
                        view.DataSource = dt;
                    }
                }
            }
            finally
            {

            }
        }

        private void dgvInvoice_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = dgvInvoice.CurrentRow;
            string orderNum = selectedRow.Cells["TRANSACTION_NO"].Value.ToString();
            if (!string.IsNullOrEmpty(orderNum))
            {
                DataTable dt = transactionDetailsStorage.GetInvoiceTransactions(orderNum);
                if (dt != null)
                {
                    InitializeData(dt, dgvItems);
                }
            }
        }
    }
}
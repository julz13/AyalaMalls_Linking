using AyalaMalls_Linking.Constants;
using AyalaMalls_Linking.Helpers;
using AyalaMalls_Linking.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AyalaMalls_Linking.LocalStorage
{
    public class TransactionsDetailsStorage : BaseStorage<TransactionDataDetails>
    {
        private string _tableName = new TransactionDataDetails().GetTableName();
        public TransactionsDetailsStorage() : base(Program.DatabaseFile)
        {
            CreateTable(new TransactionDataDetails());
        }
        public DataTable GetInvoiceTransactions(string OrderNum)
        {
            string query = string.Format(Queries.SELECT_TABLE_WHERE, _tableName, string.Format("(TRAN_OR) = ('{0}')", OrderNum));
            return GetDataTable(query);
        }
    }
}

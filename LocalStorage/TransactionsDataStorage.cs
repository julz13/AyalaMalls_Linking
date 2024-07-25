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
   public class TransactionsDataStorage : BaseStorage<TransactionsData>
    {
        private string _tableName = new TransactionsData().GetTableName();
        public TransactionsDataStorage() : base(Program.DatabaseFile)
        {
            CreateTable(new TransactionsData());
        }
        public DataTable GetInvoiceTransactions(DateTime processDate)
        {
            string query = string.Format(Queries.SELECT_TABLE_WHERE, _tableName, string.Format("(TRN_DATE) = date('{0}')", processDate.ToString("yyyy-MM-dd")));
            return GetDataTable(query);
        }

        public DataTable GetMINOr(DateTime processDate)
        {
            string query = string.Format(Queries.SELECT_MIN_WHERE, "TRANSACTION_NO", _tableName,string.Format("(TRN_DATE) = date('{0}')", processDate.ToString("yyyy-MM-dd")));
            return GetDataTable(query);
        }
        public DataTable GetMAXOr(DateTime processDate)
        {
            string query = string.Format(Queries.SELECT_MAX_WHERE, "TRANSACTION_NO", _tableName, string.Format("(TRN_DATE) = date('{0}')", processDate.ToString("yyyy-MM-dd")));
            return GetDataTable(query);
        }

    }
}
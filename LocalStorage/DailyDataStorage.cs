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
    public class DailyDataStorage : BaseStorage<DailyData>
    {
        private string _tableName = new DailyData().GetTableName();
        public DailyDataStorage() : base(Program.DatabaseFile)
        {
            CreateTable(new DailyData());
        }

        public DataTable GetLastEOD(DateTime processDate)
        {
           // string query = string.Format(Queries.SELECT_TABLE_DESC_LIMIT1, _tableName, "TRN_DATE");
            string query = string.Format(Queries.SELECT_TABLE_DESC_LIMIT1, _tableName, "ID");
            return GetDataTable(query);
        }

        public DataTable GetInvoiceTransactions(DateTime processDate)
        {
            string query = string.Format(Queries.SELECT_TABLE_WHERE, _tableName, string.Format("(TRN_DATE) = date('{0}')", processDate.ToString("yyyy-MM-dd")));
            return GetDataTable(query);
        }
    }
}

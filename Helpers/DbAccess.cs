using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AyalaMalls_Linking.Helpers
{
  public class DbAccess
    {

        private string DBFilename { get; set; }
        private string DBPassword { get; set; }

        public DbAccess(string filename, string password = "")
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException("filename", "Empty database filename");
            }
            if (!File.Exists(filename))
            {
                // CreateDatabaseFile(filename, password);
            }
            DBFilename = filename;
            DBPassword = password;
        }


        public bool ChangeDBPassword(string databaseFilename, string oldPassword, string newPassword)
        {
            string connString = GetConnectionString(databaseFilename, oldPassword) + "Mode = 12;"; //to enable change password, must set mode =12
            try
            {
                string cmdText = string.Format("ALTER DATABASE PASSWORD [{0}] [{1}]", newPassword, oldPassword);
                using (OleDbConnection conn = new OleDbConnection(connString))
                {
                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand(cmdText, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public string GetConnectionString()
        {
            if (String.IsNullOrEmpty(DBFilename))
            {
                throw new ArgumentNullException("Access database file is not found.");
            }
            return GetConnectionString(DBFilename, DBPassword);
        }

        public string GetConnectionString(string filename, string password = "")
        {
            string result = string.Empty;
            if (String.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException("Database file was not initialized.");
            }

            if (!File.Exists(filename))
            {
                throw new ArgumentNullException("Database file was not found.");
            }

            string passwordParam = string.Empty;
            if (!string.IsNullOrEmpty(password))
            {
                passwordParam = string.Format("Jet OLEDB:Database Password={0};", password);
            }
            FileInfo fileInfo = new FileInfo(filename);
            if (fileInfo.Extension.Equals(".xls"))
            {
                result = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'", filename);
            }
            else if (fileInfo.Extension.Equals(".xlsx"))
            {
                result = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0;HDR=YES;\"", filename);
            }
            else if (fileInfo.Extension.Equals(".mdb"))
            {
                result = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Persist Security Info=True;{1}", filename, passwordParam);
            }
            else
            {
                result = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Persist Security Info=True;{1}", filename, passwordParam);
            }
            return result;
        }

        public OleDbConnection GetConnection(string filename, string password)
        {
            string connString = GetConnectionString(filename, password);
            OleDbConnection conn = new OleDbConnection(connString);
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            return conn;
        }

        public OleDbConnection GetConnection()
        {
            string connString = GetConnectionString();
            OleDbConnection conn = new OleDbConnection(connString);
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            return conn;
        }

        public int ExecuteNonQuery(string sql)
        {
            int result = 0;
            try
            {
                using (OleDbConnection conn = GetConnection())
                {
                    using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                    {
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public string ExecuteScalar(string sql)
        {
            string result = string.Empty;
            using (OleDbConnection conn = GetConnection())
            {
                using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                {
                    object obj = cmd.ExecuteScalar();
                    result = obj.ToSafeString();
                }
            }
            return result;
        }

        public bool CreateTable(string tableName, Dictionary<string, string> data)
        {
            bool result = false;
            if (!IsTableExist(tableName))
            {
                string text = "";
                foreach (KeyValuePair<string, string> current in data)
                {
                    text += string.Format(" {0} {1},", current.Key, current.Value);
                }
                text = text.Substring(0, text.Length - 1);
                try
                {
                    string sql = string.Format("CREATE TABLE {0}({1});", tableName, text);
                    ExecuteNonQuery(sql);
                    result = true;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return result;
        }

        public bool DropTable(string tableName)
        {
            string query = string.Format("DROP TABLE {0};", tableName);
            int result = ExecuteNonQuery(query);
            return result > 0;
        }

        public bool Update(string tableName, Dictionary<string, string> data, string where)
        {
            string text = "";
            bool result = true;
            if (data.Count >= 1)
            {
                foreach (KeyValuePair<string, string> current in data)
                {
                    text += string.Format(" [{0}] = {1},", current.Key, current.Value);
                }
                text = text.Substring(0, text.Length - 1);
            }
            // try {
            string commandText = string.Format("update [{0}] set {1} where {2};", tableName, text, where);
            ExecuteNonQuery(commandText);
            result = true;
            //} catch {
            //    result = false;
            //}
            return result;
        }

        public bool Delete(string tableName, string where)
        {
            try
            {
                ExecuteNonQuery(string.Format("delete from [{0}] where {1};", tableName, where));
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Insert(string tableName, Dictionary<string, string> data)
        {
            string columns = "";
            string values = "";
            bool result = true;
            List<string> duplicateChecker = new List<string>();
            foreach (KeyValuePair<string, string> current in data)
            {
                if (!duplicateChecker.Contains(current.Key))
                {
                    duplicateChecker.Add(current.Key);

                    columns += string.Format(" [{0}],", current.Key);
                    if (String.IsNullOrEmpty(current.Value))
                    {
                        values += string.Format(" {0},", "NULL");
                    }
                    else
                    {
                        values += string.Format(" {0},", current.Value);
                    }
                }
                else
                {
                    throw new Exception(string.Format("Duplicates {0} : {1}", current.Key, current.Value));
                }
            }
            columns = columns.Substring(0, columns.Length - 1);
            values = values.Substring(0, values.Length - 1);
            string sql = string.Empty;
            try
            {
                sql = string.Format("insert into [{0}]({1}) values({2});", tableName, columns, values);
                ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                result = false;
                throw new Exception(ex.Message + " SQL : " + sql);
            }
            return result;
        }

        public bool AddColumns(string tablename, string newcolumnname, string dataType)
        {
            string sql = string.Format("ALTER TABLE {0} ADD COLUMN {1} {2}", tablename, newcolumnname, dataType);
            int result = 0;
            try
            {
                result = ExecuteNonQuery(sql);
            }
            catch
            {
                return false;
            }
            return (result > 0);
        }


        public bool IsTableExist(string tablename)
        {
            bool result = false;
            using (OleDbConnection conn = GetConnection())
            {
                DataTable schema = conn.GetSchema("Tables");
                foreach (DataRow dataRow in schema.Rows)
                {
                    if (dataRow.ItemArray[2].ToString().Equals(tablename))
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        public DataTable GetDataTable(string sql)
        {
            DataTable dt = new DataTable();
            try
            {
                using (OleDbConnection conn = GetConnection())
                {
                    using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                    {
                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            dt.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}\n Query: {1} \n DB: {2}", ex.Message, sql, DBFilename));
            }
            return dt;
        }

        public DataTable GetDataTable(string sql, string tablename)
        {
            DataTable dt = new DataTable(tablename);
            try
            {
                using (OleDbConnection conn = GetConnection())
                {
                    using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                    {
                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            dt.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return dt;
        }

        public DataSet GetDataSet(string sql)
        {
            DataSet dataSet = new DataSet();
            using (OleDbConnection conn = GetConnection())
            {
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(sql, conn))
                {
                    adapter.Fill(dataSet);
                }
            }
            return dataSet;
        }

        public DataTable GetDataTable(string sql, out OleDbDataAdapter oleDbAdapter)
        {
            DataTable result = new DataTable();
            DataSet dataSet = new DataSet();
            try
            {
                using (OleDbConnection conn = GetConnection())
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(sql, conn))
                    {
                        adapter.Fill(dataSet);
                        oleDbAdapter = adapter;
                        result = dataSet.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public DataTable GetDataTable(string sql, object filename, string password)
        {
            DataTable dt = new DataTable();
            try
            {
                using (OleDbConnection conn = GetConnection(filename.ToString(), password))
                {
                    using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                    {
                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            dt.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return dt;
        }

        public List<string> GetTables()
        {
            List<string> list = new List<string>();
            using (OleDbConnection conn = GetConnection())
            {
                DataTable schema = conn.GetSchema("Tables");
                foreach (DataRow dataRow in schema.Rows)
                {
                    string table = (string)dataRow[2];
                    if (!string.IsNullOrEmpty(table))
                    {
                        list.Add(table);
                    }
                }
                conn.Close();
            }
            return list;
        }
        public List<string> GetListTablesTrim()
        {
            List<string> list = new List<string>();
            foreach (string table in GetTables())
            {
                if (!table.Contains("~")
                       && !table.Contains("MSys"))
                {
                    list.Add(table);
                }
            }
            return list;
        }

        public string GetColumnValue(string tablename, string columnname, string condition)
        {
            string result = string.Empty;
            try
            {
                DataTable dataTable = GetDataTable(string.Format("Select {0} from {1} Where {2}", columnname, tablename, condition));
                if (dataTable != null)
                {
                    if (dataTable.Rows.Count > 0)
                    {
                        foreach (DataRow dataRow in dataTable.Rows)
                        {
                            result = dataRow[0].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }


        public List<string> GetColumns(string tablename)
        {
            List<string> list = new List<string>();
            string cmdText = string.Format("SELECT * FROM [{0}]", tablename);
            using (OleDbConnection conn = GetConnection())
            {
                using (OleDbCommand cmd = new OleDbCommand(cmdText, conn))
                {
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        for (int i = 0; i <= reader.FieldCount - 1; i++)
                        {
                            list.Add(reader.GetName(i).ToString());
                        }
                    }
                }
            }
            return list;
        }


        public bool HasColumn(DataRow row, string columnName)
        {
            try
            {
                return row.Table.Columns.Contains(columnName);
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
        }

        public bool IsFieldExist(string columnName, string tableName)
        {
            bool result = false;
            using (OleDbConnection conn = GetConnection())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                DataTable dataTable = new DataTable();
                string cmdText = "Select TOP 1 * from " + tableName;
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmdText, conn);
                adapter.Fill(dataTable);
                int num = dataTable.Columns.IndexOf(columnName);
                result = (num != -1);
                dataTable.Dispose();
                conn.Close();
                conn.Dispose();
            }
            return result;
        }

        public bool IsColumnExist(string columnName, string tableName)
        {
            bool result;
            using (OleDbConnection conn = GetConnection())
            {
                string cmdText = string.Format("TABLE_NAME='{0}' AND COLUMN_NAME='{1}'", tableName, columnName);
                DataTable schema = conn.GetSchema("COLUMNS");
                DataRow[] array = schema.Select(cmdText);
                result = (array.Length > 0);
            }
            return result;
        }

        public bool IsValueExist(string tablename, string column, string value)
        {
            string cmdText = string.Format("SELECT * FROM [{0}] WHERE {1}={2}", tablename, column, value);
            DataTable dt = GetDataTable(cmdText);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsEmpty(string tablename)
        {
            DataTable dataTable = GetDataTable("SELECT * FROM " + tablename);
            if (dataTable == null)
            {
                return true;
            }
            return dataTable.Rows.Count <= 0;
        }

        public string GetLastValue(string tableName, string columnName = "")
        {
            string cmdText = string.Format("SELECT TOP 1 ID FROM {0} ORDER BY ID DESC;", tableName);
            if (!string.IsNullOrEmpty(columnName))
            {
                cmdText = string.Format("SELECT TOP 1 {1} FROM {0} ORDER BY {1} DESC;", tableName, columnName);
            }
            DataTable dt = GetDataTable(cmdText);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][columnName].ToSafeString();
            }
            return string.Empty;
        }
    }
}
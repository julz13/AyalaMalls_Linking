using AyalaMalls_Linking.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AyalaMalls_Linking.Helpers
{
    public class CSVHelper
    {
        public void WriteCsv(string path, List<List<PerTransactionsFile>> entities, int tranCount)
        {
            bool isFirstEntry = true;
            string header = String.Empty;
            object value = String.Empty;
            string valueString = String.Empty;
            StringBuilder csvBuilder = new StringBuilder();
            List<string> headerListColumn = new List<string> { "CCCODE", "MERCHANT_NAME", "TRN_DATE", "NO_TRN" };

            foreach (List<PerTransactionsFile> entity in entities)
            {
                foreach (PerTransactionsFile emp in entity)
                {
                    foreach (PropertyInfo pi in emp.GetType().GetProperties())
                    {
                        header = pi.Name;
                        if (!header.Contains("Items"))
                        {
                            value = pi.GetValue(emp, null);
                            valueString = value.ToString();
                            if (header.Contains("CCCODE") && isFirstEntry)
                            {
                                csvBuilder.AppendLine(String.Format("{0},{1}", header, valueString));
                            }
                            else if (header.Contains("MERCHANT_NAME") && isFirstEntry)
                            {
                                csvBuilder.AppendLine(String.Format("{0},{1}", header, valueString));
                            }
                            else if (header.Contains("TRN_DATE") && isFirstEntry)
                            {
                                csvBuilder.AppendLine(String.Format("{0},{1}", header, valueString));
                            }
                            else if (header.Contains("NO_TRN") && isFirstEntry)
                            {
                                csvBuilder.AppendLine(String.Format("{0},{1}", header, tranCount));
                                isFirstEntry = false;
                            }
                            else
                            {
                                if (headerListColumn.Any(p => header.StartsWith(p)))
                                {
                                    continue;
                                }
                                else
                                {
                                    if (header.Contains("TER_NO"))
                                    {
                                        valueString = String.Format("{0}{1}", "", valueString); // workaround for writing leading 0 in terminal no
                                    }
                                    csvBuilder.AppendLine(String.Format("{0},{1}", header, valueString));
                                }
                            }
                        }
                        else
                        {
                            foreach (var empItem in emp.Items)
                            {
                                foreach (PropertyInfo piItem in empItem.GetType().GetProperties())
                                {
                                    header = piItem.Name;
                                    value = piItem.GetValue(empItem, null);
                                    valueString = value.ToString();
                                    csvBuilder.AppendLine(String.Format("{0},{1}", header, valueString));
                                }
                            }
                        }
                    }
                }
            }
            File.WriteAllText(path, csvBuilder.ToString());
        }

        public void WriteEODCsv(string path, List<List<DailySalesFile>> entities, int tranCount)
        {
            string header = String.Empty;
            object value = String.Empty;
            string valueString = String.Empty;
            StringBuilder csvBuilder = new StringBuilder();
            foreach (List<DailySalesFile> entity in entities)
            {
                foreach (DailySalesFile emp in entity)
                {
                    foreach (PropertyInfo pi in emp.GetType().GetProperties())
                    {
                        header = pi.Name;
                        value = pi.GetValue(emp, null);
                        valueString = value.ToString();
                        csvBuilder.AppendLine(String.Format("{0},{1}", header, valueString));
                    }
                }
                File.WriteAllText(path, csvBuilder.ToString());
            }
        }
    }
}

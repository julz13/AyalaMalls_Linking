using AyalaMalls_Linking.LocalStorage.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tarsier.Database.Enums;

namespace AyalaMalls_Linking.Models
{
    public class TransactionDataDetails
    {
        [DbColumn(IsIdentity = true, IsPrimary = true, AutoIncrement = true)]
        public int ID { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string QTY { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string ITEMCODE { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string PRICE { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string LDISC { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string TRAN_OR { get; set; }
    }
}

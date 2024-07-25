using AyalaMalls_Linking.LocalStorage.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tarsier.Database.Enums;

namespace AyalaMalls_Linking.Models
{
    public class TransactionsData
    {
        [DbColumn(IsIdentity = true, IsPrimary = true, AutoIncrement = true)]
        public int ID { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string CCCODE { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string MERCHANT_NAME { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string TRN_DATE { get; set; }

        [DbColumn(ColumnType = ColType.Integer, NotNull = true)]
        public int NO_TRN { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string CDATE { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string TRN_TIME { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string TER_NO { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string TRANSACTION_NO { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string GROSS_SLS { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string VAT_AMNT { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string VATABLE_SLS { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string NONVAT_SLS { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string VATEXEMPT_SLS { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string VATEXEMPT_AMNT { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string LOCAL_TAX { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string PWD_DISC { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string SNRCIT_DISC { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string EMPLO_DISC { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string AYALA_DISC { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string STORE_DISC { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string OTHER_DISC { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string REFUND_AMT { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string SCHRGE_AMT { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string OTHER_SCHR { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string CASH_SLS { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string CARD_SLS { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string EPAY_SLS { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string DCARD_SLS { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string OTHERSL_SLS { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string CHECK_SLS { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string GC_SLS { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string MASTERCARD_SLS { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string VISA_SLS { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string AMEX_SLS { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string DINERS_SLS { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string JCB_SLS { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string GCASH_SLS { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string PAYMAYA_SLS { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string ALIPAY_SLS { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string WECHAT_SLS { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string GRAB_SLS { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string FOODPANDA_SLS { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string MASTERDEBIT_SLS { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string VISADEBIT_SLS { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string PAYPAL_SLS { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string ONLINE_SLS { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string OPEN_SALES { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string OPEN_SALES_2 { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string OPEN_SALES_3 { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string OPEN_SALES_4 { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string OPEN_SALES_5 { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string OPEN_SALES_6 { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string OPEN_SALES_7 { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string OPEN_SALES_8 { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string OPEN_SALES_9 { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string OPEN_SALES_10 { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string OPEN_SALES_11 { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string GC_EXCESS { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string MOBILE_NO { get; set; }

        [DbColumn(ColumnType = ColType.Integer, NotNull = true)]
        public int NO_CUST { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string TRN_TYPE { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string SLS_FLAG { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string VAT_PCT { get; set; }

        [DbColumn(ColumnType = ColType.Text, NotNull = true)]
        public string QTY_SLD { get; set; }

    }
}

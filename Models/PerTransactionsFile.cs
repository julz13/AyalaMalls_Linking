using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AyalaMalls_Linking.Models
{
    public class PerTransactionsFile
    {
        public string CCCODE { get; set; }
        public string MERCHANT_NAME { get; set; }
        public string TRN_DATE { get; set; }
        public int NO_TRN { get; set; }
        public string CDATE { get; set; }
        public string TRN_TIME { get; set; }
        public string TER_NO { get; set; }
        public string TRANSACTION_NO { get; set; }
        public string GROSS_SLS { get; set; }
        public string VAT_AMNT { get; set; }
        public string VATABLE_SLS { get; set; }
        public string NONVAT_SLS { get; set; }
        public string VATEXEMPT_SLS { get; set; }
        public string VATEXEMPT_AMNT { get; set; }
        public string LOCAL_TAX { get; set; }
        public string PWD_DISC { get; set; }
        public string SNRCIT_DISC { get; set; }
        public string EMPLO_DISC { get; set; }
        public string AYALA_DISC { get; set; }
        public string STORE_DISC { get; set; }
        public string OTHER_DISC { get; set; }
        public string REFUND_AMT { get; set; }
        public string SCHRGE_AMT { get; set; }
        public string OTHER_SCHR { get; set; }
        public string CASH_SLS { get; set; }
        public string CARD_SLS { get; set; }
        public string EPAY_SLS { get; set; }
        public string DCARD_SLS { get; set; }
        public string OTHERSL_SLS { get; set; }
        public string CHECK_SLS { get; set; }
        public string GC_SLS { get; set; }
        public string MASTERCARD_SLS { get; set; }
        public string VISA_SLS { get; set; }
        public string AMEX_SLS { get; set; }
        public string DINERS_SLS { get; set; }
        public string JCB_SLS { get; set; }
        public string GCASH_SLS { get; set; }
        public string PAYMAYA_SLS { get; set; }
        public string ALIPAY_SLS { get; set; }
        public string WECHAT_SLS { get; set; }
        public string GRAB_SLS { get; set; }
        public string FOODPANDA_SLS { get; set; }
        public string MASTERDEBIT_SLS { get; set; }
        public string VISADEBIT_SLS { get; set; }
        public string PAYPAL_SLS { get; set; }
        public string ONLINE_SLS { get; set; }
        public string OPEN_SALES { get; set; }
        public string OPEN_SALES_2 { get; set; }
        public string OPEN_SALES_3 { get; set; }
        public string OPEN_SALES_4 { get; set; }
        public string OPEN_SALES_5 { get; set; }
        public string OPEN_SALES_6 { get; set; }
        public string OPEN_SALES_7 { get; set; }
        public string OPEN_SALES_8 { get; set; }
        public string OPEN_SALES_9 { get; set; }
        public string OPEN_SALES_10 { get; set; }
        public string OPEN_SALES_11 { get; set; }
        public string GC_EXCESS { get; set; }
        public string MOBILE_NO { get; set; }
        public int NO_CUST { get; set; }
        public string TRN_TYPE { get; set; }
        public string SLS_FLAG { get; set; }
        public string VAT_PCT { get; set; }
        public string QTY_SLD { get; set; }
        public List<PerTransactionsItem> Items { get; set; }

    }
    public class PerTransactionsItem
    {
        public string QTY { get; set; }
        public string ITEMCODE { get; set; }
        public string PRICE { get; set; }
        public string LDISC { get; set; }

    }

    public class PerTransactionsItemList
    {
        public List<PerTransactionsItem> transactionDetails { get; set; }
    }

}

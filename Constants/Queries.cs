using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AyalaMalls_Linking.Constants
{
   public class Queries
    {
        //sql Lite
        public const string SELECT_TABLE = "SELECT * FROM {0}";
        public const string SELECT_TABLE_WHERE = "SELECT * FROM {0} WHERE {1}";
        public const string SELECT_TABLE_ASC = "SELECT * FROM {0} ORDER BY {1} ASC";
        public const string SELECT_TABLE_DESC = "SELECT * FROM {0} ORDER BY {1} DESC";
        public const string SELECT_TABLE_DESC_LIMIT1 = "SELECT * FROM {0} ORDER BY {1} DESC LIMIT 1";
        public const string SELECT_TABLE_WHERE_LIMIT = "SELECT * FROM {0} WHERE {1} ORDER BY id DESC, ID LIMIT {2}";
        public const string SELECT_TABLE_WHERE_LIMIT1 = "SELECT * FROM {0} WHERE {1} ORDER BY id DESC, ID LIMIT 1";
        public const string SELECT_EXCEL = "Select * from [{0}$]";
        public const string SELECT_MIN_WHERE = "SELECT MIN({0}) AS STARTOR FROM {1} WHERE {2}";
        public const string SELECT_MAX_WHERE = "SELECT MAX({0}) AS ENDOR FROM {1} WHERE {2}";

        //wbox paradox queries
        //public const string OrderList = @"Select * From {0} Where AcDate = #{1:MM/dd/yyyy}#";

        public const string OrderList = @"Select * From {0} Where TableNo NOT IN ('CASH IN', 'CASH OUT') AND AcDate = #{1:MM/dd/yyyy}#";
        public const string GET_PAYMENTAPPLIED = @"Select CAmount,VAmount,AAmount,MAmount,QAmount,IAmount,DAmount,UAmount,YAmount,OAmount From {0} Where {0}.OrderNo = '{1}'"; // payments
        public const string GET_PAYMENTNAME = @"Select Name2 From Defpay Where Remark = '{0}'";
        public const string GET_SALESDISCOUNT = @"Select Number3, DiscType From {0} Where {0}.OrderNo = '{1}' And Posted = -1 And Void = 0 And AcDate = #{2:MM/dd/yyyy}#";
        public const string GET_ORDERITEMS = @"Select * From {0} Where {0}.OrderNo = '{1}'";
        public const string GET_PAYMENT = @"Select Name2 From Defpay";
        public const string GET_TABLES = @"Select TableNo From Tables";
        public const string GET_OTHERPAYMENT = @"Select Name2 From Defpay2";
        public const string GET_DISCOUNT = @"Select Name1 From DefDisc";
        public const string GET_TRANTYPE = @"Select {0}.TableNo,{0}.AreaID,{1}.AreaName From {0} INNER JOIN {1} ON {0}.AreaID = {1}.AreaID Where {0}.TableNo = '{2}'";

        //MYOB
        //public const string MYOB_SALES = @"SELECT Docket.docket_id, DateValue(Docket.docket_date) AS docket_date, Docket.transaction AS tranType, Docket.custom, Customer.barcode " +
        //   " AS customer_code, DocketLine.stock_id, DocketLine.quantity, DocketLine.sell_inc, DocketLine.print_inc " +
        //   " FROM (Docket INNER JOIN Customer ON Docket.customer_id = Customer.customer_id) INNER JOIN DocketLine ON Docket.docket_id = DocketLine.docket_id " +
        //   " WHERE(((Docket.docket_id)>0) AND((Docket.transaction)= 'SA') AND ((DateValue([Docket].[docket_date]))=#{1}#)) ORDER BY Customer.customer_id";

        //public const string MYOB_SALES = @"SELECT Docket.docket_id, DateValue(Docket.docket_date) AS docket_date, Docket.transaction AS tranType, Docket.custom, " +
        //          "  Customer.customer_id AS customer, DocketLine.stock_id, DocketLine.quantity, DocketLine.sell_inc, DocketLine.print_inc, Docket.discount AS Dscnt, " +
        //          "  Docket.total_inc AS GrossSales, Docket.total_ex AS VatExempt, GrossSales - VatExempt AS VATamount FROM (Docket INNER JOIN Customer ON " +
        //          "  Docket.customer_id = Customer.customer_id) INNER JOIN DocketLine ON Docket.docket_id = DocketLine.docket_id WHERE(((Docket.docket_id)>0) " +
        //          "  AND ((Docket.transaction)= 'SA') AND ((DateValue([Docket].[docket_date]))=#{1}#)) ORDER BY Customer.customer_id";

        public const string MYOB_SALES = @"SELECT " +
                                           " Docket.docket_id, " +
                                           " Docket.docket_date AS docket_date, " +
                                           " Docket.transaction AS tranType, " +
                                           " Docket.custom, " +
                                           " Customer.customer_id AS customer, " +
                                           " DocketLine.stock_id, " +
                                           " SUM(DocketLine.quantity) AS quantity, " +
                                           " SUM(DocketLine.sell_inc) AS sell_inc, " +
                                           " SUM(DocketLine.print_inc) AS print_inc, " +
                                           " Docket.discount AS Dscnt, " +
                                           " Docket.total_inc AS GrossSales, " +
                                           " Docket.total_ex AS VatExempt, " +
                                           " Docket.total_inc - Docket.total_ex AS VATamount, " +
                                           " Payments.paymenttype AS Paymode " +
                                        " FROM " +
                                           " (((Docket " +
                                           " INNER JOIN Customer ON Docket.customer_id = Customer.customer_id) " +
                                           " INNER JOIN DocketLine ON Docket.docket_id = DocketLine.docket_id) " +
                                           " INNER JOIN Payments ON DocketLine.docket_id = Payments.docket_id) " +
                                        " WHERE " +
                                           " Docket.docket_id > 0 " +
                                           " AND Docket.transaction = 'SA' " +
                                           " AND DateValue(Docket.docket_date) = #2024-03-21# " +
                                        " GROUP BY " +
                                           " Docket.docket_id, " +
                                           " Docket.docket_date, " +
                                           " Docket.transaction, " +
                                           " Docket.custom, " +
                                           " Customer.customer_id, " +
                                           " DocketLine.stock_id, " +
                                           " Docket.discount, " +
                                           " Docket.total_inc, " +
                                           " Docket.total_ex, " +
                                           " Payments.paymenttype " +
                                        " ORDER BY " +
                                           " Customer.customer_id"; 

        public const string MYOB_ITEMS = @"Select {0}.quantity AS qty, {0}.stock_id AS StockCode, * From {0} INNER JOIN {2} ON {0}.stock_id = {2}.stock_id Where {0}.docket_id = {1}";

        public const string MYOB_PAYMENTAPPLIED = @"Select * From {0} Where {0}.docket_id = {1} GROUP BY Payments.docket_id";
        public const string MYOB_PAYMODE = @"SELECT " +
                                               " SUM({0}.amount) AS TotalAmount " +
                                               //" {0}.docket_id, " +
                                               //" paymenttype AS PayMode " +
                                            " FROM {0} " +
                                               " WHERE {0}.docket_id = {1} " +
                                               " GROUP BY {0}.docket_id, paymenttype";
    }
}

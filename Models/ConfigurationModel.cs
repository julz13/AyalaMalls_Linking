using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AyalaMalls_Linking.Models
{
    public class ConfigurationModel
    {
        //Fields Store Config Info
        private string companyCode;
        private string merchantName;
        private string terminalNo;
        private string databaseLocation;
        private string databasePassword;
        private string discountType;
        private string shopfront;

        //Fields Sale Config Info
        private string salesLocation;
        private string salesPrinter;
        private string salesBIR;
        private string salesPOSSerial;

        //Properties -Validations for Store Info
        [DisplayName("Company Code")]
        [Required(ErrorMessage = "Company Code is required.")]
        public string CompanyCode
        {
            get { return companyCode; }
            set { companyCode = value; }
        }
        [DisplayName("Merchant Name")]
        [Required(ErrorMessage = "Merchant Name is required.")]
        public string MerchantName
        {
            get { return merchantName; }
            set { merchantName = value; }
        }
        [DisplayName("Terminal Number")]
        [Required(ErrorMessage = "POS Terminal number is required.")]
        public string TerminalNumber
        {
            get { return terminalNo; }
            set { terminalNo = value; }
        }
        [DisplayName("Database Location")]
        [Required(ErrorMessage = "Database Location is required.")]
        public string DatabaseLocation
        {
            get { return databaseLocation; }
            set { databaseLocation = value; }
        }
        [DisplayName("Database password")]
        public string DatabasePassword
        {
            get { return databasePassword; }
            set { databasePassword = value; }
        }
        [DisplayName("Discount Type")]
        [Required(ErrorMessage = "Discount type is required.")]
        public string DiscountType
        {
            get { return discountType; }
            set { discountType = value; }
        }
        [DisplayName("Shopfront")]
        [Required(ErrorMessage = "Shopfront Name is required.")]
        public string Shopfront
        {
            get { return shopfront; }
            set { shopfront = value; }
        }

        //Properties -Validations for Sales Info
        [DisplayName("Sales Location")]
        [Required(ErrorMessage = "Sales Location is required.")]
        public string SalesLocation
        {
            get { return salesLocation; }
            set { salesLocation = value; }
        }
        [DisplayName("Sales Printer")]
        [Required(ErrorMessage = "Sales Printer is required.")]
        public string SalesPrinter
        {
            get { return salesPrinter; }
            set { salesPrinter = value; }
        }
        [DisplayName("BIR Permit No")]
        public string SalesBIR
        {
            get { return salesBIR; }
            set { salesBIR = value; }
        }
        [DisplayName("POS Serial No")]
        public string SalesPOSSerial
        {
            get { return salesPOSSerial; }
            set { salesPOSSerial = value; }
        }

    }
    public class ConfigurationModelConfig
    {
        public List<ConfigurationModel> configurationModels { get; set; }
    }

    public class PaymentModel
    {
        private string wboxPayment { get; set; }
        private string mallPayment { get; set; }

        [DisplayName("Wbox Payment")]
        [Required(ErrorMessage = "Wbox payment is required")]
        public string WboxPayment
        {
            get { return wboxPayment; }
            set { wboxPayment = value; }
        }
        [DisplayName("Mall Payment")]
        [Required(ErrorMessage = "MallPayment payment is required")]
        public string MallPayment
        {
            get { return mallPayment; }
            set { mallPayment = value; }
        }
    }
    public class PaymentModelConfig
    {
        public List<PaymentModel> paymentModels { get; set; }
    }

    public class DiscountModel
    {
        private string wboxDiscount;
        private string mallDiscount;

        [DisplayName("Wbox Discount")]
        [Required(ErrorMessage = "Wbox discount is required")]
        public string WboxDiscount
        {
            get { return wboxDiscount; }
            set { wboxDiscount = value; }
        }
        [DisplayName("Mall Discount")]
        [Required(ErrorMessage = "MallPayment discount is required")]
        public string MallDiscount
        {
            get { return mallDiscount; }
            set { mallDiscount = value; }
        }

    }
    public class DiscountModelConfig
    {
        public List<DiscountModel> discountModels { get; set; }
    }

    public class PassCodeModel
    {
        public string eodPassword { get; set; }
    }

    public class PassCodeModelConfig
    {
        public List<PassCodeModel> EodPassModels { get; set; }
    }

    public static class GlobalVar
    {
        public static string inputPassword { get; set; }
    }
}
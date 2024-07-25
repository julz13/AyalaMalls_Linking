using AyalaMalls_Linking.Constants;
using AyalaMalls_Linking.Helpers;
using AyalaMalls_Linking.LocalStorage;
using AyalaMalls_Linking.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AyalaMalls_Linking.Views
{
    public partial class frmConfig : Form
    {
        public SettingsWriter _settings = null;
        public SettingsWriter _settingsEOD = null;
        public DbParadox _dbParadox = null;
        public DbAccess _dbAccess = null;
        public List<ConfigurationModel> _system = new List<ConfigurationModel>();
        public ConfigurationModelConfig _systemConfig = null;
        public List<PassCodeModel> _systemEodPass = new List<PassCodeModel>();
        public PassCodeModelConfig _systemEodPassConfig = null;
        private string discType;

        //payment
        public List<PaymentModel> _payment = new List<PaymentModel>();
        public PaymentModelConfig _paymentConfig = null;
        //discount
        public List<DiscountModel> _discount = new List<DiscountModel>();
        public DiscountModelConfig _discountConfig = null;

        //private string _databaseFileName = Path.Combine(Application.StartupPath, TempDBName.DATABASE);
        //public TransactionsDataStorage transactionsDataStorage = null;
        //public DailyDataStorage dailyDataStorage = null;
        //public TransactionsDetailsStorage transactionsDetailsStorage = null;

        public frmConfig()
        {
            InitializeComponent();
            InitializePrinter();
            InitializeSystem();
            InitializePayment();
            InitializeDiscount();
            _settings = new SettingsWriter(Path.Combine(Application.StartupPath, "Settings"), false);
            _settingsEOD = new SettingsWriter(Path.Combine(Application.StartupPath, "Settings"), true);
            // InitializeTempDB();
        }
        //private void InitializeTempDB()
        //{
        //    Program.DatabaseFile = _databaseFileName;
        //    transactionsDataStorage = new TransactionsDataStorage();
        //    dailyDataStorage = new DailyDataStorage();
        //    transactionsDetailsStorage = new TransactionsDetailsStorage();
        //}
        private void InitializePrinter()
        {
            cboxPrinter.Items.Clear();
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                cboxPrinter.Items.Add(printer);
            }
        }

        private void InitializeSystem()
        {
           
            if (!InitializeConfiguration())
            {
                //MessageBox.Show("System setting is not yet configured.", "Invalid Configuration", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                return;
            }
            if (!string.IsNullOrEmpty(txtDataLoc.Text))
            {
                //_dbParadox = new DbParadox(txtDataLoc.Text.Trim(), txtDataPassword.Text.Trim());
                _dbAccess = new DbAccess(txtDataLoc.Text.Trim(), txtDataPassword.Text.Trim());

            }
        }

        private void InitializePayment()
        {
            _paymentConfig = _settings.Read<PaymentModelConfig>("PaymentConfig");
            if (_paymentConfig == null)
            {
                _paymentConfig = new PaymentModelConfig();
            }
            else
            {
                _payment = _paymentConfig.paymentModels;

                if (_payment == null)
                {
                    _payment = new List<PaymentModel>();
                }
                else
                {
                    foreach (PaymentModel payment in _payment)
                    {
                        dgvPayment.Rows.Add(payment.WboxPayment, payment.MallPayment);
                    }
                }
            }
        }
        private void InitializeDiscount()
        {
            _discountConfig = _settings.Read<DiscountModelConfig>("DiscountConfig");
            if (_discountConfig == null)
            {
                _discountConfig = new DiscountModelConfig();
            }
            else
            {
                _discount = _discountConfig.discountModels;

                if (_discount == null)
                {
                    _discount = new List<DiscountModel>();
                }
                else
                {
                    foreach (DiscountModel discount in _discount)
                    {
                        dgvDiscount.Rows.Add(discount.WboxDiscount, discount.MallDiscount);
                    }
                }
            }
        }

        public bool InitializeConfiguration()
        {
            _settings = new SettingsWriter(Path.Combine(Application.StartupPath, "Settings"), false);
            _settingsEOD = new SettingsWriter(Path.Combine(Application.StartupPath, "Settings"), true);
            bool validConfig = true;
            if (_settings == null)
            {
                _systemConfig = new ConfigurationModelConfig();
            }
            else
            {
                _systemConfig = _settings.Read<ConfigurationModelConfig>("SystemConfig");
                if (_systemConfig == null)
                {
                    validConfig = false;
                }
            }

            if (_settingsEOD == null)
            {
                _systemEodPassConfig = new PassCodeModelConfig();
            }
            else
            {
                _systemEodPassConfig = _settingsEOD.Read<PassCodeModelConfig>("PassEOD.dll");
                if (_systemEodPassConfig == null)
                {
                    txtPassword.Text = "";
                }
                else
                {
                    _systemEodPass = _systemEodPassConfig.EodPassModels;
                    if (_systemEodPass != null)
                    {
                        foreach (PassCodeModel configuration in _systemEodPass)
                        {
                            txtPassword.Text = configuration.eodPassword;
                        }
                    }
                }
            }

            if (validConfig)
            {
                _system = _systemConfig.configurationModels;
                if (_system != null)
                {
                    foreach (ConfigurationModel configuration in _system)
                    {
                        txtCompanyCode.Text = configuration.CompanyCode;
                        txtMerchantName.Text = configuration.MerchantName;
                        txtTerminalNo.Text = configuration.TerminalNumber;
                        txtDataLoc.Text = configuration.DatabaseLocation;
                        txtDataPassword.Text = configuration.DatabasePassword;
                        txtSalesLocation.Text = configuration.SalesLocation;
                        cboxPrinter.Text = configuration.SalesPrinter;
                        txtSalesBIR.Text = configuration.SalesBIR;
                        txtShopfront.Text = configuration.Shopfront;
                        txtSalesPOSSerial.Text = configuration.SalesPOSSerial;
                        switch (configuration.DiscountType)
                        {
                            case "ByGroup":
                                chkByGroup.Checked = true;
                                chkByItem.Checked = false;
                                break;
                            case "ByItem":
                                chkByItem.Checked = true;
                                chkByGroup.Checked = false;
                                break;
                        }
                    }
                }
            }

            return validConfig;
        }

        private void btnSystemSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (chkByGroup.Checked)
                {
                    discType = "ByGroup";
                }
                else if (chkByItem.Checked)
                {
                    discType = "ByItem";
                }

                var model = new ConfigurationModel();
                model.CompanyCode = txtCompanyCode.Text.Trim();
                model.MerchantName = txtMerchantName.Text.Trim();
                model.TerminalNumber = txtTerminalNo.Text.Trim();
                model.DatabaseLocation = txtDataLoc.Text.Trim();
                model.DatabasePassword = txtDataPassword.Text.Trim();
                model.DiscountType = discType;
                model.Shopfront = txtShopfront.Text.Trim();
                model.SalesLocation = txtSalesLocation.Text.Trim();
                model.SalesPrinter = cboxPrinter.Text.Trim();
                new ModelDataValidation().Validate(model);


                _system = new List<ConfigurationModel>();
                _system.Add(new ConfigurationModel()
                {
                    CompanyCode = txtCompanyCode.Text.Trim(),
                    MerchantName = txtMerchantName.Text.Trim(),
                    TerminalNumber = txtTerminalNo.Text.Trim(),
                    DatabaseLocation = txtDataLoc.Text.Trim(),
                    DatabasePassword = txtDataPassword.Text.Trim(),
                    DiscountType = discType,
                    SalesLocation = txtSalesLocation.Text.Trim(),
                    SalesPrinter = cboxPrinter.SelectedItem.ToString(),
                    SalesBIR = txtSalesBIR.Text.Trim(),
                    Shopfront = txtShopfront.Text.Trim(),
                    SalesPOSSerial = txtSalesPOSSerial.Text.Trim()

            });
                _systemConfig = new ConfigurationModelConfig();
                _systemConfig.configurationModels = _system;
                _settings.Save<ConfigurationModelConfig>(_systemConfig, "SystemConfig");

                //EodPassword
                _systemEodPass = new List<PassCodeModel>();
                _systemEodPass.Add(new PassCodeModel()
                {
                    eodPassword = txtPassword.Text.Trim()
                });
                _systemEodPassConfig = new PassCodeModelConfig();
                _systemEodPassConfig.EodPassModels = _systemEodPass;
                _settingsEOD.Save<PassCodeModelConfig>(_systemEodPassConfig, "PassEOD.dll");

                ////payment
                //var payModel = new PaymentModel();
                //_payment = new List<PaymentModel>();
                //foreach (DataGridViewRow row in dgvPayment.Rows)
                //{
                //    if (row.Cells["WboxPayment"].Value == null || row.Cells["MallPayment"].Value == null)
                //    {
                //        continue;
                //    }
                //    _payment.Add(new PaymentModel()
                //    {
                //        WboxPayment = row.Cells["WboxPayment"].Value.ToString(),
                //        MallPayment = row.Cells["MallPayment"].Value.ToString()
                //    });
                //}
                //_paymentConfig = new PaymentModelConfig();
                //_paymentConfig.paymentModels = _payment;
                //_settings.Save<PaymentModelConfig>(_paymentConfig, "PaymentConfig");

                //discount
                var discmodel = new DiscountModel();
                _discount = new List<DiscountModel>();
                foreach (DataGridViewRow row in dgvDiscount.Rows)
                {
                    if (row.Cells["WboxDiscount"].Value == null || row.Cells["MallDiscount"].Value == null)
                    {
                        continue;
                    }
                    _discount.Add(new DiscountModel()
                    {
                        WboxDiscount = row.Cells["WboxDiscount"].Value.ToString(),
                        MallDiscount = row.Cells["MallDiscount"].Value.ToString()
                    });
                }
                _discountConfig = new DiscountModelConfig();
                _discountConfig.discountModels = _discount;
                _settings.Save<DiscountModelConfig>(_discountConfig, "DiscountConfig");
                string sf = model.Shopfront;
                getMYOBPayments(sf);

                MessageBox.Show("System setting saved.", "System Configured", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void getMYOBPayments(string shopName)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(String.Format(@"Software\VB and VBA Program Settings\RetailManager\{0}\Payment Types\", shopName));
            if (key != null)
            {
                foreach (var x in key.GetSubKeyNames())
                {
                    ////MessageBox.Show(x.Substring(3));
                    //cbCash.Items.Add(x.Substring(3));
                    //cbOther.Items.Add(x.Substring(3));
                    //cbCheck.Items.Add(x.Substring(3));
                    //cbGC.Items.Add(x.Substring(3));
                    //cbMSCard.Items.Add(x.Substring(3));
                    //cbVisa.Items.Add(x.Substring(3));
                    //cbAmex.Items.Add(x.Substring(3));
                    //cbDiners.Items.Add(x.Substring(3));
                    //cbJCB.Items.Add(x.Substring(3));
                    //cbGcash.Items.Add(x.Substring(3));
                    //cbPaymaya.Items.Add(x.Substring(3));
                    //cbAlipay.Items.Add(x.Substring(3));
                    //cbWechat.Items.Add(x.Substring(3));
                    //cbGrab.Items.Add(x.Substring(3));
                    //cbFoodPnd.Items.Add(x.Substring(3));
                    //cbMSDebit.Items.Add(x.Substring(3));
                    //cbVisaDebit.Items.Add(x.Substring(3));
                    //cbPaypal.Items.Add(x.Substring(3));
                    //cbOnline.Items.Add(x.Substring(3));
                    dgvPayment.Rows.Add(x.Substring(3));
                }
            }
        }

        private void LoadPaymentsFromWbox(DbParadox db)
        {
            DataTable dtPayment = db.GetDataTable(string.Format(Queries.GET_PAYMENT));
            if (dtPayment != null)
            {
                foreach (DataRow dr in dtPayment.Rows)
                {
                    dgvPayment.Rows.Add(dr["Name2"]);
                }
            }
            //otherPayment
            DataTable dtOtherPayment = db.GetDataTable(string.Format(Queries.GET_OTHERPAYMENT));
            if (dtOtherPayment != null)
            {
                foreach (DataRow dr in dtOtherPayment.Rows)
                {
                    dgvPayment.Rows.Add(dr["Name2"]);
                }
            }

        }

        private void btnBrowseData_Click(object sender, EventArgs e)
        {
            //using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            //{
            //    fbd.Description = "Browse location folder for wbox database";
            //    if (fbd.ShowDialog().Equals(DialogResult.OK))
            //    {
            //        txtDataLoc.Text = fbd.SelectedPath;
            //    }
            //}

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select a File";
            openFileDialog.InitialDirectory = @"C:\";
            //openFileDialog.Filter = "recent|*.mdb|All Files|*.*";
            openFileDialog.Filter = "RM Database|recent.mdb";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog.FileName;
                txtDataLoc.Text = selectedFilePath;

                //MessageBox.Show("Selected File Path: " + selectedFilePath);
            }
            else
            {
                MessageBox.Show("Operation Canceled");
            }

            //using (OpenFileDialog ofd = new OpenFileDialog()) {
            //    ofd.Title = "Select a File";
            //    ofd.InitialDirectory = @"C:\";
            //    //openFileDialog.Filter = "recent|*.mdb|All Files|*.*";
            //    ofd.Filter = "RM Database|recent.mdb";
            //    if (ofd.ShowDialog() == DialogResult.OK)
            //    {
            //        string selectedFilePath = ofd.FileName;
            //        //txtShopfront.Text = selectedFilePath;
            //        txtDataLoc.Text = selectedFilePath;

            //        MessageBox.Show("Selected File Path: " + selectedFilePath);
            //    }
            //    else
            //    {
            //        MessageBox.Show("Operation Canceled");
            //    }

            //}
        }

        private void btnBrowseSalesLoc_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Browse destination folder for CSV sales files";
                if (fbd.ShowDialog().Equals(DialogResult.OK))
                {
                    txtSalesLocation.Text = fbd.SelectedPath;
                }
            }
        }

        private void DiscountTypeOnOff(object sender, EventArgs e)
        {
            CheckBox obj = sender as CheckBox;
            var _tag = obj.Tag;
            switch (_tag)
            {
                case "GROUP":
                    if (chkByGroup.Checked == false)
                    {
                        chkByItem.Checked = true;
                    }
                    else
                    {
                        chkByItem.Checked = false;
                    }
                    break;
                case "ITEM":
                    if (chkByItem.Checked == false)
                    {
                        chkByGroup.Checked = true;
                    }
                    else
                    {
                        chkByGroup.Checked = false;
                    }
                    break;
                case "SHOWPASS":
                    if (chkShowPass.Checked == false)
                    {
                        txtPassword.UseSystemPasswordChar = true;
                    }
                    else
                    {
                        txtPassword.UseSystemPasswordChar = false;
                    } 
                    break;
            }
        }

        private void picreloadPayments_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to continue reloading of payment types?", "Reload Payment Types", MessageBoxButtons.YesNo, MessageBoxIcon.Question, true ? MessageBoxDefaultButton.Button2 : MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                dgvPayment.Rows.Clear();
                //LoadPaymentsFromWbox(_dbParadox);
                //LoadPaymentsFromWbox(_dbAccess);
            }
            else
            {
                return;
            }
        }

        private void picreloadDiscount_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to continue reloading of discount types?", "Reload Discount Types", MessageBoxButtons.YesNo, MessageBoxIcon.Question, true ? MessageBoxDefaultButton.Button2 : MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                dgvDiscount.Rows.Clear();
                LoadDiscountsFromWbox(_dbParadox);

            }
            else
            {
                return;
            }
        }

        private void LoadDiscountsFromWbox(DbParadox db)
        {
            DataTable dtDiscount = db.GetDataTable(string.Format(Queries.GET_DISCOUNT));
            if (dtDiscount != null)
            {
                foreach (DataRow dr in dtDiscount.Rows)
                {
                    dgvDiscount.Rows.Add(dr["Name1"]);
                }
            }
        }

        private void cboxPrinter_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabSales_Click(object sender, EventArgs e)
        {

        }

        private void dgvPayment_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tabPayment_Click(object sender, EventArgs e)
        {

        }
    }
}

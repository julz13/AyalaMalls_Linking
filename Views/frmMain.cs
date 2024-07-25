using AyalaMalls_Linking.Constants;
using AyalaMalls_Linking.Dialogs;
using AyalaMalls_Linking.Helpers;
using AyalaMalls_Linking.LocalStorage;
using AyalaMalls_Linking.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using Tarsier.Extensions;

namespace AyalaMalls_Linking.Views
{
    public partial class frmMain : Form
    {
        #region 
        public SettingsWriter _settings = null;
        public SettingsWriter _settingsEOD = null;
        public DbParadox _dbParadox = null;
        public DbAccess _dbAccess = null;
        public ConfigurationModelConfig _systemConfig = null;
        public List<ConfigurationModel> _system = new List<ConfigurationModel>();
        public List<PassCodeModel> _systemEodPass = new List<PassCodeModel>();
        public PassCodeModelConfig _systemEodPassConfig = null;
        public CSVHelper helper = null;
        private string companyCode;
        private string merchantName;
        private string terminalNumber;
        private string databaseLocation;
        private string databasePassword;
        private string discountType;
        private string salesLocation;
        private string salesPrinter;
        private string salesBIR;
        private string salesPOSSerial;
        private string salesFolder;
        private string yearFolder;
        private string path;
        private int lastHour;
        private DateTime lastDate;
        private bool _isBackup;
        StringManipulation sm = new StringManipulation();
        PrintClass printer = new PrintClass();

        private System.Timers.Timer _timer;
        private BackgroundWorker _worker;

        PerTransactionsItem arrayOrderItems = new PerTransactionsItem();
        List<PerTransactionsItem> ListOrderItems = new List<PerTransactionsItem>();
        PerTransactionsFile arrayOrder = new PerTransactionsFile();
        List<List<PerTransactionsFile>> perTransactionsList = new List<List<PerTransactionsFile>>();
        List<PerTransactionsFile> ListOrder = new List<PerTransactionsFile>();


        DailySalesFile arrayOrderEOD = new DailySalesFile();
        List<List<DailySalesFile>> perTransactionsListEOD = new List<List<DailySalesFile>>();
        List<DailySalesFile> ListOrderEOD = new List<DailySalesFile>();

        private string _databaseFileName = Path.Combine(Application.StartupPath, TempDBName.DATABASE);
        public TransactionsDataStorage transactionDataStorage = null;
        public TransactionsDetailsStorage transactionDetailsStorage = null;
        public DailyDataStorage dailyData = null;
        public List<PaymentModel> _payment = new List<PaymentModel>();
        public PaymentModelConfig _paymentConfig = null;
        public List<DiscountModel> _discount = new List<DiscountModel>();
        public DiscountModelConfig _discountConfig = null;
        #endregion

        #region perInvoiceVariables
        private string locVarCCCODE;
        private string locVarMERCHANT_NAME;
        private string locVarTRN_DATE;
        private int locVarNO_TRN;
        private string locVarCDATE;
        private string locVarTRN_TIME;
        private string locVarTER_NO;
        private string locVarTRANSACTION_NO;
        private decimal locVarGROSS_SLS;
        private decimal locVarVAT_AMNT;
        private decimal locVarVATABLE_SLS;
        private decimal locVarNONVAT_SLS;
        private decimal locVarVATEXEMPT_SLS;
        private decimal locVarVATEXEMPT_AMNT;
        private decimal locVarLOCAL_TAX;
        private decimal locVarPWD_DISC;
        private decimal locVarSNRCIT_DISC;
        private decimal locVarEMPLO_DISC;
        private decimal locVarAYALA_DISC;
        private decimal locVarSTORE_DISC;
        private decimal locVarOTHER_DISC;
        private decimal locVarREFUND_AMT;
        private decimal locVarSCHRGE_AMT;
        private decimal locVarOTHER_SCHR;
        private decimal locVarCASH_SLS;
        private decimal locVarCARD_SLS;
        private decimal locVarEPAY_SLS;
        private decimal locVarDCARD_SLS;
        private decimal locVarOTHERSL_SLS;
        private decimal locVarCHECK_SLS;
        private decimal locVarGC_SLS;
        private decimal locVarMASTERCARD_SLS;
        private decimal locVarVISA_SLS;
        private decimal locVarAMEX_SLS;
        private decimal locVarDINERS_SLS;
        private decimal locVarJCB_SLS;
        private decimal locVarGCASH_SLS;
        private decimal locVarPAYMAYA_SLS;
        private decimal locVarALIPAY_SLS;
        private decimal locVarWECHAT_SLS;
        private decimal locVarGRAB_SLS;
        private decimal locVarFOODPANDA_SLS;
        private decimal locVarMASTERDEBIT_SLS;
        private decimal locVarVISADEBIT_SLS;
        private decimal locVarPAYPAL_SLS;
        private decimal locVarONLINE_SLS;
        private decimal locVarOPEN_SALES;
        private decimal locVarOPEN_SALES_2;
        private decimal locVarOPEN_SALES_3;
        private decimal locVarOPEN_SALES_4;
        private decimal locVarOPEN_SALES_5;
        private decimal locVarOPEN_SALES_6;
        private decimal locVarOPEN_SALES_7;
        private decimal locVarOPEN_SALES_8;
        private decimal locVarOPEN_SALES_9;
        private decimal locVarOPEN_SALES_10;
        private decimal locVarOPEN_SALES_11;
        private decimal locVarGC_EXCESS;
        private string locVarMOBILE_NO;
        private int locVarNO_CUST;
        private string locVarTRN_TYPE;
        private string locVarSLS_FLAG;
        private decimal locVarVAT_PCT;
        private decimal locVarQTY_SLD;
        private decimal locVarQTY;
        private string locVarITEMCODE;
        private decimal locVarPRICE;
        private decimal locVarLDISC;
        private int tranCnt;
        private DateTime dateNow;
        private string filePath;
        private string LastOr;
        private string tranDate;
        private string fileDate;
        private int itemQty;
        private string discountName;
        private decimal discountAmount;

        #endregion

        #region EODVariables

        private string EodCCCODE;
        private string EodMERCHANT_NAME;
        private string EodTER_NO;
        private string EodTRN_DATE;
        private string EodSTRANS;
        private string EodETRANS;
        private decimal EodGROSS_SLS;
        private decimal EodVAT_AMNT;
        private decimal EodVATABLE_SLS;
        private decimal EodNONVAT_SLS;
        private decimal EodVATEXEMPT_SLS;
        private decimal EodVATEXEMPT_AMNT;
        private decimal EodOLD_GRNTOT;
        private decimal EodNEW_GRNTOT;
        private decimal EodLOCAL_TAX;
        private decimal EodVOID_AMNT;
        private int EodNO_VOID;
        private decimal EodDISCOUNTS;
        private int EodNO_DISC;
        private decimal EodREFUND_AMT;
        private int EodNO_REFUND;
        private decimal EodSNRCIT_DISC;
        private int EodNO_SNRCIT;
        private decimal EodPWD_DISC;
        private int EodNO_PWD;
        private decimal EodEMPLO_DISC;
        private int EodNO_EMPLO;
        private decimal EodAYALA_DISC;
        private int EodNO_AYALA;
        private decimal EodSTORE_DISC;
        private int EodNO_STORE;
        private decimal EodOTHER_DISC;
        private int EodNO_OTHER_DISC;
        private decimal EodSCHRGE_AMT;
        private decimal EodOTHER_SCHR;
        private decimal EodCASH_SLS;
        private decimal EodCARD_SLS;
        private decimal EodEPAY_SLS;
        private decimal EodDCARD_SLS;
        private decimal EodOTHER_SLS;
        private decimal EodCHECK_SLS;
        private decimal EodGC_SLS;
        private decimal EodMASTERCARD_SLS;
        private decimal EodVISA_SLS;
        private decimal EodAMEX_SLS;
        private decimal EodDINERS_SLS;
        private decimal EodJCB_SLS;
        private decimal EodGCASH_SLS;
        private decimal EodPAYMAYA_SLS;
        private decimal EodALIPAY_SLS;
        private decimal EodWECHAT_SLS;
        private decimal EodGRAB_SLS;
        private decimal EodFOODPANDA_SLS;
        private decimal EodMASTERDEBIT_SLS;
        private decimal EodVISADEBIT_SLS;
        private decimal EodPAYPAL_SLS;
        private decimal EodONLINE_SLS;
        private decimal EodOPEN_SALES;
        private decimal EodOPEN_SALES_2;
        private decimal EodOPEN_SALES_3;
        private decimal EodOPEN_SALES_4;
        private decimal EodOPEN_SALES_5;
        private decimal EodOPEN_SALES_6;
        private decimal EodOPEN_SALES_7;
        private decimal EodOPEN_SALES_8;
        private decimal EodOPEN_SALES_9;
        private decimal EodOPEN_SALES_10;
        private decimal EodOPEN_SALES_11;
        private decimal EodGC_EXCESS;
        private int EodNO_VATEXEMT;
        private int EodNO_SCHRGE;
        private int EodNO_OTHER_SUR;
        private int EodNO_CASH;
        private int EodNO_CARD;
        private int EodNO_EPAY;
        private int EodNO_DCARD_SLS;
        private int EodNO_OTHER_SLS;
        private int EodNO_CHECK;
        private int EodNO_GC;
        private int EodNO_MASTERCARD_SLS;
        private int EodNO_VISA_SLS;
        private int EodNO_AMEX_SLS;
        private int EodNO_DINERS_SLS;
        private int EodNO_JCB_SLS;
        private int EodNO_GCASH_SLS;
        private int EodNO_PAYMAYA_SLS;
        private int EodNO_ALIPAY_SLS;
        private int EodNO_WECHAT_SLS;
        private int EodNO_GRAB_SLS;
        private int EodNO_FOODPANDA_SLS;
        private int EodNO_MASTERDEBIT_SLS;
        private int EodNO_VISADEBIT_SLS;
        private int EodNO_PAYPAL_SLS;
        private int EodNO_ONLINE_SLS;
        private int EodNO_OPEN_SALES;
        private int EodNO_OPEN_SALES_2;
        private int EodNO_OPEN_SALES_3;
        private int EodNO_OPEN_SALES_4;
        private int EodNO_OPEN_SALES_5;
        private int EodNO_OPEN_SALES_6;
        private int EodNO_OPEN_SALES_7;
        private int EodNO_OPEN_SALES_8;
        private int EodNO_OPEN_SALES_9;
        private int EodNO_OPEN_SALES_10;
        private int EodNO_OPEN_SALES_11;
        private int EodNO_NOSALE;
        private int EodNO_CUST;
        private int EodNO_TRN;
        private int EodPREV_EODCTR;
        private int EodEODCTR;

        public string EODPass { get; private set; }

        #endregion

        public frmMain()
        {
            InitializeComponent();
            InitializeSystem();
            InitializeYearlySubFolder();
        }

        private void InitializeYearlySubFolder()
        {
            try
            {
                salesFolder = salesLocation;
                yearFolder = DateTime.Now.Year.ToString();
                path = Path.Combine(salesFolder, yearFolder);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                else { }
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("{0}/{1}", ex.Message, ex.StackTrace), "Yearly Sub Folder Creation.", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void InitializeSystem()
        {
            try
            {
                lastHour = DateTime.Now.Hour;
                if (!InitializeConfiguration())
                {
                    MessageBox.Show("System setting is not yet configured.", "Invalid Configuration", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                    frmConfig config = new frmConfig();
                    config.ShowDialog();
                    Environment.Exit(1);
                }
                if (!string.IsNullOrEmpty(databaseLocation))
                {
                    // database connection
                    //_dbParadox = new DbParadox(databaseLocation, databasePassword);
                    _dbAccess = new DbAccess(databaseLocation, databasePassword);
                    _discountConfig = _settings.Read<DiscountModelConfig>("DiscountConfig");
                    _paymentConfig = _settings.Read<PaymentModelConfig>("PaymentConfig");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("{0}/{1}", ex.Message, ex.StackTrace), "System Initialization", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private bool InitializeConfiguration()
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
                    EODPass = "vcsi123!@#";
                }
                else
                {
                    _systemEodPass = _systemEodPassConfig.EodPassModels;
                    if (_systemEodPass != null)
                    {
                        foreach (PassCodeModel configuration in _systemEodPass)
                        {
                            EODPass = configuration.eodPassword;
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
                        companyCode = configuration.CompanyCode;
                        merchantName = configuration.MerchantName;
                        terminalNumber = configuration.TerminalNumber;
                        databaseLocation = configuration.DatabaseLocation;
                        databasePassword = configuration.DatabasePassword;
                        discountType = configuration.DiscountType;
                        salesLocation = configuration.SalesLocation;
                        salesPrinter = configuration.SalesPrinter;
                        salesBIR = configuration.SalesBIR;
                        salesPOSSerial = configuration.SalesPOSSerial;

                    }
                }
            }

            return validConfig;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            Program.DatabaseFile = _databaseFileName;
            notifyIcon1.BalloonTipText = "Running...";
            notifyIcon1.ShowBalloonTip(2000);

            transactionDataStorage = new TransactionsDataStorage();
            transactionDetailsStorage = new TransactionsDetailsStorage();
            dailyData = new DailyDataStorage();
            helper = new CSVHelper();

            _isBackup = true;
            ParadoxTable.ORDERS = (_isBackup ? "ordbkup" : "orders");
            ParadoxTable.PAYMENTS = (_isBackup ? "paybkup" : "payment");
            ParadoxTable.ITEMS = (_isBackup ? "itemBkup" : "orditem");

            _timer = new System.Timers.Timer(5000);
            _timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            _timer.AutoReset = true;
            _timer.Start();

            _worker = new BackgroundWorker();
            _worker.WorkerReportsProgress = true;
            _worker.WorkerSupportsCancellation = true;
            _worker.DoWork += WorkerDoWork;
            _worker.ProgressChanged += WorkerProgressChanged;
            _worker.RunWorkerCompleted += WorkerCompleted;

        }

        private void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            notifyIcon1.BalloonTipText = "Sales generated!";
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.ShowBalloonTip(500);
        }

        #region For Hourly/EOD
        private void InitilizeEODValues()
        {
            EodCCCODE = String.Empty;
            EodMERCHANT_NAME = String.Empty;
            EodTER_NO = String.Empty;
            EodTRN_DATE = String.Empty;
            EodSTRANS = String.Empty;
            EodETRANS = String.Empty;
            EodGROSS_SLS = 0.00M;
            EodVAT_AMNT = 0.00M;
            EodVATABLE_SLS = 0.00M;
            EodNONVAT_SLS = 0.00M;
            EodVATEXEMPT_SLS = 0.00M;
            EodVATEXEMPT_AMNT = 0.00M;
            EodOLD_GRNTOT = 0.00M;
            EodNEW_GRNTOT = 0.00M;
            EodLOCAL_TAX = 0.00M;
            EodVOID_AMNT = 0.00M;
            EodNO_VOID = 0;
            EodDISCOUNTS = 0.00M;
            EodNO_DISC = 0;
            EodREFUND_AMT = 0.00M;
            EodNO_REFUND = 0;
            EodSNRCIT_DISC = 0.00M;
            EodNO_SNRCIT = 0;
            EodPWD_DISC = 0.00M;
            EodNO_PWD = 0;
            EodEMPLO_DISC = 0.00M;
            EodNO_EMPLO = 0;
            EodAYALA_DISC = 0.00M;
            EodNO_AYALA = 0;
            EodSTORE_DISC = 0.00M;
            EodNO_STORE = 0;
            EodOTHER_DISC = 0.00M;
            EodNO_OTHER_DISC = 0;
            EodSCHRGE_AMT = 0.00M;
            EodOTHER_SCHR = 0.00M;
            EodCASH_SLS = 0.00M;
            EodCARD_SLS = 0.00M;
            EodEPAY_SLS = 0.00M;
            EodDCARD_SLS = 0.00M;
            EodOTHER_SLS = 0.00M;
            EodCHECK_SLS = 0.00M;
            EodGC_SLS = 0.00M;
            EodMASTERCARD_SLS = 0.00M;
            EodVISA_SLS = 0.00M;
            EodAMEX_SLS = 0.00M;
            EodDINERS_SLS = 0.00M;
            EodJCB_SLS = 0.00M;
            EodGCASH_SLS = 0.00M;
            EodPAYMAYA_SLS = 0.00M;
            EodALIPAY_SLS = 0.00M;
            EodWECHAT_SLS = 0.00M;
            EodGRAB_SLS = 0.00M;
            EodFOODPANDA_SLS = 0.00M;
            EodMASTERDEBIT_SLS = 0.00M;
            EodVISADEBIT_SLS = 0.00M;
            EodPAYPAL_SLS = 0.00M;
            EodONLINE_SLS = 0.00M;
            EodOPEN_SALES = 0.00M;
            EodOPEN_SALES_2 = 0.00M;
            EodOPEN_SALES_3 = 0.00M;
            EodOPEN_SALES_4 = 0.00M;
            EodOPEN_SALES_5 = 0.00M;
            EodOPEN_SALES_6 = 0.00M;
            EodOPEN_SALES_7 = 0.00M;
            EodOPEN_SALES_8 = 0.00M;
            EodOPEN_SALES_9 = 0.00M;
            EodOPEN_SALES_10 = 0.00M;
            EodOPEN_SALES_11 = 0.00M;
            EodGC_EXCESS = 0.00M;
            EodNO_VATEXEMT = 0;
            EodNO_SCHRGE = 0;
            EodNO_OTHER_SUR = 0;
            EodNO_CASH = 0;
            EodNO_CARD = 0;
            EodNO_EPAY = 0;
            EodNO_DCARD_SLS = 0;
            EodNO_OTHER_SLS = 0;
            EodNO_CHECK = 0;
            EodNO_GC = 0;
            EodNO_MASTERCARD_SLS = 0;
            EodNO_VISA_SLS = 0;
            EodNO_AMEX_SLS = 0;
            EodNO_DINERS_SLS = 0;
            EodNO_JCB_SLS = 0;
            EodNO_GCASH_SLS = 0;
            EodNO_PAYMAYA_SLS = 0;
            EodNO_ALIPAY_SLS = 0;
            EodNO_WECHAT_SLS = 0;
            EodNO_GRAB_SLS = 0;
            EodNO_FOODPANDA_SLS = 0;
            EodNO_MASTERDEBIT_SLS = 0;
            EodNO_VISADEBIT_SLS = 0;
            EodNO_PAYPAL_SLS = 0;
            EodNO_ONLINE_SLS = 0;
            EodNO_OPEN_SALES = 0;
            EodNO_OPEN_SALES_2 = 0;
            EodNO_OPEN_SALES_3 = 0;
            EodNO_OPEN_SALES_4 = 0;
            EodNO_OPEN_SALES_5 = 0;
            EodNO_OPEN_SALES_6 = 0;
            EodNO_OPEN_SALES_7 = 0;
            EodNO_OPEN_SALES_8 = 0;
            EodNO_OPEN_SALES_9 = 0;
            EodNO_OPEN_SALES_10 = 0;
            EodNO_OPEN_SALES_11 = 0;
            EodNO_NOSALE = 0;
            EodNO_CUST = 0;
            EodNO_TRN = 0;
            EodPREV_EODCTR = 0;
            EodEODCTR = 0;
        }
        private void InitializeValues()
        {

            #region HeaderSales
            locVarCCCODE = companyCode;
            locVarMERCHANT_NAME = merchantName;
            locVarTRN_DATE = DateTime.Now.ToString("yyyy-MM-dd");
            locVarNO_TRN = 0;
            locVarCDATE = DateTime.Now.ToString("yyyy-MM-dd");
            locVarTRN_TIME = DateTime.Now.ToString("HH:MM");
            locVarTER_NO = terminalNumber;
            locVarTRANSACTION_NO = String.Empty;
            locVarGROSS_SLS = 0.00M;
            locVarVAT_AMNT = 0.00M;
            locVarVATABLE_SLS = 0.00M;
            locVarNONVAT_SLS = 0.00M;
            locVarVATEXEMPT_SLS = 0.00M;
            locVarVATEXEMPT_AMNT = 0.00M;
            locVarLOCAL_TAX = 0.00M;
            locVarPWD_DISC = 0.00M;
            locVarSNRCIT_DISC = 0.00M;
            locVarEMPLO_DISC = 0.00M;
            locVarAYALA_DISC = 0.00M;
            locVarSTORE_DISC = 0.00M;
            locVarOTHER_DISC = 0.00M;
            locVarREFUND_AMT = 0.00M;
            locVarSCHRGE_AMT = 0.00M;
            locVarOTHER_SCHR = 0.00M;
            locVarCASH_SLS = 0.00M;
            locVarCARD_SLS = 0.00M;
            locVarEPAY_SLS = 0.00M;
            locVarDCARD_SLS = 0.00M;
            locVarOTHERSL_SLS = 0.00M;
            locVarCHECK_SLS = 0.00M;
            locVarGC_SLS = 0.00M;
            locVarMASTERCARD_SLS = 0.00M;
            locVarVISA_SLS = 0.00M;
            locVarAMEX_SLS = 0.00M;
            locVarDINERS_SLS = 0.00M;
            locVarJCB_SLS = 0.00M;
            locVarGCASH_SLS = 0.00M;
            locVarPAYMAYA_SLS = 0.00M;
            locVarALIPAY_SLS = 0.00M;
            locVarWECHAT_SLS = 0.00M;
            locVarGRAB_SLS = 0.00M;
            locVarFOODPANDA_SLS = 0.00M;
            locVarMASTERDEBIT_SLS = 0.00M;
            locVarVISADEBIT_SLS = 0.00M;
            locVarPAYPAL_SLS = 0.00M;
            locVarONLINE_SLS = 0.00M;
            locVarOPEN_SALES = 0.00M;
            locVarOPEN_SALES_2 = 0.00M;
            locVarOPEN_SALES_3 = 0.00M;
            locVarOPEN_SALES_4 = 0.00M;
            locVarOPEN_SALES_5 = 0.00M;
            locVarOPEN_SALES_6 = 0.00M;
            locVarOPEN_SALES_7 = 0.00M;
            locVarOPEN_SALES_8 = 0.00M;
            locVarOPEN_SALES_9 = 0.00M;
            locVarOPEN_SALES_10 = 0.00M;
            locVarOPEN_SALES_11 = 0.00M;
            locVarGC_EXCESS = 0.00M;
            locVarMOBILE_NO = String.Empty;
            locVarNO_CUST = 0;
            locVarTRN_TYPE = String.Empty;
            locVarSLS_FLAG = String.Empty;
            locVarVAT_PCT = 0.00M;
            locVarQTY_SLD = 0.000M;
            #endregion

            #region Details
            locVarQTY = 0.000M;
            locVarITEMCODE = String.Empty;
            locVarPRICE = 0.00M;
            locVarLDISC = 0.00M;
            #endregion

            LastOr = String.Empty;

        }
        #endregion

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            if (lastHour < DateTime.Now.Hour)
            {
                if (!_worker.IsBusy)
                {
                    _worker.RunWorkerAsync();
                }
            }
        }

        private void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            try
            {
                notifyIcon1.BalloonTipTitle = "Generating sales";
                notifyIcon1.BalloonTipText = String.Format("{0} from {1} to {2}", "Generation of sales", DateTime.Now.AddHours(-1).ToString("h: mm tt"), DateTime.Now.ToString("h: mm tt"));
                notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                notifyIcon1.Text = String.Format("{0} from {1} to {2}", "Generation of sales", DateTime.Now.AddHours(-1).ToString("h: mm tt"), DateTime.Now.ToString("h: mm tt"));
                notifyIcon1.ShowBalloonTip(1000);

                lastHour = DateTime.Now.Hour;
                lastDate = DateTime.Now;
                GenerateSales(worker, e);

            }
            catch (Exception)
            {
                worker.CancelAsync();
            }
        }

        private void GenerateSales(BackgroundWorker worker, DoWorkEventArgs e)
        {
            try
            {
                tranCnt = 0;
                //dateNow = new DateTime(2023, 02, 14); pang test
                Console.WriteLine("Running background task...");
                InitializeValues();
                perTransactionsList = new List<List<PerTransactionsFile>>();
                //dateNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                dateNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                //getting order/transaction lists
                //string getOrders = string.Format(Queries.OrderList, ParadoxTable.ORDERS, dateNow);
                string getOrders = string.Format(Queries.MYOB_SALES, 0, dateNow.ToString("yyyy/MM/dd"));
                DataTable dtOrder = _dbAccess.GetDataTable(getOrders);
                if (dtOrder.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtOrder.Rows)
                    {
                        if (!transactionDataStorage.IsExistValue("TRANSACTION_NO", dr["docket_id"].ToString())) //check if already generated
                        {
                            InitializeValues();
                            tranCnt = tranCnt + 1;
                            itemQty = 0;
                            ListOrder = new List<PerTransactionsFile>();
                            ListOrderItems = new List<PerTransactionsItem>();
                            LastOr = dr["docket_id"].ToString(); //get every OR until get the last record
                            tranDate = dr["docket_date"].ToString(); // get each transaction date for file name output
                            //DateTime date = DateTime.ParseExact(tranDate, "MM/dd/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                            DateTime date = DateTime.ParseExact(tranDate, "yyyy/MM/dd h:mm:ss tt", CultureInfo.InvariantCulture);
                            tranDate = date.ToString("yyyy-MM-dd");
                            fileDate = date.ToString("MMddyy");

                            //computation and retrieval for each lines
                            #region VAT_AMNT
                            //if (!dr["Disctype"].ToString().ToUpper().Contains("PWD") || !dr["Disctype"].ToString().ToUpper().Contains("SENIOR"))
                            //{
                            //    locVarVAT_AMNT = dr["GST"].ToSafeDecimal();
                            //}
                            locVarVAT_AMNT = dr["VATamount"].ToString().Replace("Php","").ToSafeDecimal();
                            #endregion

                            #region LOCAL_TAX
                            //locVarLOCAL_TAX = dr["PST"].ToSafeDecimal();
                            locVarLOCAL_TAX = 0.00M;
                            #endregion

                            #region PWD_DISC SNRCIT_DISC
                            //if (dr["Disctype"].ToString().ToUpper().Contains("PWD"))
                            //{
                            //    locVarPWD_DISC = dr["Disc_amt"].ToSafeDecimal();
                            //}
                            //if (dr["Disctype"].ToString().ToUpper().Contains("SENIOR"))
                            //{
                            //    locVarSNRCIT_DISC = dr["Disc_amt"].ToSafeDecimal();
                            //}

                            locVarPWD_DISC = 0.00M;
                            locVarSNRCIT_DISC = 0.00M;
                            #endregion

                            #region EMPLO_DISC,AYALA_DISC,STORE_DISC,OTHER_DISC
                            //GetDiscAmount(dr["OrderNo"].ToString(), dateNow);

                            //GetDiscAmount(dtOrder, dr["OrderNo"].ToString());
                            locVarEMPLO_DISC = 0.00M;
                            locVarAYALA_DISC = 0.00M;
                            locVarSTORE_DISC = 0.00M;
                            locVarOTHER_DISC = dr["Dscnt"].ToString().Replace("Php", "").ToSafeDecimal();
                            #endregion

                            #region REFUND_AMT
                            locVarREFUND_AMT = 0.00M; //no refund on wbox
                            #endregion

                            #region SCHRGE_AMT
                            //locVarSCHRGE_AMT = dr["Serv"].ToSafeDecimal();
                            locVarSCHRGE_AMT = 0.00M;//no service charge
                            #endregion

                            #region OTHER_SCHR
                            locVarOTHER_SCHR = 0.00M; //no other charge on wbox
                            #endregion

                            #region Payments
                            GetPayAmount(dr["Paymode"].ToString(), dr["docket_id"].ToString());
                            #endregion

                            #region NO_CUST , MOBILENO
                            locVarNO_CUST = dr["customer"].ToSafeInteger();
                            locVarMOBILE_NO = String.Empty;
                            #endregion

                            #region TRN_TYPE
                            //locVarTRN_TYPE = GetTranType(dr["TableNo"].ToString());
                            locVarTRN_TYPE = "T";
                            #endregion

                            #region SLS_FLAG, VAT_PCT
                            locVarSLS_FLAG = "S";
                            locVarVAT_PCT = 12.00M;
                            #endregion

                            

                            //getting transaction items lists GET_ITEMS
                            //string getOrderItems = string.Format(Queries.GET_ORDERITEMS, ParadoxTable.ITEMS, dr["OrderNo"].ToString());
                            string getOrderItems = string.Format(Queries.MYOB_ITEMS, "DocketLine", dr["docket_id"].ToString(), "Stock");
                            DataTable dtOrerItems = _dbAccess.GetDataTable(getOrderItems);
                            if (dtOrerItems.Rows.Count > 0)
                            {
                                foreach (DataRow drItem in dtOrerItems.Rows)
                                {

                                    #region VATABLE_SLS NONVAT_SLS VATEXEMPT_SLS
                                    locVarVATABLE_SLS = drItem["sell_inc"].ToSafeDecimal();
                                    locVarNONVAT_SLS = drItem["rrp"].ToSafeDecimal();
                                    locVarVATEXEMPT_SLS = drItem["sell_ex"].ToSafeDecimal();
                                    #endregion

                                    #region VATABLE_SLS
                                    //if (discountType.Contains("ByItem"))
                                    //{
                                    //    
                                    //    if (!drItem["Integer3"].ToString().Contains("300"))
                                    //    {
                                    //        locVarVATABLE_SLS = locVarVATABLE_SLS + (drItem["cost"].ToSafeDecimal() / 1.12M);
                                    //    }
                                    //}
                                    #endregion

                                    #region NONVAT_SLS
                                    //if (discountType.Contains("ByItem"))
                                    //{
                                    //    if (drItem["Logic5"].ToString().Contains("True"))
                                    //    {
                                    //        locVarNONVAT_SLS = locVarNONVAT_SLS + (drItem["Price"].ToSafeDecimal() / 1.12M);
                                    //    }
                                    //}
                                    #endregion

                                    #region VATEXEMPT_SLS
                                    //if (discountType.Contains("ByItem"))
                                    //{
                                    //    if (drItem["Integer3"].ToString().Contains("300"))
                                    //    {
                                    //        locVarVATEXEMPT_SLS = locVarVATEXEMPT_SLS + (drItem["Price"].ToSafeDecimal() / 1.12M);
                                    //    }
                                    //}
                                    #endregion

                                    //saving to Items temporary database
                                    #region save to Items TEMPDB
                                    var saleItems = new TransactionDataDetails
                                    {
                                        ITEMCODE = drItem["StockCode"].ToString(),
                                        PRICE = Convert.ToDecimal(drItem["sell_inc"]).ToDecimalPlaces(2),
                                        //LDISC = Convert.ToDecimal(drItem[""]).ToDecimalPlaces(2),
                                        LDISC = "0",
                                        QTY = Convert.ToDecimal(drItem["qty"]).ToDecimalPlaces(3),
                                        TRAN_OR = dr["docket_id"].ToString()
                                    };
                                    transactionDetailsStorage.Add(saleItems, false);
                                    #endregion

                                    //saving items array in List
                                    arrayOrderItems = new PerTransactionsItem
                                    {
                                        ITEMCODE = drItem["StockCode"].ToString(),
                                        PRICE = Convert.ToDecimal(drItem["sell_inc"]).ToDecimalPlaces(2),
                                        //LDISC = Convert.ToDecimal(drItem["DiscValue"]).ToDecimalPlaces(2),
                                        LDISC = "0",
                                        QTY = Convert.ToDecimal(drItem["qty"]).ToDecimalPlaces(3)
                                    };
                                    ListOrderItems.Add(arrayOrderItems);
                                    itemQty = itemQty + 1;
                                }

                                #region VATEXEMPT_AMNT
                                locVarVATEXEMPT_AMNT = locVarNONVAT_SLS + locVarVATEXEMPT_SLS;
                                #endregion

                                #region GROSS_SLS
                                //LINE10 + LINE 11 + LINE 12 + LINE 13 + LINE 14 + LINE 15 + LINE 16 + LINE 17 +
                                //LINE 18 + LINE 19 + LINE 20 + LINE 21 + LINE 23 + LINE 24
                                locVarGROSS_SLS = locVarVAT_AMNT + locVarVATABLE_SLS + locVarVATEXEMPT_SLS + locVarVATEXEMPT_AMNT + locVarLOCAL_TAX + locVarPWD_DISC + locVarSNRCIT_DISC + locVarEMPLO_DISC + locVarAYALA_DISC + locVarSTORE_DISC + locVarOTHER_DISC + locVarREFUND_AMT + locVarSCHRGE_AMT + locVarOTHER_SCHR;
                                #endregion

                                //saving to Header temporary database
                                #region save to Header TEMPDB
                                var salesOrders = new TransactionsData
                                {
                                    CCCODE = locVarCCCODE,
                                    MERCHANT_NAME = locVarMERCHANT_NAME,
                                    TRN_DATE = tranDate,
                                    NO_TRN = 1,
                                    CDATE = DateTime.Now.ToString("yyyy-MM-dd"),
                                    TRN_TIME = Convert.ToDateTime(dr["docket_date"]).ToString("HH:mm"),
                                    TER_NO = locVarTER_NO,
                                    TRANSACTION_NO = dr["docket_id"].ToString(),
                                    GROSS_SLS = locVarGROSS_SLS.ToDecimalPlaces(2),
                                    VAT_AMNT = locVarVAT_AMNT.ToDecimalPlaces(2),
                                    VATABLE_SLS = locVarVATABLE_SLS.ToDecimalPlaces(2),
                                    NONVAT_SLS = locVarNONVAT_SLS.ToDecimalPlaces(2),
                                    VATEXEMPT_SLS = locVarVATEXEMPT_SLS.ToDecimalPlaces(2),
                                    VATEXEMPT_AMNT = locVarVAT_AMNT.ToDecimalPlaces(2),
                                    LOCAL_TAX = locVarLOCAL_TAX.ToDecimalPlaces(2),
                                    PWD_DISC = locVarPWD_DISC.ToDecimalPlaces(2),
                                    SNRCIT_DISC = locVarSNRCIT_DISC.ToDecimalPlaces(2),
                                    EMPLO_DISC = locVarEMPLO_DISC.ToDecimalPlaces(2),
                                    AYALA_DISC = locVarAYALA_DISC.ToDecimalPlaces(2),
                                    STORE_DISC = locVarSTORE_DISC.ToDecimalPlaces(2),
                                    OTHER_DISC = locVarOTHER_DISC.ToDecimalPlaces(2),
                                    REFUND_AMT = locVarREFUND_AMT.ToDecimalPlaces(2),
                                    SCHRGE_AMT = locVarSCHRGE_AMT.ToDecimalPlaces(2),
                                    OTHER_SCHR = locVarOTHER_SCHR.ToDecimalPlaces(2),
                                    CASH_SLS = locVarCASH_SLS.ToDecimalPlaces(2),
                                    CARD_SLS = locVarCARD_SLS.ToDecimalPlaces(2),
                                    EPAY_SLS = locVarEPAY_SLS.ToDecimalPlaces(2),
                                    DCARD_SLS = locVarDCARD_SLS.ToDecimalPlaces(2),
                                    OTHERSL_SLS = locVarOTHERSL_SLS.ToDecimalPlaces(2),
                                    CHECK_SLS = locVarCHECK_SLS.ToDecimalPlaces(2),
                                    GC_SLS = locVarGC_SLS.ToDecimalPlaces(2),
                                    MASTERCARD_SLS = locVarMASTERCARD_SLS.ToDecimalPlaces(2),
                                    VISA_SLS = locVarVISA_SLS.ToDecimalPlaces(2),
                                    AMEX_SLS = locVarAMEX_SLS.ToDecimalPlaces(2),
                                    DINERS_SLS = locVarDINERS_SLS.ToDecimalPlaces(2),
                                    JCB_SLS = locVarJCB_SLS.ToDecimalPlaces(2),
                                    GCASH_SLS = locVarGCASH_SLS.ToDecimalPlaces(2),
                                    PAYMAYA_SLS = locVarPAYMAYA_SLS.ToDecimalPlaces(2),
                                    ALIPAY_SLS = locVarALIPAY_SLS.ToDecimalPlaces(2),
                                    WECHAT_SLS = locVarWECHAT_SLS.ToDecimalPlaces(2),
                                    GRAB_SLS = locVarGRAB_SLS.ToDecimalPlaces(2),
                                    FOODPANDA_SLS = locVarFOODPANDA_SLS.ToDecimalPlaces(2),
                                    MASTERDEBIT_SLS = locVarMASTERDEBIT_SLS.ToDecimalPlaces(2),
                                    VISADEBIT_SLS = locVarVISADEBIT_SLS.ToDecimalPlaces(2),
                                    PAYPAL_SLS = locVarPAYPAL_SLS.ToDecimalPlaces(2),
                                    ONLINE_SLS = locVarONLINE_SLS.ToDecimalPlaces(2),
                                    OPEN_SALES = "0.00",
                                    OPEN_SALES_2 = "0.00",
                                    OPEN_SALES_3 = "0.00",
                                    OPEN_SALES_4 = "0.00",
                                    OPEN_SALES_5 = "0.00",
                                    OPEN_SALES_6 = "0.00",
                                    OPEN_SALES_7 = "0.00",
                                    OPEN_SALES_8 = "0.00",
                                    OPEN_SALES_9 = "0.00",
                                    OPEN_SALES_10 = "0.00",
                                    OPEN_SALES_11 = "0.00",
                                    GC_EXCESS = "0.00",
                                    MOBILE_NO = string.Empty,
                                    NO_CUST = locVarNO_CUST,
                                    TRN_TYPE = locVarTRN_TYPE,
                                    SLS_FLAG = locVarSLS_FLAG,
                                    VAT_PCT = locVarVAT_PCT.ToDecimalPlaces(2),
                                    QTY_SLD = Convert.ToDecimal(itemQty).ToDecimalPlaces(3)
                                };
                                transactionDataStorage.Add(salesOrders, false);
                                #endregion

                                //saving data as List for creation of output file
                                #region for csv file data saving
                                arrayOrder = new PerTransactionsFile
                                {
                                    CCCODE = locVarCCCODE,
                                    MERCHANT_NAME = locVarMERCHANT_NAME,
                                    TRN_DATE = tranDate,
                                    NO_TRN = 1,
                                    CDATE = DateTime.Now.ToString("yyyy-MM-dd"),
                                    TRN_TIME = Convert.ToDateTime(dr["docket_date"]).ToString("HH:mm"),
                                    TER_NO = locVarTER_NO,
                                    TRANSACTION_NO = dr["docket_id"].ToString(),
                                    GROSS_SLS = locVarGROSS_SLS.ToDecimalPlaces(2),
                                    VAT_AMNT = locVarVAT_AMNT.ToDecimalPlaces(2),
                                    VATABLE_SLS = locVarVATABLE_SLS.ToDecimalPlaces(2),
                                    NONVAT_SLS = locVarNONVAT_SLS.ToDecimalPlaces(2),
                                    VATEXEMPT_SLS = locVarVATEXEMPT_SLS.ToDecimalPlaces(2),
                                    VATEXEMPT_AMNT = locVarVAT_AMNT.ToDecimalPlaces(2),
                                    LOCAL_TAX = locVarLOCAL_TAX.ToDecimalPlaces(2),
                                    PWD_DISC = locVarPWD_DISC.ToDecimalPlaces(2),
                                    SNRCIT_DISC = locVarSNRCIT_DISC.ToDecimalPlaces(2),
                                    EMPLO_DISC = locVarEMPLO_DISC.ToDecimalPlaces(2),
                                    AYALA_DISC = locVarAYALA_DISC.ToDecimalPlaces(2),
                                    STORE_DISC = locVarSTORE_DISC.ToDecimalPlaces(2),
                                    OTHER_DISC = locVarOTHER_DISC.ToDecimalPlaces(2),
                                    REFUND_AMT = locVarREFUND_AMT.ToDecimalPlaces(2),
                                    SCHRGE_AMT = locVarSCHRGE_AMT.ToDecimalPlaces(2),
                                    OTHER_SCHR = locVarOTHER_SCHR.ToDecimalPlaces(2),
                                    CASH_SLS = locVarCASH_SLS.ToDecimalPlaces(2),
                                    CARD_SLS = locVarCARD_SLS.ToDecimalPlaces(2),
                                    EPAY_SLS = locVarEPAY_SLS.ToDecimalPlaces(2),
                                    DCARD_SLS = locVarDCARD_SLS.ToDecimalPlaces(2),
                                    OTHERSL_SLS = locVarOTHERSL_SLS.ToDecimalPlaces(2),
                                    CHECK_SLS = locVarCHECK_SLS.ToDecimalPlaces(2),
                                    GC_SLS = locVarGC_SLS.ToDecimalPlaces(2),
                                    MASTERCARD_SLS = locVarMASTERCARD_SLS.ToDecimalPlaces(2),
                                    VISA_SLS = locVarVISA_SLS.ToDecimalPlaces(2),
                                    AMEX_SLS = locVarAMEX_SLS.ToDecimalPlaces(2),
                                    DINERS_SLS = locVarDINERS_SLS.ToDecimalPlaces(2),
                                    JCB_SLS = locVarJCB_SLS.ToDecimalPlaces(2),
                                    GCASH_SLS = locVarGCASH_SLS.ToDecimalPlaces(2),
                                    PAYMAYA_SLS = locVarPAYMAYA_SLS.ToDecimalPlaces(2),
                                    ALIPAY_SLS = locVarALIPAY_SLS.ToDecimalPlaces(2),
                                    WECHAT_SLS = locVarWECHAT_SLS.ToDecimalPlaces(2),
                                    GRAB_SLS = locVarGRAB_SLS.ToDecimalPlaces(2),
                                    FOODPANDA_SLS = locVarFOODPANDA_SLS.ToDecimalPlaces(2),
                                    MASTERDEBIT_SLS = locVarMASTERDEBIT_SLS.ToDecimalPlaces(2),
                                    VISADEBIT_SLS = locVarVISADEBIT_SLS.ToDecimalPlaces(2),
                                    PAYPAL_SLS = locVarPAYPAL_SLS.ToDecimalPlaces(2),
                                    ONLINE_SLS = locVarONLINE_SLS.ToDecimalPlaces(2),
                                    OPEN_SALES = "0.00",
                                    OPEN_SALES_2 = "0.00",
                                    OPEN_SALES_3 = "0.00",
                                    OPEN_SALES_4 = "0.00",
                                    OPEN_SALES_5 = "0.00",
                                    OPEN_SALES_6 = "0.00",
                                    OPEN_SALES_7 = "0.00",
                                    OPEN_SALES_8 = "0.00",
                                    OPEN_SALES_9 = "0.00",
                                    OPEN_SALES_10 = "0.00",
                                    OPEN_SALES_11 = "0.00",
                                    GC_EXCESS = "0.00",
                                    MOBILE_NO = string.Empty,
                                    NO_CUST = locVarNO_CUST,
                                    TRN_TYPE = locVarTRN_TYPE,
                                    SLS_FLAG = "S",
                                    VAT_PCT = locVarVAT_PCT.ToDecimalPlaces(2),
                                    QTY_SLD = Convert.ToDecimal(itemQty).ToDecimalPlaces(3),
                                    Items = ListOrderItems
                                };
                                ListOrder.Add(arrayOrder);
                                perTransactionsList.Add(ListOrder);
                                #endregion
                            }
                        }
                    }
                    //creation of csv file per transactions
                    #region CSV file creation
                    if (tranCnt > 0)
                    {
                        string fileName = String.Format("{0}{1}{2}_{3}.csv", locVarCCCODE, fileDate.Replace("/", ""), locVarTER_NO, LastOr);
                        filePath = Path.Combine(path, fileName);
                        helper.WriteCsv(filePath, perTransactionsList, tranCnt);
                    }
                    #endregion

                    //MessageBox.Show("Done");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: dito sa GENERATESALES Function \n" + ex.Message);
                //worker.CancelAsync();
            }
        }

        private void GetDiscAmount(DataTable dt, string orNum)
        {
            string discName = GetSalesDiscount(orNum, dt);
            switch (discName.ToUpper().ToString())
            {
                case "EMPLOYEE":
                    locVarEMPLO_DISC = discountAmount;
                    break;
                case "AYALA":
                    locVarAYALA_DISC = discountAmount;
                    break;
                case "STORE":
                    locVarSTORE_DISC = discountAmount;
                    break;
                case "OTHER":
                    locVarOTHER_DISC = discountAmount;
                    break;
            }
        }

        private string GetSalesDiscount(string OrderNumber, DataTable dtDiscount)
        {
            discountName = String.Empty;
            discountAmount = 0.00M;
            _discount = _discountConfig.discountModels;
            if (dtDiscount != null)
            {
                var result = from DataRow row in dtDiscount.Rows
                             where (string)row["OrderNo"] == OrderNumber
                             select row;

                foreach (var dr in result)
                {
                    discountName = dr["DiscType"].ToString();
                    discountAmount = Convert.ToDecimal(dr["Number3"]);

                    DiscountModel disc = _discount.Find(x => x.WboxDiscount == discountName);
                    if (disc != null)
                    {
                        discountName = disc.MallDiscount.ToString();
                    }
                }
            }
            return discountName;
        }

        private void GetPayAmount(string paymenttype, string OrNum)
        {
            string defaulttPayName = "CASH";
            _payment = _paymentConfig.paymentModels;

            //string query = string.Format(Queries.GET_PAYMENTAPPLIED, ParadoxTable.PAYMENTS, OrNum);
            string query = string.Format(Queries.MYOB_PAYMODE, "Payments", OrNum);
            DataTable dtPayment = _dbAccess.GetDataTable(query);
            if (dtPayment.Rows.Count > 0)
            {
                if (paymenttype == "MULTI")
                {
                    Dictionary<string, decimal> payments = new Dictionary<string, decimal>();
                    string payName = "";
                    decimal payValue = 0;
                    int count = 1;
                    foreach (DataColumn string1column in dtPayment.Columns)
                    {
                        if (dtPayment.Rows[0][string1column].ToString() != "")
                        {
                            //payName = GetPayment(OrNum, true, count);
                            payName = "CASH";
                            payValue = Convert.ToDecimal(dtPayment.Rows[0][string1column]);


                            PaymentModel pay = _payment.Find(x => x.WboxPayment == payName);
                            if (pay != null)
                            {
                                paymenttype = pay.MallPayment.ToString();
                            }

                            switch (paymenttype.ToUpper().ToString())
                            {
                                case "CASH":
                                    locVarCASH_SLS = locVarCASH_SLS + payValue;
                                    break;
                                case "CHECK":
                                    locVarCHECK_SLS = locVarCHECK_SLS + payValue;
                                    break;
                                case "GC":
                                    locVarGC_SLS = locVarGC_SLS + payValue;
                                    break;
                                case "MASTERCARD":
                                    locVarMASTERCARD_SLS = locVarMASTERCARD_SLS + payValue;
                                    break;
                                case "VISA":
                                    locVarVISA_SLS = locVarVISA_SLS + payValue;
                                    break;
                                case "AMEX":
                                    locVarAMEX_SLS = locVarAMEX_SLS + payValue;
                                    break;
                                case "DINERS":
                                    locVarDINERS_SLS = locVarDINERS_SLS + payValue;
                                    break;
                                case "JCB":
                                    locVarJCB_SLS = locVarJCB_SLS + payValue;
                                    break;
                                case "GCASH":
                                    locVarGCASH_SLS = locVarGCASH_SLS + payValue;
                                    break;
                                case "PAYMAYA":
                                    locVarPAYMAYA_SLS = locVarPAYMAYA_SLS + payValue;
                                    break;
                                case "ALIPAY":
                                    locVarALIPAY_SLS = locVarALIPAY_SLS + payValue;
                                    break;
                                case "WECHAT":
                                    locVarWECHAT_SLS = locVarWECHAT_SLS + payValue;
                                    break;
                                case "GRAB":
                                    locVarGRAB_SLS = locVarGRAB_SLS + payValue;
                                    break;
                                case "FOODPANDA":
                                    locVarFOODPANDA_SLS = locVarFOODPANDA_SLS + payValue;
                                    break;
                                case "MASTERDEBIT":
                                    locVarMASTERDEBIT_SLS = locVarMASTERDEBIT_SLS + payValue;
                                    break;
                                case "VISADEBIT":
                                    locVarVISADEBIT_SLS = locVarVISADEBIT_SLS + payValue;
                                    break;
                                case "PAYPAL":
                                    locVarPAYPAL_SLS = locVarPAYPAL_SLS + payValue;
                                    break;
                                case "OPEN SALES":
                                    locVarOPEN_SALES = locVarOPEN_SALES + payValue;
                                    break;
                                case "ONLINE":
                                    locVarONLINE_SLS = locVarONLINE_SLS + payValue;
                                    break;
                                case "GC_EXCESS":
                                    locVarGC_EXCESS = locVarGC_EXCESS + payValue;
                                    break;
                            }
                            payName = payName == "" ? defaulttPayName : payName;
                            payments.Add(payName, payValue);
                        }
                        count = count + 1;
                    }
                }
                else
                {
                    foreach (DataColumn string1column in dtPayment.Columns)
                    {
                        if (dtPayment.Rows[0][string1column].ToString() != "")
                        {
                            //string payment1 = GetPayment(OrNum, false, 0);
                            string payment1 = "";
                            decimal payment1Value = Convert.ToDecimal(dtPayment.Rows[0][string1column].ToString().Replace("Php", ""));
                            payment1 = payment1 == "" ? paymenttype : payment1;
                            switch (payment1.ToUpper().ToString())
                            {
                                case "CASH":
                                    locVarCASH_SLS = locVarCASH_SLS + payment1Value;
                                    break;
                                case "CHECK":
                                    locVarCHECK_SLS = locVarCHECK_SLS + payment1Value;
                                    break;
                                case "GC":
                                    locVarGC_SLS = locVarGC_SLS + payment1Value;
                                    break;
                                case "MASTERCARD":
                                    locVarMASTERCARD_SLS = locVarMASTERCARD_SLS + payment1Value;
                                    break;
                                case "VISA":
                                    locVarVISA_SLS = locVarVISA_SLS + payment1Value;
                                    break;
                                case "AMEX":
                                    locVarAMEX_SLS = locVarAMEX_SLS + payment1Value;
                                    break;
                                case "DINERS":
                                    locVarDINERS_SLS = locVarDINERS_SLS + payment1Value;
                                    break;
                                case "JCB":
                                    locVarJCB_SLS = locVarJCB_SLS + payment1Value;
                                    break;
                                case "GCASH":
                                    locVarGCASH_SLS = locVarGCASH_SLS + payment1Value;
                                    break;
                                case "PAYMAYA":
                                    locVarPAYMAYA_SLS = locVarPAYMAYA_SLS + payment1Value;
                                    break;
                                case "ALIPAY":
                                    locVarALIPAY_SLS = locVarALIPAY_SLS + payment1Value;
                                    break;
                                case "WECHAT":
                                    locVarWECHAT_SLS = locVarWECHAT_SLS + payment1Value;
                                    break;
                                case "GRAB":
                                    locVarGRAB_SLS = locVarGRAB_SLS + payment1Value;
                                    break;
                                case "FOODPANDA":
                                    locVarFOODPANDA_SLS = locVarFOODPANDA_SLS + payment1Value;
                                    break;
                                case "MASTERDEBIT":
                                    locVarMASTERDEBIT_SLS = locVarMASTERDEBIT_SLS + payment1Value;
                                    break;
                                case "VISADEBIT":
                                    locVarVISADEBIT_SLS = locVarVISADEBIT_SLS + payment1Value;
                                    break;
                                case "PAYPAL":
                                    locVarPAYPAL_SLS = locVarPAYPAL_SLS + payment1Value;
                                    break;
                                case "OPEN SALES":
                                    locVarOPEN_SALES = locVarOPEN_SALES + payment1Value;
                                    break;
                                case "ONLINE":
                                    locVarONLINE_SLS = locVarONLINE_SLS + payment1Value;
                                    break;
                                case "GC_EXCESS":
                                    locVarGC_EXCESS = locVarGC_EXCESS + payment1Value;
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private string GetPayment(string orNum, bool multiPayment, int count)
        {
            string paymenttype = "CASH";
            string otherpaystring1 = "";
            _payment = _paymentConfig.paymentModels;
            if (multiPayment)
            {
                //string cmdtextMultiPay = string.Format(Queries.GET_PAYMENTNAME, count);
                string cmdtextMultiPay = string.Format(Queries.GET_PAYMENTNAME, count);
                DataTable dt = _dbAccess.GetDataTable(cmdtextMultiPay);
                if (dt != null && dt.Rows.Count > 0)
                {
                    paymenttype = dt.Rows[0]["Name2"].ToString();
                }
            }
            else
            {
                //taena bahala na spaghetti code nalang muna to
                string cmdtext = String.Format("Select String1 From {0} Where OrderNo = '{1}'", ParadoxTable.ORDERS, orNum);
                DataTable dt = _dbParadox.GetDataTable(cmdtext); 
                if (dt != null && dt.Rows.Count > 0)
                {
                    paymenttype = dt.Rows[0]["String1"].ToString();
                }

                string cmdpay = String.Format(String.Format("Select Remark From DefPay Where Name1 = '{0}'", paymenttype));
                DataTable dtpay = _dbParadox.GetDataTable(cmdpay);
                if (dtpay != null)
                {
                    foreach (DataRow row in dtpay.Rows)
                    {
                        if (row["Remark"].ToString() == "10")
                        {
                            string cmdotherpay = String.Format("Select * From {0} Where OrderNo = '{1}'", ParadoxTable.PAYMENTS, orNum);
                            DataTable dtotherpay = _dbParadox.GetDataTable(cmdotherpay);
                            if (dtotherpay != null && dtotherpay.Rows.Count > 0)
                            {
                                otherpaystring1 = dt.Rows[0]["String1"].ToString();
                            }

                            string cmddefpay2 = String.Format("Select * From DEFPAY2 Where code = '{0}'", otherpaystring1);
                            DataTable dtdefpay2 = _dbParadox.GetDataTable(cmddefpay2);
                            if (dtdefpay2 != null && dtdefpay2.Rows.Count > 0)
                            {
                                paymenttype = dtdefpay2.Rows[0]["Name1"] == DBNull.Value ? "CASH" : Convert.ToString(dtdefpay2.Rows[0]["Name1"]);
                            }
                        }
                    }
                }
            }
            return paymenttype;
        }

        private string GetTranType(string tableNo)
        {
            string trantype = "D";
            string getTableNo = string.Format(Queries.GET_TRANTYPE,"Tables","Area",tableNo);
            DataTable dtitemTable = _dbParadox.GetDataTable(getTableNo);
            if (dtitemTable.Rows.Count > 0)
            {
                foreach (DataRow dr in dtitemTable.Rows)
                {
                    if (dr["AreaName"].ToString().ToUpper().Contains("DINE IN"))
                    {
                        trantype = "D";
                    }
                    else if(dr["AreaName"].ToString().ToUpper().Contains("TO GO"))
                    {
                        trantype = "D";
                    }
                    else 
                    {
                        trantype = "C";
                    }
                }
            }
            return trantype;
        }

        private void WorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Console.WriteLine("Progress: " + e.ProgressPercentage + "%");
        }
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            notifyIcon1.Dispose();
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon1.Dispose();
            Environment.Exit(0);
        }
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (InputPassword wait = new InputPassword(this))
            {
                if (!wait.ShowDialog().Equals(DialogResult.OK))
                {
                    return;
                }
                else
                {
                    if (GlobalVar.inputPassword != "vcsi123!@#")
                    {
                        MessageBox.Show("Incorrect Passcode!", "Authorization Failed", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                        return;
                    }
                    else
                    {
                        frmConfig config = new frmConfig();
                        config.ShowDialog();
                    }
                }
            }
        }

        private void EODToolStripMenuItemipMenuItem1_Click(object sender, EventArgs e)
        { 
            using (InputPassword wait = new InputPassword(this))
            {
                if (!wait.ShowDialog().Equals(DialogResult.OK))
                {
                    return;
                }
                else
                {
                    if (GlobalVar.inputPassword != EODPass)
                    {
                        MessageBox.Show("Incorrect Passcode!", "Authenticate Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                        return;
                    }
                    else
                    {
                        _timer.Stop();
                        InitilizeEODValues();
                        BackgroundWorker worker = sender as BackgroundWorker;
                        GenerateSales(worker, null);

                        //generation of EOD file
                        //string dateNow = new DateTime(2023, 02, 14).ToString();
                        string dateNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).ToString();
                        DateTime date = DateTime.ParseExact(dateNow, "MM/dd/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);  //error : 'String was not recognized as a valid 
                        tranDate = date.ToString("yyyy-MM-dd");
                        fileDate = date.ToString("MMddyy");
                        DataTable dt = transactionDataStorage.GetInvoiceTransactions(date);

                        DataTable dtMin = transactionDataStorage.GetMINOr(date);
                        foreach (DataRow rowMin in dtMin.Rows)
                        {
                            EodSTRANS = rowMin["STARTOR"].ToString();
                        }
                        DataTable dtMax = transactionDataStorage.GetMAXOr(date);
                        foreach (DataRow rowMax in dtMax.Rows)
                        {
                            EodETRANS = rowMax["ENDOR"].ToString();
                        }

                        foreach (DataRow row in dt.Rows)
                        {
                            EodCCCODE = row["CCCODE"].ToString();
                            EodMERCHANT_NAME = row["MERCHANT_NAME"].ToString();
                            EodTER_NO = row["TER_NO"].ToString();
                            EodTRN_DATE = row["TRN_DATE"].ToString();

                            #region VAT_AMNT, VATABLE_SLS, NONVAT_SLS, VATEXEMPT_SLS, VATEXEMPT_AMNT, LOCAL_TAX, VOID_AMNT, REFUND_AMT, NO_REFUND

                            EodVAT_AMNT = EodVAT_AMNT + row["VAT_AMNT"].ToSafeDecimal();

                            EodVATABLE_SLS = EodVATABLE_SLS + row["VATABLE_SLS"].ToSafeDecimal();

                            EodNONVAT_SLS = EodNONVAT_SLS + row["NONVAT_SLS"].ToSafeDecimal();

                            EodVATEXEMPT_SLS = EodVATEXEMPT_SLS + row["VATEXEMPT_SLS"].ToSafeDecimal();

                            EodVATEXEMPT_AMNT = EodVATEXEMPT_AMNT + row["VATEXEMPT_AMNT"].ToSafeDecimal();
                            if (row["VATEXEMPT_AMNT"].ToSafeDecimal() > 0)
                            {
                                EodNO_VATEXEMT = EodNO_VATEXEMT + 1;
                            }

                            EodLOCAL_TAX = EodLOCAL_TAX + row["LOCAL_TAX"].ToSafeDecimal();

                            //EodVOID_AMNT = EodVOID_AMNT + row["VOID_AMNT"].ToSafeDecimal();

                            //EodNO_VOID = EodNO_VOID + row["NO_VOID"].ToSafeInteger();

                            EodREFUND_AMT = EodREFUND_AMT + row["REFUND_AMT"].ToSafeDecimal();
                            if (row["REFUND_AMT"].ToSafeDecimal() > 0)
                            {
                                EodNO_REFUND = EodNO_REFUND + 1;
                            }

                            #endregion

                            #region SNRCIT_DISC, NO_SNRCIT, PWD_DISC, NO_PWD, EMPLO_DISC, NO_EMPLO, AYALA_DISC, NO_AYALA, STORE_DISC, NO_STORE

                            EodSNRCIT_DISC = EodSNRCIT_DISC + row["SNRCIT_DISC"].ToSafeDecimal();
                            if (row["SNRCIT_DISC"].ToSafeDecimal() > 0)
                            {
                                EodNO_SNRCIT = EodNO_SNRCIT + 1;
                            }

                            EodPWD_DISC = EodPWD_DISC + row["PWD_DISC"].ToSafeDecimal();
                            if (row["PWD_DISC"].ToSafeDecimal() > 0)
                            {
                                EodNO_PWD = EodNO_PWD + 1;
                            }

                            EodEMPLO_DISC = EodEMPLO_DISC + row["EMPLO_DISC"].ToSafeDecimal();
                            if (row["EMPLO_DISC"].ToSafeDecimal() > 0)
                            {
                                EodNO_EMPLO = EodNO_EMPLO + 1;
                            }

                            EodAYALA_DISC = EodAYALA_DISC + row["AYALA_DISC"].ToSafeDecimal();
                            if (row["AYALA_DISC"].ToSafeDecimal() > 0)
                            {
                                EodNO_AYALA = EodNO_AYALA + 1;
                            }

                            EodSTORE_DISC = EodSTORE_DISC + row["STORE_DISC"].ToSafeDecimal();
                            if (row["STORE_DISC"].ToSafeDecimal() > 0)
                            {
                                EodNO_STORE = EodNO_STORE + 1;
                            }

                            EodOTHER_DISC = EodOTHER_DISC + row["OTHER_DISC"].ToSafeDecimal();
                            if (row["OTHER_DISC"].ToSafeDecimal() > 0)
                            {
                                EodNO_OTHER_DISC = EodNO_OTHER_DISC + 1;
                            }

                            EodSCHRGE_AMT = EodSCHRGE_AMT + row["SCHRGE_AMT"].ToSafeDecimal();
                            if (row["SCHRGE_AMT"].ToSafeDecimal() > 0)
                            {
                                EodNO_SCHRGE = EodNO_SCHRGE + 1;
                            }

                            EodOTHER_SCHR = EodOTHER_SCHR + row["OTHER_SCHR"].ToSafeDecimal();
                            if (row["OTHER_SCHR"].ToSafeDecimal() > 0)
                            {
                                EodNO_OTHER_SUR = EodNO_OTHER_SUR + 1;
                            }

                            #endregion

                            #region Payment Sales

                            EodCASH_SLS = EodCASH_SLS + row["CASH_SLS"].ToSafeDecimal();
                            if (row["CASH_SLS"].ToSafeDecimal() > 0)
                            {
                                EodNO_CASH = EodNO_CASH + 1;
                            }

                            EodCARD_SLS = EodCARD_SLS + row["CARD_SLS"].ToSafeDecimal();

                            EodEPAY_SLS = EodEPAY_SLS + row["EPAY_SLS"].ToSafeDecimal();
                            if (row["EPAY_SLS"].ToSafeDecimal() > 0)
                            {
                                EodNO_EPAY = EodNO_EPAY + 1;
                            }

                            EodDCARD_SLS = EodDCARD_SLS + row["DCARD_SLS"].ToSafeDecimal();

                            EodOTHER_SLS = EodOTHER_SLS + row["OTHERSL_SLS"].ToSafeDecimal();
                            if (row["OTHERSL_SLS"].ToSafeDecimal() > 0)
                            {
                                EodNO_OTHER_SLS = EodNO_OTHER_SLS + 1;
                            }

                            EodCHECK_SLS = EodCHECK_SLS + row["CHECK_SLS"].ToSafeDecimal();
                            if (row["CHECK_SLS"].ToSafeDecimal() > 0)
                            {
                                EodNO_CHECK = EodNO_CHECK + 1;
                            }

                            EodGC_SLS = EodGC_SLS + row["GC_SLS"].ToSafeDecimal();
                            if (row["GC_SLS"].ToSafeDecimal() > 0)
                            {
                                EodNO_GC = EodNO_GC + 1;
                            }

                            EodMASTERCARD_SLS = EodMASTERCARD_SLS + row["MASTERCARD_SLS"].ToSafeDecimal();
                            if (row["MASTERCARD_SLS"].ToSafeDecimal() > 0)
                            {
                                EodNO_MASTERCARD_SLS = EodNO_MASTERCARD_SLS + 1;
                            }

                            EodVISA_SLS = EodVISA_SLS + row["VISA_SLS"].ToSafeDecimal();
                            if (row["VISA_SLS"].ToSafeDecimal() > 0)
                            {
                                EodNO_VISA_SLS = EodNO_VISA_SLS + 1;
                            }

                            EodAMEX_SLS = EodAMEX_SLS + row["AMEX_SLS"].ToSafeDecimal();
                            if (row["AMEX_SLS"].ToSafeDecimal() > 0)
                            {
                                EodNO_AMEX_SLS = EodNO_AMEX_SLS + 1;
                            }

                            EodDINERS_SLS = EodDINERS_SLS + row["DINERS_SLS"].ToSafeDecimal();
                            if (row["DINERS_SLS"].ToSafeDecimal() > 0)
                            {
                                EodNO_DINERS_SLS = EodNO_DINERS_SLS + 1;
                            }

                            EodJCB_SLS = EodJCB_SLS + row["JCB_SLS"].ToSafeDecimal();
                            if (row["JCB_SLS"].ToSafeDecimal() > 0)
                            {
                                EodNO_JCB_SLS = EodNO_JCB_SLS + 1;
                            }

                            EodGCASH_SLS = EodGCASH_SLS + row["GCASH_SLS"].ToSafeDecimal();
                            if (row["GCASH_SLS"].ToSafeDecimal() > 0)
                            {
                                EodNO_GCASH_SLS = EodNO_GCASH_SLS + 1;
                            }

                            EodPAYMAYA_SLS = EodPAYMAYA_SLS + row["PAYMAYA_SLS"].ToSafeDecimal();
                            if (row["PAYMAYA_SLS"].ToSafeDecimal() > 0)
                            {
                                EodNO_PAYMAYA_SLS = EodNO_PAYMAYA_SLS + 1;
                            }

                            EodALIPAY_SLS = EodALIPAY_SLS + row["ALIPAY_SLS"].ToSafeDecimal();
                            if (row["ALIPAY_SLS"].ToSafeDecimal() > 0)
                            {
                                EodNO_ALIPAY_SLS = EodNO_ALIPAY_SLS + 1;
                            }

                            EodWECHAT_SLS = EodWECHAT_SLS + row["WECHAT_SLS"].ToSafeDecimal();
                            if (row["WECHAT_SLS"].ToSafeDecimal() > 0)
                            {
                                EodNO_WECHAT_SLS = EodNO_WECHAT_SLS + 1;
                            }

                            EodGRAB_SLS = EodGRAB_SLS + row["GRAB_SLS"].ToSafeDecimal();
                            if (row["GRAB_SLS"].ToSafeDecimal() > 0)
                            {
                                EodNO_GRAB_SLS = EodNO_GRAB_SLS + 1;
                            }

                            EodFOODPANDA_SLS = EodFOODPANDA_SLS + row["FOODPANDA_SLS"].ToSafeDecimal();
                            if (row["FOODPANDA_SLS"].ToSafeDecimal() > 0)
                            {
                                EodNO_FOODPANDA_SLS = EodNO_FOODPANDA_SLS + 1;
                            }

                            EodMASTERDEBIT_SLS = EodMASTERDEBIT_SLS + row["MASTERDEBIT_SLS"].ToSafeDecimal();
                            if (row["MASTERDEBIT_SLS"].ToSafeDecimal() > 0)
                            {
                                EodNO_MASTERDEBIT_SLS = EodNO_MASTERDEBIT_SLS + 1;
                            }

                            EodVISADEBIT_SLS = EodVISADEBIT_SLS + row["VISADEBIT_SLS"].ToSafeDecimal();
                            if (row["VISADEBIT_SLS"].ToSafeDecimal() > 0)
                            {
                                EodNO_VISADEBIT_SLS = EodNO_VISADEBIT_SLS + 1;
                            }

                            EodPAYPAL_SLS = EodPAYPAL_SLS + row["PAYPAL_SLS"].ToSafeDecimal();
                            if (row["PAYPAL_SLS"].ToSafeDecimal() > 0)
                            {
                                EodNO_PAYPAL_SLS = EodNO_PAYPAL_SLS + 1;
                            }

                            EodONLINE_SLS = EodONLINE_SLS + row["ONLINE_SLS"].ToSafeDecimal();
                            if (row["ONLINE_SLS"].ToSafeDecimal() > 0)
                            {
                                EodNO_ONLINE_SLS = EodNO_ONLINE_SLS + 1;
                            }

                            EodOPEN_SALES = EodOPEN_SALES + row["OPEN_SALES"].ToSafeDecimal();
                            if (row["OPEN_SALES"].ToSafeDecimal() > 0)
                            {
                                EodNO_OPEN_SALES = EodNO_OPEN_SALES + 1;
                            }

                            EodOPEN_SALES_2 = EodOPEN_SALES_2 + row["OPEN_SALES_2"].ToSafeDecimal();
                            EodOPEN_SALES_3 = EodOPEN_SALES_3 + row["OPEN_SALES_3"].ToSafeDecimal();
                            EodOPEN_SALES_4 = EodOPEN_SALES_4 + row["OPEN_SALES_4"].ToSafeDecimal();
                            EodOPEN_SALES_5 = EodOPEN_SALES_5 + row["OPEN_SALES_5"].ToSafeDecimal();
                            EodOPEN_SALES_6 = EodOPEN_SALES_6 + row["OPEN_SALES_6"].ToSafeDecimal();
                            EodOPEN_SALES_7 = EodOPEN_SALES_7 + row["OPEN_SALES_7"].ToSafeDecimal();
                            EodOPEN_SALES_8 = EodOPEN_SALES_8 + row["OPEN_SALES_8"].ToSafeDecimal();
                            EodOPEN_SALES_9 = EodOPEN_SALES_9 + row["OPEN_SALES_9"].ToSafeDecimal();
                            EodOPEN_SALES_10 = EodOPEN_SALES_10 + row["OPEN_SALES_10"].ToSafeDecimal();
                            EodOPEN_SALES_11 = EodOPEN_SALES_11 + row["OPEN_SALES_11"].ToSafeDecimal();
                            EodGC_EXCESS = EodGC_EXCESS + row["GC_EXCESS"].ToSafeDecimal();

                            EodNO_OPEN_SALES_2 = 0;
                            EodNO_OPEN_SALES_3 = 0;
                            EodNO_OPEN_SALES_4 = 0;
                            EodNO_OPEN_SALES_5 = 0;
                            EodNO_OPEN_SALES_6 = 0;
                            EodNO_OPEN_SALES_7 = 0;
                            EodNO_OPEN_SALES_8 = 0;
                            EodNO_OPEN_SALES_9 = 0;
                            EodNO_OPEN_SALES_10 = 0;
                            EodNO_OPEN_SALES_11 = 0;
                            EodNO_NOSALE = 0; // to be confirmed pa to

                            #endregion

                            #region Count of needed transactions

                            EodNO_CUST = EodNO_CUST + row["NO_CUST"].ToSafeInteger();
                            EodNO_TRN = EodNO_TRN + 1;

                            #endregion


                        }

                        #region GROSS , OLD and NEW GT , DISCOUNTS, NO_DISC

                        EodDISCOUNTS = EodSNRCIT_DISC + EodPWD_DISC + EodEMPLO_DISC + EodAYALA_DISC + EodSTORE_DISC + EodOTHER_DISC; //22 + 24 + 26 + 28 + 30 + 32
                        EodNO_DISC = EodNO_SNRCIT + EodNO_PWD + EodNO_EMPLO + EodNO_AYALA + EodNO_STORE + EodNO_OTHER_DISC; //23 + 25 + 27 + 29 + 31 + 33
                        EodNO_CARD = EodNO_MASTERCARD_SLS + EodNO_VISA_SLS + EodNO_AMEX_SLS + EodNO_DINERS_SLS + EodNO_JCB_SLS; //80 + 81 + 82 + 83 + 84
                        EodNO_DCARD_SLS = EodNO_MASTERDEBIT_SLS + EodNO_VISADEBIT_SLS; // 91 + 92
                        EodGROSS_SLS = EodVAT_AMNT + EodVATABLE_SLS + EodNONVAT_SLS + EodVATEXEMPT_SLS + EodVATEXEMPT_AMNT + EodLOCAL_TAX + EodVOID_AMNT + EodDISCOUNTS + EodREFUND_AMT + EodSCHRGE_AMT; // 8 + 9 + 10 + 11 + 12 + 15 + 16 + 18 + 20 + 34
                        #endregion

                        #region select sa eod temp
                        DataTable dtEOD = dailyData.GetLastEOD(date);
                        foreach (DataRow rowEOD in dtEOD.Rows)
                        {
                            EodPREV_EODCTR = rowEOD["EODCTR"].ToSafeInteger();
                            EodOLD_GRNTOT = rowEOD["OLD_GRNTOT"].ToSafeInteger();

                        }
                        EodNEW_GRNTOT = EodOLD_GRNTOT + EodGROSS_SLS;
                        EodEODCTR = EodPREV_EODCTR + 1;
                        #endregion


                        ///save to tempDB
                        var OrderEOD = new DailyData
                        {
                            CCCODE = EodCCCODE,
                            MERCHANT_NAME = EodMERCHANT_NAME,
                            TER_NO = EodTER_NO,
                            TRN_DATE = EodTRN_DATE,
                            STRANS = EodSTRANS,
                            ETRANS = EodETRANS,
                            GROSS_SLS = EodGROSS_SLS.ToDecimalPlaces(2),
                            VAT_AMNT = EodVAT_AMNT.ToDecimalPlaces(2),
                            VATABLE_SLS = EodVATABLE_SLS.ToDecimalPlaces(2),
                            NONVAT_SLS = EodNONVAT_SLS.ToDecimalPlaces(2),
                            VATEXEMPT_SLS = EodVATEXEMPT_SLS.ToDecimalPlaces(2),
                            VATEXEMPT_AMNT = EodVATEXEMPT_AMNT.ToDecimalPlaces(2),
                            OLD_GRNTOT = EodOLD_GRNTOT.ToDecimalPlaces(2),
                            NEW_GRNTOT = EodNEW_GRNTOT.ToDecimalPlaces(2),
                            LOCAL_TAX = EodLOCAL_TAX.ToDecimalPlaces(2),
                            VOID_AMNT = EodVOID_AMNT.ToDecimalPlaces(2),
                            NO_VOID = EodNO_VOID.ToSafeInteger(),
                            DISCOUNTS = EodDISCOUNTS.ToDecimalPlaces(2),
                            NO_DISC = EodNO_DISC.ToSafeInteger(),
                            REFUND_AMT = EodREFUND_AMT.ToDecimalPlaces(2),
                            NO_REFUND = EodNO_REFUND.ToSafeInteger(),
                            SNRCIT_DISC = EodSNRCIT_DISC.ToDecimalPlaces(2),
                            NO_SNRCIT = EodNO_SNRCIT.ToSafeInteger(),
                            PWD_DISC = EodPWD_DISC.ToDecimalPlaces(2),
                            NO_PWD = EodNO_PWD.ToSafeInteger(),
                            EMPLO_DISC = EodEMPLO_DISC.ToDecimalPlaces(2),
                            NO_EMPLO = EodNO_EMPLO.ToSafeInteger(),
                            AYALA_DISC = EodAYALA_DISC.ToDecimalPlaces(2),
                            NO_AYALA = EodNO_AYALA.ToSafeInteger(),
                            STORE_DISC = EodSTORE_DISC.ToDecimalPlaces(2),
                            NO_STORE = EodNO_STORE.ToSafeInteger(),
                            OTHER_DISC = EodOTHER_DISC.ToDecimalPlaces(2),
                            NO_OTHER_DISC = EodNO_OTHER_DISC.ToSafeInteger(),
                            SCHRGE_AMT = EodSCHRGE_AMT.ToDecimalPlaces(2),
                            OTHER_SCHR = EodOTHER_SCHR.ToDecimalPlaces(2),
                            CASH_SLS = EodCASH_SLS.ToDecimalPlaces(2),
                            CARD_SLS = EodCARD_SLS.ToDecimalPlaces(2),
                            EPAY_SLS = EodEPAY_SLS.ToDecimalPlaces(2),
                            DCARD_SLS = EodDCARD_SLS.ToDecimalPlaces(2),
                            OTHER_SLS = EodOTHER_SLS.ToDecimalPlaces(2),
                            CHECK_SLS = EodCHECK_SLS.ToDecimalPlaces(2),
                            GC_SLS = EodGC_SLS.ToDecimalPlaces(2),
                            MASTERCARD_SLS = EodMASTERCARD_SLS.ToDecimalPlaces(2),
                            VISA_SLS = EodVISA_SLS.ToDecimalPlaces(2),
                            AMEX_SLS = EodAMEX_SLS.ToDecimalPlaces(2),
                            DINERS_SLS = EodDINERS_SLS.ToDecimalPlaces(2),
                            JCB_SLS = EodJCB_SLS.ToDecimalPlaces(2),
                            GCASH_SLS = EodGCASH_SLS.ToDecimalPlaces(2),
                            PAYMAYA_SLS = EodPAYMAYA_SLS.ToDecimalPlaces(2),
                            ALIPAY_SLS = EodALIPAY_SLS.ToDecimalPlaces(2),
                            WECHAT_SLS = EodWECHAT_SLS.ToDecimalPlaces(2),
                            GRAB_SLS = EodGRAB_SLS.ToDecimalPlaces(2),
                            FOODPANDA_SLS = EodFOODPANDA_SLS.ToDecimalPlaces(2),
                            MASTERDEBIT_SLS = EodMASTERDEBIT_SLS.ToDecimalPlaces(2),
                            VISADEBIT_SLS = EodVISADEBIT_SLS.ToDecimalPlaces(2),
                            PAYPAL_SLS = EodPAYPAL_SLS.ToDecimalPlaces(2),
                            ONLINE_SLS = EodONLINE_SLS.ToDecimalPlaces(2),
                            OPEN_SALES = EodOPEN_SALES.ToDecimalPlaces(2),
                            OPEN_SALES_2 = EodOPEN_SALES_2.ToDecimalPlaces(2),
                            OPEN_SALES_3 = EodOPEN_SALES_3.ToDecimalPlaces(2),
                            OPEN_SALES_4 = EodOPEN_SALES_4.ToDecimalPlaces(2),
                            OPEN_SALES_5 = EodOPEN_SALES_5.ToDecimalPlaces(2),
                            OPEN_SALES_6 = EodOPEN_SALES_6.ToDecimalPlaces(2),
                            OPEN_SALES_7 = EodOPEN_SALES_7.ToDecimalPlaces(2),
                            OPEN_SALES_8 = EodOPEN_SALES_8.ToDecimalPlaces(2),
                            OPEN_SALES_9 = EodOPEN_SALES_9.ToDecimalPlaces(2),
                            OPEN_SALES_10 = EodOPEN_SALES_10.ToDecimalPlaces(2),
                            OPEN_SALES_11 = EodOPEN_SALES_11.ToDecimalPlaces(2),
                            GC_EXCESS = EodGC_EXCESS.ToDecimalPlaces(2),
                            NO_VATEXEMT = EodNO_VATEXEMT.ToSafeInteger(),
                            NO_SCHRGE = EodNO_SCHRGE.ToSafeInteger(),
                            NO_OTHER_SUR = EodNO_OTHER_SUR.ToSafeInteger(),
                            NO_CASH = EodNO_CASH.ToSafeInteger(),
                            NO_CARD = EodNO_CARD.ToSafeInteger(),
                            NO_EPAY = EodNO_EPAY.ToSafeInteger(),
                            NO_DCARD_SLS = EodNO_DCARD_SLS.ToSafeInteger(),
                            NO_OTHER_SLS = EodNO_OTHER_SLS.ToSafeInteger(),
                            NO_CHECK = EodNO_CHECK.ToSafeInteger(),
                            NO_GC = EodNO_GC.ToSafeInteger(),
                            NO_MASTERCARD_SLS = EodNO_MASTERCARD_SLS.ToSafeInteger(),
                            NO_VISA_SLS = EodNO_VISA_SLS.ToSafeInteger(),
                            NO_AMEX_SLS = EodNO_AMEX_SLS.ToSafeInteger(),
                            NO_DINERS_SLS = EodNO_DINERS_SLS.ToSafeInteger(),
                            NO_JCB_SLS = EodNO_JCB_SLS.ToSafeInteger(),
                            NO_GCASH_SLS = EodNO_GCASH_SLS.ToSafeInteger(),
                            NO_PAYMAYA_SLS = EodNO_PAYMAYA_SLS.ToSafeInteger(),
                            NO_ALIPAY_SLS = EodNO_ALIPAY_SLS.ToSafeInteger(),
                            NO_WECHAT_SLS = EodNO_WECHAT_SLS.ToSafeInteger(),
                            NO_GRAB_SLS = EodNO_GRAB_SLS.ToSafeInteger(),
                            NO_FOODPANDA_SLS = EodNO_FOODPANDA_SLS.ToSafeInteger(),
                            NO_MASTERDEBIT_SLS = EodNO_MASTERDEBIT_SLS.ToSafeInteger(),
                            NO_VISADEBIT_SLS = EodNO_VISADEBIT_SLS.ToSafeInteger(),
                            NO_PAYPAL_SLS = EodNO_PAYPAL_SLS.ToSafeInteger(),
                            NO_ONLINE_SLS = EodNO_ONLINE_SLS.ToSafeInteger(),
                            NO_OPEN_SALES = EodNO_OPEN_SALES.ToSafeInteger(),
                            NO_OPEN_SALES_2 = EodNO_OPEN_SALES_2.ToSafeInteger(),
                            NO_OPEN_SALES_3 = EodNO_OPEN_SALES_3.ToSafeInteger(),
                            NO_OPEN_SALES_4 = EodNO_OPEN_SALES_4.ToSafeInteger(),
                            NO_OPEN_SALES_5 = EodNO_OPEN_SALES_5.ToSafeInteger(),
                            NO_OPEN_SALES_6 = EodNO_OPEN_SALES_6.ToSafeInteger(),
                            NO_OPEN_SALES_7 = EodNO_OPEN_SALES_7.ToSafeInteger(),
                            NO_OPEN_SALES_8 = EodNO_OPEN_SALES_8.ToSafeInteger(),
                            NO_OPEN_SALES_9 = EodNO_OPEN_SALES_9.ToSafeInteger(),
                            NO_OPEN_SALES_10 = EodNO_OPEN_SALES_10.ToSafeInteger(),
                            NO_OPEN_SALES_11 = EodNO_OPEN_SALES_11.ToSafeInteger(),
                            NO_NOSALE = EodNO_NOSALE.ToSafeInteger(),
                            NO_CUST = EodNO_CUST.ToSafeInteger(),
                            NO_TRN = EodNO_TRN.ToSafeInteger(),
                            PREV_EODCTR = EodPREV_EODCTR.ToSafeInteger(),
                            EODCTR = EodEODCTR.ToSafeInteger()
                        };

                        if (!dailyData.IsExistValue("TRN_DATE", tranDate)) //check if already generated
                        {
                            dailyData.Add(OrderEOD, false);
                        }

                        else
                        {
                            dailyData.UpdateByDate(OrderEOD, tranDate, true);
                        }


                        //for csv file creation
                        arrayOrderEOD = new DailySalesFile
                        {
                            CCCODE = EodCCCODE,
                            MERCHANT_NAME = EodMERCHANT_NAME,
                            TER_NO = EodTER_NO,
                            TRN_DATE = EodTRN_DATE,
                            STRANS = EodSTRANS,
                            ETRANS = EodETRANS,
                            GROSS_SLS = EodGROSS_SLS.ToDecimalPlaces(2),
                            VAT_AMNT = EodVAT_AMNT.ToDecimalPlaces(2),
                            VATABLE_SLS = EodVATABLE_SLS.ToDecimalPlaces(2),
                            NONVAT_SLS = EodNONVAT_SLS.ToDecimalPlaces(2),
                            VATEXEMPT_SLS = EodVATEXEMPT_SLS.ToDecimalPlaces(2),
                            VATEXEMPT_AMNT = EodVATEXEMPT_AMNT.ToDecimalPlaces(2),
                            OLD_GRNTOT = EodOLD_GRNTOT.ToDecimalPlaces(2),
                            NEW_GRNTOT = EodNEW_GRNTOT.ToDecimalPlaces(2),
                            LOCAL_TAX = EodLOCAL_TAX.ToDecimalPlaces(2),
                            VOID_AMNT = EodVOID_AMNT.ToDecimalPlaces(2),
                            NO_VOID = EodNO_VOID.ToSafeInteger(),
                            DISCOUNTS = EodDISCOUNTS.ToDecimalPlaces(2),
                            NO_DISC = EodNO_DISC.ToSafeInteger(),
                            REFUND_AMT = EodREFUND_AMT.ToDecimalPlaces(2),
                            NO_REFUND = EodNO_REFUND.ToSafeInteger(),
                            SNRCIT_DISC = EodSNRCIT_DISC.ToDecimalPlaces(2),
                            NO_SNRCIT = EodNO_SNRCIT.ToSafeInteger(),
                            PWD_DISC = EodPWD_DISC.ToDecimalPlaces(2),
                            NO_PWD = EodNO_PWD.ToSafeInteger(),
                            EMPLO_DISC = EodEMPLO_DISC.ToDecimalPlaces(2),
                            NO_EMPLO = EodNO_EMPLO.ToSafeInteger(),
                            AYALA_DISC = EodAYALA_DISC.ToDecimalPlaces(2),
                            NO_AYALA = EodNO_AYALA.ToSafeInteger(),
                            STORE_DISC = EodSTORE_DISC.ToDecimalPlaces(2),
                            NO_STORE = EodNO_STORE.ToSafeInteger(),
                            OTHER_DISC = EodOTHER_DISC.ToDecimalPlaces(2),
                            NO_OTHER_DISC = EodNO_OTHER_DISC.ToSafeInteger(),
                            SCHRGE_AMT = EodSCHRGE_AMT.ToDecimalPlaces(2),
                            OTHER_SCHR = EodOTHER_SCHR.ToDecimalPlaces(2),
                            CASH_SLS = EodCASH_SLS.ToDecimalPlaces(2),
                            CARD_SLS = EodCARD_SLS.ToDecimalPlaces(2),
                            EPAY_SLS = EodEPAY_SLS.ToDecimalPlaces(2),
                            DCARD_SLS = EodDCARD_SLS.ToDecimalPlaces(2),
                            OTHER_SLS = EodOTHER_SLS.ToDecimalPlaces(2),
                            CHECK_SLS = EodCHECK_SLS.ToDecimalPlaces(2),
                            GC_SLS = EodGC_SLS.ToDecimalPlaces(2),
                            MASTERCARD_SLS = EodMASTERCARD_SLS.ToDecimalPlaces(2),
                            VISA_SLS = EodVISA_SLS.ToDecimalPlaces(2),
                            AMEX_SLS = EodAMEX_SLS.ToDecimalPlaces(2),
                            DINERS_SLS = EodDINERS_SLS.ToDecimalPlaces(2),
                            JCB_SLS = EodJCB_SLS.ToDecimalPlaces(2),
                            GCASH_SLS = EodGCASH_SLS.ToDecimalPlaces(2),
                            PAYMAYA_SLS = EodPAYMAYA_SLS.ToDecimalPlaces(2),
                            ALIPAY_SLS = EodALIPAY_SLS.ToDecimalPlaces(2),
                            WECHAT_SLS = EodWECHAT_SLS.ToDecimalPlaces(2),
                            GRAB_SLS = EodGRAB_SLS.ToDecimalPlaces(2),
                            FOODPANDA_SLS = EodFOODPANDA_SLS.ToDecimalPlaces(2),
                            MASTERDEBIT_SLS = EodMASTERDEBIT_SLS.ToDecimalPlaces(2),
                            VISADEBIT_SLS = EodVISADEBIT_SLS.ToDecimalPlaces(2),
                            PAYPAL_SLS = EodPAYPAL_SLS.ToDecimalPlaces(2),
                            ONLINE_SLS = EodONLINE_SLS.ToDecimalPlaces(2),
                            OPEN_SALES = EodOPEN_SALES.ToDecimalPlaces(2),
                            OPEN_SALES_2 = EodOPEN_SALES_2.ToDecimalPlaces(2),
                            OPEN_SALES_3 = EodOPEN_SALES_3.ToDecimalPlaces(2),
                            OPEN_SALES_4 = EodOPEN_SALES_4.ToDecimalPlaces(2),
                            OPEN_SALES_5 = EodOPEN_SALES_5.ToDecimalPlaces(2),
                            OPEN_SALES_6 = EodOPEN_SALES_6.ToDecimalPlaces(2),
                            OPEN_SALES_7 = EodOPEN_SALES_7.ToDecimalPlaces(2),
                            OPEN_SALES_8 = EodOPEN_SALES_8.ToDecimalPlaces(2),
                            OPEN_SALES_9 = EodOPEN_SALES_9.ToDecimalPlaces(2),
                            OPEN_SALES_10 = EodOPEN_SALES_10.ToDecimalPlaces(2),
                            OPEN_SALES_11 = EodOPEN_SALES_11.ToDecimalPlaces(2),
                            GC_EXCESS = EodGC_EXCESS.ToDecimalPlaces(2),
                            NO_VATEXEMT = EodNO_VATEXEMT.ToSafeInteger(),
                            NO_SCHRGE = EodNO_SCHRGE.ToSafeInteger(),
                            NO_OTHER_SUR = EodNO_OTHER_SUR.ToSafeInteger(),
                            NO_CASH = EodNO_CASH.ToSafeInteger(),
                            NO_CARD = EodNO_CARD.ToSafeInteger(),
                            NO_EPAY = EodNO_EPAY.ToSafeInteger(),
                            NO_DCARD_SLS = EodNO_DCARD_SLS.ToSafeInteger(),
                            NO_OTHER_SLS = EodNO_OTHER_SLS.ToSafeInteger(),
                            NO_CHECK = EodNO_CHECK.ToSafeInteger(),
                            NO_GC = EodNO_GC.ToSafeInteger(),
                            NO_MASTERCARD_SLS = EodNO_MASTERCARD_SLS.ToSafeInteger(),
                            NO_VISA_SLS = EodNO_VISA_SLS.ToSafeInteger(),
                            NO_AMEX_SLS = EodNO_AMEX_SLS.ToSafeInteger(),
                            NO_DINERS_SLS = EodNO_DINERS_SLS.ToSafeInteger(),
                            NO_JCB_SLS = EodNO_JCB_SLS.ToSafeInteger(),
                            NO_GCASH_SLS = EodNO_GCASH_SLS.ToSafeInteger(),
                            NO_PAYMAYA_SLS = EodNO_PAYMAYA_SLS.ToSafeInteger(),
                            NO_ALIPAY_SLS = EodNO_ALIPAY_SLS.ToSafeInteger(),
                            NO_WECHAT_SLS = EodNO_WECHAT_SLS.ToSafeInteger(),
                            NO_GRAB_SLS = EodNO_GRAB_SLS.ToSafeInteger(),
                            NO_FOODPANDA_SLS = EodNO_FOODPANDA_SLS.ToSafeInteger(),
                            NO_MASTERDEBIT_SLS = EodNO_MASTERDEBIT_SLS.ToSafeInteger(),
                            NO_VISADEBIT_SLS = EodNO_VISADEBIT_SLS.ToSafeInteger(),
                            NO_PAYPAL_SLS = EodNO_PAYPAL_SLS.ToSafeInteger(),
                            NO_ONLINE_SLS = EodNO_ONLINE_SLS.ToSafeInteger(),
                            NO_OPEN_SALES = EodNO_OPEN_SALES.ToSafeInteger(),
                            NO_OPEN_SALES_2 = EodNO_OPEN_SALES_2.ToSafeInteger(),
                            NO_OPEN_SALES_3 = EodNO_OPEN_SALES_3.ToSafeInteger(),
                            NO_OPEN_SALES_4 = EodNO_OPEN_SALES_4.ToSafeInteger(),
                            NO_OPEN_SALES_5 = EodNO_OPEN_SALES_5.ToSafeInteger(),
                            NO_OPEN_SALES_6 = EodNO_OPEN_SALES_6.ToSafeInteger(),
                            NO_OPEN_SALES_7 = EodNO_OPEN_SALES_7.ToSafeInteger(),
                            NO_OPEN_SALES_8 = EodNO_OPEN_SALES_8.ToSafeInteger(),
                            NO_OPEN_SALES_9 = EodNO_OPEN_SALES_9.ToSafeInteger(),
                            NO_OPEN_SALES_10 = EodNO_OPEN_SALES_10.ToSafeInteger(),
                            NO_OPEN_SALES_11 = EodNO_OPEN_SALES_11.ToSafeInteger(),
                            NO_NOSALE = EodNO_NOSALE.ToSafeInteger(),
                            NO_CUST = EodNO_CUST.ToSafeInteger(),
                            NO_TRN = EodNO_TRN.ToSafeInteger(),
                            PREV_EODCTR = EodPREV_EODCTR.ToSafeInteger(),
                            EODCTR = EodEODCTR.ToSafeInteger()
                        };

                        ListOrderEOD.Add(arrayOrderEOD);
                        perTransactionsListEOD.Add(ListOrderEOD);


                        string fileName = String.Format("{0}{1}{2}.csv", "EOD", EodCCCODE, fileDate.Replace(" / ", ""));
                        filePath = Path.Combine(path, fileName);
                        helper.WriteEODCsv(filePath, perTransactionsListEOD, tranCnt);

                        PrintReceipt(date);
                        //frmInvoiceSales inv = new frmInvoiceSales();
                        //inv.ShowDialog();
                    }
                }
            }
        }

        private void PrintReceipt(DateTime date)
        {
            try
            {
                DataTable dt = dailyData.GetInvoiceTransactions(date);
                if (dt.Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (DataRow dr in dt.Rows)
                    {
                        sb.AppendLine(sm.Prefixchar(" ", 40, " "));
                        sb.AppendLine(sm.CenterString("END OF DAY RECEIPT", 34));
                        sb.AppendLine(sm.Prefixchar("-", 33, "-"));
                        sm.DisplayLongString(sb, String.Format("BIR Permit No.: {0}", salesBIR == null ? "" : salesBIR.ToUpper()), 40, false);
                        sm.DisplayLongString(sb, String.Format("POS Serial No.: {0}", salesPOSSerial == null ? "" : salesPOSSerial.ToUpper()), 40, false);
                        sb.AppendLine(sm.Prefixchar(" ", 40, " "));
                        sm.DisplayLongString(sb, String.Format("TOTAL DLYSALE: {0}", dr["GROSS_SLS"].TwoDecimalRoundedOff(2)), 40, false);
                        sm.DisplayLongString(sb, String.Format("TOTAL DISCOUNT: {0}", dr["DISCOUNTS"].TwoDecimalRoundedOff(2)), 40, false);
                        sm.DisplayLongString(sb, String.Format("TOTAL REFUND: {0}", dr["REFUND_AMT"].TwoDecimalRoundedOff(2)), 40, false);
                        sm.DisplayLongString(sb, String.Format("TOTAL CANCELLATION: {0}", dr["VOID_AMNT"].TwoDecimalRoundedOff(2)), 40, false);
                        sm.DisplayLongString(sb, String.Format("TOTAL SERVICE CHARGE: {0}", dr["SCHRGE_AMT"].TwoDecimalRoundedOff(2)), 40, false);
                        sm.DisplayLongString(sb, String.Format("TOTAL NOTAXSALE: {0}", dr["NONVAT_SLS"].TwoDecimalRoundedOff(2)), 40, false);
                        sm.DisplayLongString(sb, String.Format("TOTAL VAT: {0}", dr["VATABLE_SLS"].TwoDecimalRoundedOff(2)), 40, false);
                        sm.DisplayLongString(sb, String.Format("TOTAL CASH: {0}", dr["CASH_SLS"].TwoDecimalRoundedOff(2)), 40, false);
                        sm.DisplayLongString(sb, String.Format("TOTAL DEBIT: {0}", dr["DCARD_SLS"].TwoDecimalRoundedOff(2)), 40, false);
                        sm.DisplayLongString(sb, String.Format("TOTAL CREDIT: {0}", dr["CARD_SLS"].TwoDecimalRoundedOff(2)), 40, false);
                        sm.DisplayLongString(sb, String.Format("TOTAL MASTERCARD: {0}", dr["MASTERCARD_SLS"].TwoDecimalRoundedOff(2)), 40, false);
                        sm.DisplayLongString(sb, String.Format("TOTAL VISA: {0}", dr["VISA_SLS"].TwoDecimalRoundedOff(2)), 40, false);
                        sb.AppendLine(sm.Prefixchar(" ", 40, " "));
                        sb.AppendLine(sm.Prefixchar("-", 33, "-"));
                    }

                    printer.SelectedPrinterReceipt(sb.ToString(), salesPrinter);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Printing error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void dateRangeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}

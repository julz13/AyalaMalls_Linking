using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AyalaMalls_Linking.Helpers
{
    public class PrintClass
    {
        #region SELECTED PRINTER
        public void SelectedPrinterReceipt(string _printText, string _printerName)
        {
            try
            {
                PrintDocument p = new PrintDocument();
                p.PrinterSettings.PrinterName = _printerName;
                p.PrintPage += delegate (object sender1, PrintPageEventArgs e1)
                {
                    e1.Graphics.DrawString(_printText, new Font("Lucida Console", 11), new SolidBrush(Color.Black), new RectangleF(0, 0, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));
                };
                p.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
        }
        #endregion

        #region DEFAULT PRINTING
        public void DefaultPrinterReceipt(string _printText)
        {
            try
            {
                PrintDocument p = new PrintDocument();
                PrintController printController = new StandardPrintController();
                p.PrintController = printController;
                p.PrintPage += delegate (object sender1, PrintPageEventArgs e1)
                {
                    e1.Graphics.DrawString(_printText, new Font("Lucida Console", 10), new SolidBrush(Color.Black), new RectangleF(0, 0, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));
                };
                p.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
        }
        #endregion

        //#region COM PRINTING
        //private void OpenPrinterCOMPort(SerialPort ComPort)
        //{
        //    try
        //    {
        //        ComPort.PortName = Properties.Settings.Default.Port;
        //        ComPort.BaudRate = 9600;
        //        ComPort.DataBits = 8;
        //        ComPort.Parity = Parity.None;
        //        ComPort.StopBits = StopBits.One;
        //        ComPort.Handshake = Handshake.None;
        //        ComPort.Open();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
        //    }
        //}
        //public void PrintCOMReceipt(string _printText)
        //{
        //    try
        //    {
        //        StringManipulation sm = new StringManipulation();
        //        SerialPort ComPort = new SerialPort();
        //        OpenPrinterCOMPort(ComPort);
        //        string prntCommand;
        //        string Command1 = _printText;
        //        string CommandSent;
        //        int Length, j = 0;

        //        Length = Command1.Length;
        //        for (int i = 0; i < Length; i++)
        //        {
        //            CommandSent = Command1.Substring(j, 1);
        //            ComPort.Write(CommandSent);
        //            j++;
        //        }

        //        sm.HexString2Ascii("1B69", out prntCommand);
        //        ComPort.Write(prntCommand);
        //        ComPort.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
        //    }
        //}
        //#endregion
    }
}

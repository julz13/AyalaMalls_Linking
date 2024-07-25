using AyalaMalls_Linking.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AyalaMalls_Linking.Dialogs
{
    public partial class InputPassword : Form
    {
        public InputPassword(Control parent)
        {
            InitializeComponent();
            if (parent != null)
            {
                this.StartPosition = FormStartPosition.Manual;
                this.CenterToParent();
            }
            else
            {
                this.StartPosition = FormStartPosition.CenterParent;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOk_Click_1(object sender, EventArgs e)
        {
            GlobalVar.inputPassword = txtPassword.Text.Trim().ToString();
            this.DialogResult = DialogResult.OK;
            Close();
        }
    }
}

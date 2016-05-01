using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RezystorDoBazy;

namespace RezystorDoBazy
{
    public partial class HelpForm : Form
    {
        public HelpForm()
        {
            InitializeComponent();
            Form111 frm = new Form111();
            this.Text = frm.Text+" - Pomoc";

        }

        private void HelpForm_Load(object sender, EventArgs e)
        {

        }
    }
}

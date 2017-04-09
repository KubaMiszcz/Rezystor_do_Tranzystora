using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RezystorDoTranzystora
{
    public partial class Form111 : Form
    {
        float hfe, Ucesat, Ube, Ptot;//z DS
        float k, Rload, Uwy, Uwe;//dane
        float Ibase, P, Rbase, Uload,Iload;//wyniki
        private string _currentSavedFileName;
        private string _titleBar;
        private String wrong_decsep;
        private String dec_separator = "" + Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

        public Form111()
        {
            InitializeComponent();
            if (dec_separator == ",") wrong_decsep = ".";
            else if (dec_separator == ".") wrong_decsep = ",";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _titleBar = this.Text;
            this.TopMost = true;
            toolTip1.SetToolTip(tbRload, "R obciążenia,\njak trzeba sam prad - wcelowac w żądany prad Ic zmieniajac Rload");
            toolTip1.SetToolTip(tbUwe, "napiecie sterujace na wejsciu");
            toolTip1.SetToolTip(tbUwy, "napiecie na wyjsciu");
            toolTip1.SetToolTip(tbIload, "prad na obciązeniu Iload=Ic");
            toolTip1.SetToolTip(tbUcesat, "napiecie nasycenia");
            toolTip1.SetToolTip(tbUbe, "napiecie na zlaczu B-E");
            toolTip1.SetToolTip(tbhfe, "wzmocnienie hfe");
            toolTip1.SetToolTip(tbPtot, "maks dopuszczalna moc rozpraszana");
            toolTip1.SetToolTip(tbUload, "napiecie na obciążeniu Rload");
            toolTip1.SetToolTip(tbIbase, "prad bazy,\n uwaga na jednostke! standardowo mA\n ale jak mniej niz 1ma\n to podaje w uA");
            toolTip1.SetToolTip(tbk, "wspolczynnik przesterowania = 2do5,\n im większy tym tranzystor dłużej się wyłącza");
            toolTip1.SetToolTip(tbRbase, "rezystor do bazy, przy ktorym poplynie wyliczony Ibase");
            toolTip1.SetToolTip(tbP, "moc wydzielona na tranzystorze");

            obliczRb();
        }

        void tb2val()
        {
            Rload = float.Parse(tbRload.Text.Replace(wrong_decsep,dec_separator));
            Uwe = float.Parse(tbUwe.Text.Replace(wrong_decsep,dec_separator));
            Uwy = float.Parse(tbUwy.Text.Replace(wrong_decsep,dec_separator));
            k = float.Parse(tbk.Text.Replace(wrong_decsep,dec_separator));
            Ucesat = float.Parse(tbUcesat.Text.Replace(wrong_decsep,dec_separator));
            Ube = float.Parse(tbUbe.Text.Replace(wrong_decsep,dec_separator));
            hfe = float.Parse(tbhfe.Text.Replace(wrong_decsep,dec_separator));
            Ptot = float.Parse(tbPtot.Text.Replace(wrong_decsep,dec_separator));
       }

        void val2tb()
        {
            tbIload.Text = (Math.Round(Iload,2)).ToString();
            tbUload.Text = (Math.Round(Uload,2)).ToString();
            tbIbase.Text = (Math.Round(Ibase,2)).ToString();
            tbRbase.Text = (Math.Round(Rbase,2)).ToString();
            tbP.Text = (Math.Round(P,2)).ToString();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            HelpForm frm = new HelpForm();
            frm.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.kubamiszcz.hekko24.pl/joomla35/index.php");
        }

        private void linkmail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //System.Diagnostics.Process proc = new System.Diagnostics.Process();
            //proc.StartInfo.FileName = "mailto:zielonyeufor@gmail.com?subject='mail-Radiator Do Tranzystora'&body='wysłano z programu Radiator do Tranzystora'";
            //proc.Start();
        }
         

        private void tbk_TextChanged(object sender, EventArgs e)
        {
            obliczRb();
        }

        private void tbRload_KeyDown(object sender, KeyEventArgs e)
        {
            //Rload = float.Parse(tbRload.Text);
            if (e.KeyCode == Keys.Up)
            {
                Rload += 0.01f;
                tbRload.Text = Rload.ToString(dec_separator);
            }
            if (e.KeyCode == Keys.Down)
            {
                Rload -= 0.01f;
                tbRload.Text = Rload.ToString(dec_separator);
            }
            obliczRb();
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpForm frm = new HelpForm();
            frm.Show();
        }

        private void TopMostMenuItem1_Click(object sender, EventArgs e)
        {
            this.TopMost = !this.TopMost;
            if (this.TopMost)
                TopMostMenuItem1.Image = Properties.Resources.topmostactive;
            else
                TopMostMenuItem1.Image = Properties.Resources.topmostinactive;
        }

        private void obliczRb()
        {
            tb2val();
            Iload = (Uwy - Ucesat) / Rload;
            Uload = Uwy - Ucesat;
            Ibase = (Iload * k) / hfe;
            Rbase = (Uwe - Ube) / Ibase;
            P = Ibase*Ube + Iload * Ucesat;

            if (P < Ptot)
            {
                pictureBox1.Image = RezystorDoTranzystora.Properties.Resources.ok;
                tbP.BackColor = Color.PeachPuff;
            }
            else
            {
                pictureBox1.Image = RezystorDoTranzystora.Properties.Resources.error;
                tbP.BackColor = Color.Red;
            }

            Iload=Iload * 1000f;//A->mA
            Ibase = Ibase * 1000f;//A->mA
            if (Ibase < 1f)
            {
                Ibase = Ibase * 1000f;//mA->uA
                label9.Text = "Ibase [uA]";
            }
            else
            {
                label9.Text = "Ibase [mA]";
            }
            Rbase = Rbase / 1000f; //ohm->kohm
            val2tb();
        }

        private void tbJ_TextChanged(object sender, EventArgs e)
        {
            obliczRb();
        }

        private void tbU_TextChanged(object sender, EventArgs e)
        {
            obliczRb();
        }

        private void tbRthjc_TextChanged(object sender, EventArgs e)
        {
            obliczRb();
        }

        private void tbRthcs_TextChanged(object sender, EventArgs e)
        {
            obliczRb();
        }

        private void tbRthsa_TextChanged(object sender, EventArgs e)
        {
            obliczRb();
        }

        private void tbP_TextChanged(object sender, EventArgs e)
        {
            obliczRb();
        }

        private void tbTamb_TextChanged(object sender, EventArgs e)
        {
            obliczRb();
        }

        private void tbRth_TextChanged(object sender, EventArgs e)
        {
            obliczRb();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            obliczRb();
        }
    }
}

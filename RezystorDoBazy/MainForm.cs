using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RezystorDoBazy
{
    public partial class Form111 : Form
    {
        float hfe, Ucesat, Ube, Ptot;//z DS
        float k, Rl, Uwy, Uwe;//dane
        float Ib, P, Rb, Uload,Iload;//wyniki
        

        public Form111()
        {
            InitializeComponent();

        }

        void tb2val()
        {
            Rl = float.Parse(tbRl.Text, System.Globalization.CultureInfo.InvariantCulture);
            Uwe = float.Parse(tbUwe.Text, System.Globalization.CultureInfo.InvariantCulture);
            Uwy = float.Parse(tbUwy.Text, System.Globalization.CultureInfo.InvariantCulture);
            k = float.Parse(tbk.Text, System.Globalization.CultureInfo.InvariantCulture);
            Ucesat = float.Parse(tbUcesat.Text, System.Globalization.CultureInfo.InvariantCulture);
            Ube = float.Parse(tbUbe.Text, System.Globalization.CultureInfo.InvariantCulture);
            hfe = float.Parse(tbhfe.Text, System.Globalization.CultureInfo.InvariantCulture);
            Ptot = float.Parse(tbPtot.Text, System.Globalization.CultureInfo.InvariantCulture);
       }

        void val2tb()
        {
            tbIload.Text = Math.Round(Iload,2).ToString(System.Globalization.CultureInfo.InvariantCulture);
            tbUload.Text = Math.Round(Uload,2).ToString(System.Globalization.CultureInfo.InvariantCulture);
            tbIb.Text = Math.Round(Ib,2).ToString(System.Globalization.CultureInfo.InvariantCulture);
            tbRb.Text = Math.Round(Rb,2).ToString(System.Globalization.CultureInfo.InvariantCulture);
            tbP.Text = Math.Round(P,2).ToString(System.Globalization.CultureInfo.InvariantCulture);
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
         

        private void Form1_Load(object sender, EventArgs e)
        {
            
            toolTip1.SetToolTip(tbRl, "R obciążenia,\njak trzeba sam prad - wcelowac w żądany prad Ic zmieniajac Rload");
            toolTip1.SetToolTip(tbUwe, "napiecie sterujace na wejsciu");
            toolTip1.SetToolTip(tbUwy, "napiecie na wyjsciu");
            toolTip1.SetToolTip(tbIload, "prad na obciązeniu Iload=Ic");
            toolTip1.SetToolTip(tbUcesat, "napiecie nasycenia");
            toolTip1.SetToolTip(tbUbe, "napiecie na zlaczu B-E");
            toolTip1.SetToolTip(tbhfe, "wzmocnienie hfe");
            toolTip1.SetToolTip(tbPtot, "maks dopuszczalna moc rozpraszana");
            toolTip1.SetToolTip(tbUload, "napiecie na obciążeniu Rload");
            toolTip1.SetToolTip(tbIb, "prad bazy,\n uwaga na jednostke! standardowo mA\n ale jak mniej niz 1ma\n to podaje w uA");
            toolTip1.SetToolTip(tbk, "wspolczynnik przesterowania = 2do5,\n im większy tym tranzystor dłużej się wyłącza");
            toolTip1.SetToolTip(tbRb, "rezystor do bazy, przy ktorym poplynie wyliczony Ib");
            toolTip1.SetToolTip(tbP, "moc wydzielona na tranzystorze");

            obliczRb();
        }

        private void tbk_TextChanged(object sender, EventArgs e)
        {
            obliczRb();
        }

        private void obliczRb()
        {
            tb2val();
            Iload = (Uwy - Ucesat) / Rl;
            Uload = Uwy - Ucesat;
            Ib = (Iload * k) / hfe;
            Rb = (Uwe - Ube) / Ib;
            P = Ib*Ube + Iload * Ucesat;

            if (P < Ptot)
            {
                pictureBox1.Image = RezystorDoBazy.Properties.Resources.ok;
                tbP.BackColor = Color.PeachPuff;
            }
            else
            {
                pictureBox1.Image = RezystorDoBazy.Properties.Resources.error;
                tbP.BackColor = Color.Red;
            }

            Iload=Iload * 1000f;//A->mA
            Ib = Ib * 1000f;//A->mA
            if (Ib < 1f)
            {
                Ib = Ib * 1000f;//mA->uA
                label9.Text = "Ib [uA]";
            }
            else
            {
                label9.Text = "Ib [mA]";
            }
            Rb = Rb / 1000f; //ohm->kohm
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

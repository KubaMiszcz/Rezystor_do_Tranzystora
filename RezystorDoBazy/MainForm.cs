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
        private NumberFormatInfo nfi;


        public Form111()
        {
            InitializeComponent();
            nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _titleBar = this.Text;

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
            Rload = float.Parse(tbRload.Text, CultureInfo.InvariantCulture);
            Uwe = float.Parse(tbUwe.Text, CultureInfo.InvariantCulture);
            Uwy = float.Parse(tbUwy.Text, CultureInfo.InvariantCulture);
            k = float.Parse(tbk.Text, CultureInfo.InvariantCulture);
            Ucesat = float.Parse(tbUcesat.Text, CultureInfo.InvariantCulture);
            Ube = float.Parse(tbUbe.Text, CultureInfo.InvariantCulture);
            hfe = float.Parse(tbhfe.Text, CultureInfo.InvariantCulture);
            Ptot = float.Parse(tbPtot.Text, CultureInfo.InvariantCulture);
       }

        void val2tb()
        {
            tbIload.Text = Math.Round(Iload,2).ToString(nfi);
            tbUload.Text = Math.Round(Uload,2).ToString(nfi);
            tbIbase.Text = Math.Round(Ibase,2).ToString(nfi);
            tbRbase.Text = Math.Round(Rbase,2).ToString(nfi);
            tbP.Text = Math.Round(P,2).ToString(nfi);
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

        private void zapiszJakoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            {
                saveFileDialog1.Filter = "Txt files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 1;
                if (!(Directory.Exists(Directory.GetCurrentDirectory() + "\\Saved Transistors")))//jesli nie istnieje
                {
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Saved Transistors");
                }
                saveFileDialog1.InitialDirectory=Directory.GetCurrentDirectory() + "\\Saved Transistors";
                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.DefaultExt = ".pn";
                saveFileDialog1.FileName = "Tranzystor1";
                Stream myStream;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if ((myStream = saveFileDialog1.OpenFile()) != null)
                    {
                        using (StreamWriter writer = new StreamWriter(myStream, System.Text.Encoding.UTF8))
                        {
                            writer.WriteLine(_titleBar);
                            writer.WriteLine(tbUwe.Text);
                            writer.WriteLine(tbRload.Text);
                            writer.WriteLine(tbUwy.Text);
                            writer.WriteLine(tbk.Text);
                            writer.WriteLine(tbUcesat.Text);
                            writer.WriteLine(tbUbe.Text);
                            writer.WriteLine(tbhfe.Text);
                            writer.WriteLine(tbPtot.Text);
                        }
                        myStream.Close();

                    }
                }
                //String AktualnyPlikDoZapisu_nazwa = System.IO.Path.GetFileNameWithoutExtension(AktualnyPlikDoZapisu);
                _currentSavedFileName = saveFileDialog1.FileName;
                this.Text = _titleBar + " - " + System.IO.Path.GetFileNameWithoutExtension(_currentSavedFileName); 
            }
        }

        private void zapiszToolStripMenuItem_Click(object sender, EventArgs e)
        {
            {
                if (_currentSavedFileName == null) zapiszJakoToolStripMenuItem_Click(sender, e);
                
                FileStream file = new FileStream(_currentSavedFileName, FileMode.Create);
                using (StreamWriter writer = new StreamWriter(file, System.Text.Encoding.UTF8))
                {
                    writer.WriteLine(_titleBar);
                    writer.WriteLine(tbUwe.Text);
                    writer.WriteLine(tbRload.Text);
                    writer.WriteLine(tbUwy.Text);
                    writer.WriteLine(tbk.Text);
                    writer.WriteLine(tbUcesat.Text);
                    writer.WriteLine(tbUbe.Text);
                    writer.WriteLine(tbhfe.Text);
                    writer.WriteLine(tbPtot.Text);
                }
                file.Close();
            }
        }

        private void wczytajToolStripMenuItem_Click(object sender, EventArgs e)
        {
            {
                Stream myStream = null;
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "Txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog1.FilterIndex = 1;
                if (!(Directory.Exists(Directory.GetCurrentDirectory() + "\\Saved Transistors")))//jesli nie istnieje
                {
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Saved Transistors");
                }
                openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory() + "\\Saved Transistors";
                openFileDialog1.RestoreDirectory = true;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if ((myStream = openFileDialog1.OpenFile()) != null)
                        {
                            using (StreamReader reader = new StreamReader(myStream, System.Text.Encoding.UTF8))
                            {
                                // Insert code to read the stream here.
                                while (!reader.EndOfStream)
                                {
                                    this.Text= reader.ReadLine(); 
                                    
                                    tbUwe.Text= reader.ReadLine();
                                    tbRload.Text=reader.ReadLine();
                                    tbUwy.Text=reader.ReadLine();
                                    tbk.Text=reader.ReadLine();
                                    tbUcesat.Text=reader.ReadLine();
                                    tbUbe.Text=reader.ReadLine();
                                    tbhfe.Text=reader.ReadLine();
                                    tbPtot.Text=reader.ReadLine();

                                    obliczRb();
                                }

                                myStream.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + ex.StackTrace);
                    }
                }
                //String AktualnyPlikDoZapisu_nazwa = System.IO.Path.GetFileNameWithoutExtension(AktualnyPlikDoZapisu);
                _currentSavedFileName = openFileDialog1.FileName;
                if (_currentSavedFileName != "")
                {
                    this.Text = _titleBar + " - " + System.IO.Path.GetFileNameWithoutExtension(_currentSavedFileName);
                }
            }
        }

        private void tbRload_KeyDown(object sender, KeyEventArgs e)
        {
            //Rload = float.Parse(tbRload.Text);
            if (e.KeyCode == Keys.Up)
            {
                Rload += 0.01f;
                tbRload.Text = Rload.ToString(nfi);
            }
            if (e.KeyCode == Keys.Down)
            {
                Rload -= 0.01f;
                tbRload.Text = Rload.ToString(nfi);
            }
            obliczRb();
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

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

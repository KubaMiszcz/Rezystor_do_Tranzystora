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

        private void zapiszJakoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            {
                saveFileDialog1.Filter = "Txt files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 1;
                if (!(Directory.Exists(Directory.GetCurrentDirectory() + "\\Saved Transistors")))//jesli nie istnieje
                {
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Saved Transistors");
                }
                saveFileDialog1.InitialDirectory = Directory.GetCurrentDirectory() + "\\Saved Transistors";
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
                                    this.Text = reader.ReadLine();

                                    tbUwe.Text = reader.ReadLine();
                                    tbRload.Text = reader.ReadLine();
                                    tbUwy.Text = reader.ReadLine();
                                    tbk.Text = reader.ReadLine();
                                    tbUcesat.Text = reader.ReadLine();
                                    tbUbe.Text = reader.ReadLine();
                                    tbhfe.Text = reader.ReadLine();
                                    tbPtot.Text = reader.ReadLine();

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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Serial_Communication
{
    public partial class Form1 : Form
    {
        SerialPort sp = new SerialPort(); // Seri port nesnesi oluşturuyoruz
        //SerialPort sp = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            string[] portlar = SerialPort.GetPortNames(); // Bağlı seri portları diziye aktardık
            foreach (string portAdi in portlar)
            {
                comboBox1.Items.Add(portAdi);
            }
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 4;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            

            try
            {
                if (!sp.IsOpen)
                {
                    if (comboBox1.SelectedItem != null && comboBox2.SelectedItem != null)
                    {
                        sp.PortName = comboBox1.SelectedItem.ToString(); // Kullanacağımız seri port adını seçiyoruz (String)
                        sp.BaudRate = int.Parse(comboBox2.SelectedItem.ToString());  // Seri haberleşme hızını seçiyoruz (int32)
                        sp.DataBits = 8; // göndereceğimiz bilginin kaç bitten oluşacağını bildiriyoruz (int32).
                        sp.Parity = Parity.None; // Eşlik bitidir. Gönderilen verinin doğruluğunu kontrol etmek için kullanılır. 
                        sp.StopBits = StopBits.One; // Stop bitinin kaç bit olacağını belirtir.
                    }
                    else
                    {
                        MessageBox.Show("Lütfen Bir Port Seçiniz...", "Hata");
                    }

                    sp.Open(); // Seri portumuzu açıyoruz
                    MessageBox.Show("Bağlantı Başarılı...", "Bağlantı");
                    button2.Enabled = true;
                    button3.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Bağlantı Zaten Açık...", "Bağlantı");
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Hata");
            }
            
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sp.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
            
        }
        int[] dizi = new int[100];
        int i = 0;
        private void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            int veri = sp.ReadByte();
            dizi[i] = veri;
            listBox1.Items.Add(veri.ToString());
            textBox1.Text += veri.ToString()+",";
            if (i==17)
            {
                i = -1;
            }
            i++;
            if (dizi[1]<100)
            {
                progressBar1.Value = dizi[1];
            }
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (sp.IsOpen)
            {
                sp.Close();
                MessageBox.Show("Bağlantı Durduruldu...", "Bağlantı");
                button2.Enabled = false;
                button3.Enabled = false;
            }
            else
            {
                MessageBox.Show("Bağlantı Zaten Kapalı...", "Bağlantı");
            }


        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sp.IsOpen)
            {
                sp.Close();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace Client
{
    public partial class Form1 : Form
    {
        static private Socket Client;
        private IPAddress ip = null;
        private int port = 0;
        private Thread th;

        public Form1()
        {
            InitializeComponent();

            richTextBox1.Enabled = false;
            richTextBox2.Enabled = false;
            button2.Enabled = false;

            try
            {
                var sr = new StreamReader(@"Client_Info/Data_Info.txt");
                string buffer = sr.ReadToEnd();
                sr.Close();
                string[] connect_info = buffer.Split(':');
                ip = IPAddress.Parse(connect_info[0]);
                port = int.Parse(connect_info[1]);

                label4.ForeColor = Color.Green;
                label4.Text = "Настройки: \n IP Сервера: " + connect_info[0] + "\n Порт Сервера: " + connect_info[1];
            }
            catch(Exception ex)
            {
                label4.ForeColor = Color.Red;
                /*richTextBox2.SelectionFont = new Font("Verdana", 12, FontStyle.Bold);
                richTextBox2.SelectionColor = Color.Red;*/
                label4.Text = "Настройки не найдены!";
                Form2 form = new Form2();
                form.Show();
            }


        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            form.Show();
        }

        void SendMessage(string message)
        {
            if(message != "" && message != "")
            {
                byte[] buffer = new byte[1024];
                buffer = Encoding.UTF8.GetBytes(message);
                Client.Send(buffer);
            }
        }

        void RecvMessage()
        {
            byte[] buffer = new byte[1024];

            for(int i=0; i<buffer.Length; i++)
            {
                buffer[i] = 0;
            }

            for(;;)
            {
                try
                {
                    Client.Receive(buffer);
                    string message = Encoding.UTF8.GetString(buffer);

                    int count = message.IndexOf(";;;5");

                    if(count == -1)
                    {
                        continue;
                    }

                    string Clear_Message = "";

                    for (int i = 0; i < count; i++)
                    {
                        Clear_Message += message[i];
                    }

                    for (int i = 0; i < buffer.Length; i++)
                    {
                        buffer[i] = 0;
                    }

                    this.Invoke((MethodInvoker)delegate ()
                    {
                    richTextBox1.AppendText(Clear_Message);
                    });
                }
                catch (Exception ex) { }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox1.Text != " ")
            {
                button2.Enabled = true;
                richTextBox2.Enabled = true;

                Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                if (ip != null)
                {
                    Client.Connect(ip, port);
                    th = new Thread(delegate () { RecvMessage(); });
                    th.Start();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SendMessage("\n" + textBox1.Text + ": " + richTextBox2.Text + ";;;5");
            richTextBox2.Clear();
        }

        private void авторToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Шелепухин Аркадий, 2016");
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(th != null) th.Abort();
            Application.Exit();
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Client
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text != "" && textBox1.Text != " " && textBox2.Text != "" && textBox2.Text != " ")
            {
                try
                {
                    DirectoryInfo data = new DirectoryInfo("Client_Info");
                    data.Create();

                    var sw = new StreamWriter(@"Client_Info/Data_Info.txt");
                    sw.WriteLine(textBox1.Text + ":" + textBox2.Text);
                    sw.Close();

                    this.Hide();
                    Application.Restart();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }
    }
}

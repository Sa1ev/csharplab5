using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab5client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Logic.SendMessage();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Logic.startClient(textBox2.Text, richTextBox1, textBox1);
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

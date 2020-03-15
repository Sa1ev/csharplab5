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

        private void button1_Click(object sender, EventArgs e)
        {
            Logic.SendMessage();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Logic.startClient(nameTextBox.Text, this);
        }
    
       
        public delegate void doAddRichText(String text);

        public void richAddText(String text) {
            if (this.InvokeRequired) {
                this.Invoke(new doAddRichText(richAddText), text);
            }
            else { this.outputRichBox.AppendText(text);
                Console.WriteLine(text);
            }
        }

        private void outputTextBox_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void messageTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        public string getName()
        {
            return nameTextBox.Text;
        }
        public string getMessage()
        {
            return messageTextBox.Text;
        }
        public void clearMessage()
        {
            messageTextBox.Text = "";
        }
        
    }
}

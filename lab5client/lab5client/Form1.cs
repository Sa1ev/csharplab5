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
            
            nameTextBox.Text = "Введите свой ник";
            nameTextBox.ForeColor = Color.Gray;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (messageTextBox.Text != "")
            {
                if (comboBox1.SelectedIndex == 0)
                {
                    outputRichBox.AppendText("Вы отправили: " + messageTextBox.Text + "\n");
                }
                else
                {
                    outputRichBox.AppendText(String.Format("Вы отправили {0} лс: {1}\n", getComboValue(), messageTextBox.Text));
                }
                Logic.SendMessage();
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (nameTextBox.Text != "" & nameTextBox.Text != "Введите свой ник")
            {
                
                Logic.startClient(nameTextBox.Text, this);
            }
            
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

        public void setupWindow()
        {
            comboBox1.Items.Add("Общий");
            comboBox1.SelectedIndex = 0;
            messageTextBox.Visible = true;
            button2.Visible = false;
            outputRichBox.Visible = true;
            button1.Visible = true;
            comboBox1.Visible = true;
            nameTextBox.Enabled=false;
        }

        public void defaultWindow()
        {
            comboBox1.Items.Add("Общий");
            comboBox1.SelectedIndex = 0;
            messageTextBox.Visible = false;
            outputRichBox.Clear();
            button2.Visible = true;
            outputRichBox.Visible = false;
            button1.Visible = false;
            comboBox1.Visible = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public delegate void delFillCombo(String values);

        public void fillCombo(String values)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new delFillCombo(fillCombo), values);
            }
            else
            {
                String[] arr = values.Substring(9).Split(Logic.splitChar);
                comboBox1.Items.Clear();
                comboBox1.Items.Add("Общий");
                comboBox1.SelectedIndex = 0;
                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr[i].Split('\n')[0] != nameTextBox.Text)
                    {
                        comboBox1.Items.Add(arr[i]);
                    }
                    
                }
            }
        }
        

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Logic.Disconnect();
        }

        public String getComboValue()
        {
            String cText = this.comboBox1.GetItemText(this.comboBox1.SelectedItem);
            return cText.Split('\n')[0];
        }

        private void nameTextBox_Enter(object sender, EventArgs e)
        {
            if (nameTextBox.Text == "Введите свой ник")
            {
                nameTextBox.Text = "";
                nameTextBox.ForeColor = Color.Black;
            }
        }

        private void nameTextBox_Leave(object sender, EventArgs e)
        {
            if (nameTextBox.Text == "")
            {
                nameTextBox.Text = "Введите свой ник";
                nameTextBox.ForeColor = Color.Gray;
            }
        }
    }
}

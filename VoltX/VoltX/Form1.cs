using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace VoltX
{
    public partial class Form1 : Form
    {
        public string Username;
        static MySqlConnection connection = new MySqlConnection();
        public Form1()
        {
            InitializeComponent();
        }

        private void flatClose1_Click(object sender, EventArgs e)
        {
            connection.Close();
            connection.Open();
            MySqlCommand command = new MySqlCommand($"DELETE FROM usersinchat WHERE user='{Username}'", connection);
            command.ExecuteNonQuery();
            System.Threading.Thread.Sleep(500);
            Environment.Exit(1);
        }

        private void flatMini1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void checkForMessageTimer_Tick(object sender, EventArgs e)
        {
            checkForMessages();
            checkForUsers();
        }
        void logOut()
        {
            connection.Close();
            connection.Open();
            MySqlCommand command = new MySqlCommand($"DELETE FROM usersinchat WHERE user='{Username}'", connection);
            command.ExecuteNonQuery();
            this.Hide();
            LoginRegister loginRegister = new LoginRegister();
            loginRegister.Show();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            connection = new MySqlConnection("SERVER=server.xgaming.ca; UID=root; PASSWORD=Adamissix12; DATABASE=messages;");
            richTextBox1.ReadOnly = true;
            connection.Close();
            connection.Open();
            MySqlCommand command = new MySqlCommand($"INSERT INTO usersinchat (user) VALUES ('{Username}')", connection);
            try
            {
                command.ExecuteNonQuery();
                richTextBox1.Enabled = true;
                richTextBox1.ReadOnly = true;
            }
            catch (Exception)
            {
                MessageBox.Show("You are logged in from another computer! Can not connect to chat!");
                Environment.Exit(1);
            }
            
        }


        void checkForMessages()
        {
            connection.Close();
            connection.Open();
            richTextBox1.Clear();
            MySqlCommand command = new MySqlCommand("SELECT * FROM messages;", connection);
            MySqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                richTextBox1.Text += dr.GetValue(1).ToString() + "\n";
            }
        }

        void checkForUsers()
        {
            connection.Close();
            connection.Open();
            listBox2.Items.Clear();
            MySqlCommand command = new MySqlCommand("SELECT * FROM usersinchat;", connection);
            MySqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                listBox2.Items.Add(dr.GetValue(0).ToString());
            }
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            Send();
        }
        void Send()
        {
            connection.Close();
            connection.Open();
            string text = flatTextBox1.Text;
            MySqlCommand command = new MySqlCommand($"INSERT INTO messages (message) VALUES ('{Username}: {text}')", connection);
            MySqlCommand commandlog = new MySqlCommand($"INSERT INTO messagelog (message) VALUES ('{Username}: {text}')", connection);
            int sent = command.ExecuteNonQuery();
            int sentToLog = commandlog.ExecuteNonQuery();
            if (sent != 1)
            {
                MessageBox.Show("Message not sent");
            }
            else
            {
                richTextBox1.Text += $"{ Username}: {text}";
                flatTextBox1.Text = "";
                if (richTextBox1.Lines.Length >= 20)
                {
                    MySqlCommand eraseCommand = new MySqlCommand($"DELETE FROM messages", connection);
                    eraseCommand.ExecuteNonQuery();
                }
            }
        }
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        private void Form1_MouseDown_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            connection.Close();
            connection.Open();
            MySqlCommand command = new MySqlCommand($"DELETE FROM usersinchat WHERE user='{Username}'", connection);
            command.ExecuteNonQuery();
            System.Threading.Thread.Sleep(500);
            Environment.Exit(1);
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void listBox2_MouseDown(object sender, MouseEventArgs e)
        {
        }
    }
}

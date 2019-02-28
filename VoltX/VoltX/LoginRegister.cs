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
using System.Security.Cryptography;
namespace VoltX
{
    public partial class LoginRegister : Form
    {
        public MySqlConnection connection = new MySqlConnection("SERVER=server.xgaming.ca; UID=root; PASSWORD=Adamissix12; DATABASE=messages;");
        public LoginRegister()
        {
            InitializeComponent();
        }

        private void flatClose1_Click(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }

        private void flatMini1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void flatButton2_Click(object sender, EventArgs e)
        {
            connection.Close();
            connection.Open();
            string username = flatTextBox1.Text;
            string password = MD5Hash(flatTextBox2.Text);
            if (username.Contains("'") || username.Contains("--"))
            {
                MessageBox.Show("!Warning! The username you have entered contains forbidden characters.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MySqlCommand command = new MySqlCommand($"INSERT INTO accounts (Username, Password) VALUES ('{username}', '{password}')", connection);
                try
                {
                    if (command.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show($"User {username} has been created. You can now login!", "Account Created", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Error! The user {username} has not been created due to an error.", "Account Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Username is already in use!", "Account Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        private void flatButton1_Click(object sender, EventArgs e)
        {
            connection.Close();
            connection.Open();
            string username = flatTextBox1.Text;
            string password = MD5Hash(flatTextBox2.Text);
            if (username.Contains("'") || username.Contains("--"))
            {
                MessageBox.Show("!Warning! The username you have entered contains forbidden characters.");
            }
            else
            {
                MySqlCommand command = new MySqlCommand($"SELECT * FROM accounts WHERE Username='{username}' AND Password='{password}'", connection);
                command.ExecuteNonQuery();
                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(command);
                da.Fill(dt);
                int i;
                i = Convert.ToInt32(dt.Rows.Count.ToString());

                if (i == 0)
                {
                    MessageBox.Show("Invalid username / password!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                else
                {
                    MessageBox.Show($"Welcome, {username}!", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Form1 main = new Form1();
                    main.Username = username;
                    this.Hide();
                    main.Show();
                }
            }
        }

        private void LoginRegister_Load(object sender, EventArgs e)
        {
           // DesktopNotificationManagerCompat.RegisterAumidAndComServer<MyNotificationActivator>("YourCompany.YourApp");
            flatTextBox2.UseSystemPasswordChar = true;


        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void LoginRegister_Load_1(object sender, EventArgs e)
        {
            flatTextBox2.UseSystemPasswordChar = true;
        }

        private void LoginRegister_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}
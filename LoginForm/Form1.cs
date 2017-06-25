using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace LoginForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close(); //closes the window/app
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //connect to the database
            SqlConnection sqlcon = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\emirm\Desktop\C#\GitHub\LoginForm\LoginForm\LoginDB.mdf;Integrated Security=True");
            string query = "SELECT * FROM tbl_Login WHERE username = '" + textBox2.Text.Trim() + "' AND password = '" + textBox1.Text.Trim() + "'";
            SqlDataAdapter sda = new SqlDataAdapter(query, sqlcon);
            DataTable dtbl = new DataTable();
            sda.Fill(dtbl);

            //if there is a match...
            if (dtbl.Rows.Count == 1)
            {
                Form2 objForm2 = new Form2(); //creates the new object
                this.Hide(); //hiding the loginform
                objForm2.Show();  //opens the new window
            }
            else
            {
                MessageBox.Show("You have entered wrong username or password!");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

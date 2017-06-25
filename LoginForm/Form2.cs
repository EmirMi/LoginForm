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
    public partial class Form2 : Form
    {
        SqlConnection sqlCon = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\emirm\Desktop\C#\GitHub\LoginForm\LoginForm\LoginDB.mdf;Integrated Security=True");
        int ContactID = 0;

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            Reset();            //cals reset function
            FillDataGridView(); //show all data at the start
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();
                if (btnSave.Text == "SAVE")
                {
                    SqlCommand sqlCmd = new SqlCommand("ContactInsertOrEdit", sqlCon);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@mode", "Add");
                    sqlCmd.Parameters.AddWithValue("@ContactID", 0);
                    sqlCmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@TelNumber", txtPhone.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Adress", txtAdress.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@City", txtCity.Text.Trim());
                    sqlCmd.ExecuteNonQuery();
                    MessageBox.Show("Saved successfully!");
                }
                else
                {
                    SqlCommand sqlCmd = new SqlCommand("ContactInsertOrEdit", sqlCon);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@mode", "Edit");
                    sqlCmd.Parameters.AddWithValue("@ContactID", ContactID);
                    sqlCmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@TelNumber", txtPhone.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Adress", txtAdress.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@City", txtCity.Text.Trim());
                    sqlCmd.ExecuteNonQuery();
                    MessageBox.Show("Updated successfully!");
                }
                Reset();
                FillDataGridView();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Massage");
            }
            finally
            {
                sqlCon.Close();
            }
        }

        void FillDataGridView()
        {
            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();
            SqlDataAdapter sqlDa = new SqlDataAdapter("ContactViewOrSearch", sqlCon);
            sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDa.SelectCommand.Parameters.AddWithValue("@ContactName", txtSearch.Text.Trim());
            DataTable dtbl = new DataTable();
            sqlDa.Fill(dtbl);
            dataGridView1.DataSource = dtbl;
            dataGridView1.Columns[0].Visible = false;
            sqlCon.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                FillDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Message");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();
                SqlCommand sqlCmd = new SqlCommand("ContactDelete", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@ContactID", ContactID);
                sqlCmd.ExecuteNonQuery();
                MessageBox.Show("Deleted successfully!");
                Reset();
                FillDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Message");
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Index != -1)
            {
                ContactID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                txtName.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                txtPhone.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                txtAdress.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                txtCity.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                btnSave.Text = "UPDATE";
                btnDelete.Enabled = true;
            }
        }

        void Reset()
        {
            //clears all fields, changes button from UPDATE to SAVE
            txtName.Text = txtPhone.Text = txtAdress.Text = txtCity.Text = "";
            btnSave.Text = "SAVE";
            ContactID = 0;
            btnDelete.Enabled = false;
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}

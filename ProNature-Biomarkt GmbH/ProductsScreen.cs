using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProNature_Biomarkt_GmbH
{
    public partial class ProductsScreen : Form
    {
        private SqlConnection databaseConnection = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=C:\Users\Admin\Documents\Pro-Natura Biomarkt GmbH.mdf;Integrated Security = True; Connect Timeout = 30");

        public ProductsScreen()
        {
            InitializeComponent();
            
            ShowProducts();

        }

        public void ShowProducts()
        {
            databaseConnection.Open();

            string query = "select * from Products";
            SqlDataAdapter adapter = new SqlDataAdapter(query, databaseConnection);

            var dataSet = new DataSet();
            adapter.Fill(dataSet);

            dataGridView1.DataSource = dataSet.Tables[0];

            dataGridView1.Columns[0].Visible = false;

            databaseConnection.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (textBoxName.Text == "" 
                || textBoxBrand.Text == ""
                || comboBoxCategorie.Text == ""
                || textBoxPrice.Text == "")
            {
                  MessageBox.Show("Bitte fülle alle Felder aus!");
                  return;
            }


            string productName = textBoxName.Text;
            string productBrand = textBoxBrand.Text;
            string productCategorie = comboBoxCategorie.Text;
            string productPrice = textBoxPrice.Text;

            databaseConnection.Open();
            string query = string.Format("insert into Products values('{0}','{1}','{2}','{3}')", productName, productBrand, productCategorie, productPrice);
            SqlCommand sqlCommand = new SqlCommand(query, databaseConnection);
            sqlCommand.ExecuteNonQuery();
            databaseConnection.Close();

            ClearAllFields();

            ShowProducts();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            ClearAllFields();

            ShowProducts();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearAllFields();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            ShowProducts();
        }

        public void ClearAllFields()
        {
            textBoxName.Text = "";
            textBoxBrand.Text = "";
            textBoxPrice.Text = "";
            comboBoxCategorie.Text = "";
            comboBoxCategorie.SelectedItem = null;
        }


    }
}

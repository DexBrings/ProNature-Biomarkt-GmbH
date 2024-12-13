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
        private int lastSelectedProductKey;

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
                || comboBoxCategory.Text == ""
                || textBoxPrice.Text == "")
            {
                  MessageBox.Show("Bitte fülle alle Felder aus!");
                  return;
            }


            string productName = textBoxName.Text;
            string productBrand = textBoxBrand.Text;
            string productCategory = comboBoxCategory.Text;
            string productPrice = textBoxPrice.Text;

            string query = string.Format("insert into Products values('{0}','{1}','{2}','{3}')", productName, productBrand, productCategory, productPrice);
            ExecuteQuery(query);

            ClearAllFields();

            ShowProducts();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lastSelectedProductKey == 0)
            {
                MessageBox.Show("Bitte wähle zuerst ein Produkt aus.");
                return;
            }

            string productName = textBoxName.Text;
            string productBrand = textBoxBrand.Text;
            string productCategory = comboBoxCategory.Text;
            string productPrice = textBoxPrice.Text;

            string query = string.Format("update Products set Name='{0}', Brand='{1}', Category='{2}', Price='{3}' where Id={4}"
                , productName, productBrand, productCategory, productPrice, lastSelectedProductKey);         
            ExecuteQuery(query);

            ClearAllFields();

            ShowProducts();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearAllFields();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(lastSelectedProductKey == 0)
            {
                MessageBox.Show("Bitte wähle zuerst ein Produkt aus.");
                return;
            }
            string query = string.Format("delete from Products where Id ={0};", lastSelectedProductKey);
            ExecuteQuery(query);

            ClearAllFields();

            ShowProducts();
        }

        public void ClearAllFields()
        {
            textBoxName.Text = "";
            textBoxBrand.Text = "";
            textBoxPrice.Text = "";
            comboBoxCategory.Text = "";
            comboBoxCategory.SelectedItem = null;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            textBoxName.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            textBoxBrand.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            comboBoxCategory.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            textBoxPrice.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();

            lastSelectedProductKey = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
        }

        private void ExecuteQuery(string query)
        {
            databaseConnection.Open();
            SqlCommand sqlCommand = new SqlCommand(query, databaseConnection);
            sqlCommand.ExecuteNonQuery();
            databaseConnection.Close();
        }





    }
}

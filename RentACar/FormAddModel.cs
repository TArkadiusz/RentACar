using MySql.Data.MySqlClient;
using RentACar.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RentACar
{
    public partial class FormAddModel : Form
    {
        public FormAddModel()
        {
            InitializeComponent();
        }

        BindingSource bsBrands = new BindingSource();

        private void FormAddModel_Load(object sender, EventArgs e)
        {
            LoadDictionaryData();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                SaveData();
            }

            RefreshForm();
        }
        
        private void LoadDictionaryData()
        {
            try
            {
                //ładowanie słownika marek
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                String sql = "SELECT id, name FROM car_brands ORDER BY name ASC";
                adapter.SelectCommand = new MySqlCommand(sql, GlobalData.connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                bsBrands.DataSource = dt;
                cbAddModel.DataSource = bsBrands;
                cbAddModel.DisplayMember = "name";
                cbAddModel.ValueMember = "id";
                cbAddModel.SelectedIndex = -1;
                cbAddModel.SelectedIndexChanged += CbAddModel_SelectedIndexChanged;

                tbAddModel.Enabled = false;
            }
            catch (Exception exc)
            {
                DialogHelper.E(exc.Message);
            }
            
        }

        private void CbAddModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbAddModel.SelectedIndex > -1)
            {
                tbAddModel.Enabled = true;
            }
        }

        private bool ValidateData()
        {
            if (tbAddModel.Text != "" )
            {
                return true;
            }
            else
            {
                DialogHelper.E("Wypełnij pole tekstowe");
                return false;
            }
        }

        private void SaveData()
        {
            String sql = "";
            try
            {

                sql = @"INSERT INTO car_models VALUES (NULL, @brand_id, @name)";

                MySqlCommand cmd = new MySqlCommand(sql, GlobalData.connection);
                cmd.Parameters.Add("@name", MySqlDbType.VarChar, 50);
                cmd.Parameters.Add("@brand_id", MySqlDbType.Int32);

                cmd.Parameters["@name"].Value = tbAddModel.Text;
                cmd.Parameters["@brand_id"].Value = cbAddModel.SelectedValue;


                cmd.ExecuteNonQuery();
            }
            catch (Exception exc)
            {
                DialogHelper.E(exc.Message);
            }
        }

        private void RefreshForm()
        {
            tbAddModel.Clear();
        }

        private void cbAddModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

    }
}

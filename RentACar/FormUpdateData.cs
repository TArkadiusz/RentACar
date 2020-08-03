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
    public partial class FormUpdateData : Form
    {
        public FormUpdateData()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                SaveData();
            }

            RefreshForm();
        }

        private bool ValidateData()
        {
            if (tbBrand.Text != null)
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
                
                sql = @"INSERT INTO car_brands VALUES (NULL, @name)";

                MySqlCommand cmd = new MySqlCommand(sql, GlobalData.connection);
                cmd.Parameters.Add("@name", MySqlDbType.VarChar, 50);
                cmd.Parameters["@name"].Value = tbBrand.Text;


                cmd.ExecuteNonQuery();
                

            }
            catch (Exception exc)
            {
                DialogHelper.E(exc.Message);
            }

            
        }

        private void RefreshForm()
        {
            tbBrand.Clear();
        }

        private void btnAddModel_Click(object sender, EventArgs e)
        {
            FormAddModel form = new FormAddModel();
            form.ShowDialog();
        }

        private void FormUpdateData_Load(object sender, EventArgs e)
        {

        }
    }
}

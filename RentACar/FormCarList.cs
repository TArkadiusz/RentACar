﻿using MySql.Data.MySqlClient;
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
    public partial class FormCarList : Form
    {
        public FormCarList()
        {
            InitializeComponent();
        }
        BindingSource bSource = new BindingSource();

        //Wyświetlenie tablicy samochodów na stanie 
        private void FormCarList_Load(object sender, EventArgs e)
        {
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            String sql = @"SELECT 
                        c.id, b.name AS brand, m.name AS model, t.name AS car_type, 
                        c.registration_plate, c.engine, c.manufacturer_year, 
                        c.avail, c.fuel
                        FROM
                        cars AS c, car_models AS m, car_types AS t, car_brands AS b
                        WHERE
                        c.model_id = m.id AND c.type_id = t.id AND m.brand_id = b.id";

            adapter.SelectCommand = new MySqlCommand(sql, GlobalData.connection);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            bSource.DataSource = dt;
            grid.DataSource = bSource;

            grid.Columns["id"].HeaderText = "ID";
            grid.Columns["brand"].HeaderText = "Marka";
            grid.Columns["model"].HeaderText = "Model";
            grid.Columns["car_type"].HeaderText = "Własność";
            grid.Columns["registration_plate"].HeaderText = "Nr rejestracyjny";
            grid.Columns["engine"].HeaderText = "Pojemność [cm3]";
            grid.Columns["engine"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grid.Columns["manufacturer_year"].HeaderText = "Rok produkcji";
            grid.Columns["manufacturer_year"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grid.Columns["avail"].HeaderText = "Dostępny";
            grid.Columns["avail"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.Columns["fuel"].HeaderText = "Paliwo";
            
        }

        private void grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == grid.Columns["avail"].Index)
            {
                e.Value = Convert.ToInt32(e.Value) == 1 ? "TAK" : "NIE";
            }
        }

        private void mnuDeleteCar_Click(object sender, EventArgs e)
        {
            if (grid.SelectedRows.Count == 0) return;
            DialogResult res = MessageBox.Show("Czy na pewno usunąć rekord?", "Pytanie",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res != DialogResult.Yes) return;

            String sql = " DELETE FROM cars WHERE id= @rowId ";
            using (MySqlCommand deleteCommand = new MySqlCommand(sql, GlobalData.connection))
            {
                deleteCommand.Parameters.Add("@rowId", MySqlDbType.Int32);

                int selectedIndex = grid.SelectedRows[0].Index;
                deleteCommand.Parameters["@rowId"].Value = grid["id", selectedIndex].Value;

                deleteCommand.ExecuteNonQuery();

                //update danych w datagridview
                grid.Rows.RemoveAt(selectedIndex);
            }
        }

        //odświeża dane w gridview
        private void RefreshData()
        {
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            String sql = @"SELECT 
                        c.id, b.name AS brand, m.name AS model, t.name AS car_type, 
                        c.registration_plate, c.engine, c.manufacturer_year, 
                        c.avail, c.fuel
                        FROM
                        cars AS c, car_models AS m, car_types AS t, car_brands AS b
                        WHERE
                        c.model_id = m.id AND c.type_id = t.id AND m.brand_id = b.id";

            adapter.SelectCommand = new MySqlCommand(sql, GlobalData.connection);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            bSource.DataSource = dt;
            grid.DataSource = bSource;
        }

        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void mnuRefresh_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void tsbInsert_Click(object sender, EventArgs e)
        {
            AddNewCar();
        }

        private void AddNewCar()
        {
            FormAddCar form = new FormAddCar();
            if (form.ShowDialog() == DialogResult.OK)
            {
                RefreshData();
            }
        }

        //Edycja samochodu
        private void mnuEditCar_Click(object sender, EventArgs e)
        {
            if (grid.SelectedRows.Count == 0) return;
            int selectedIndex = grid.SelectedRows[0].Index;
            int rowId = Convert.ToInt32( grid["id", selectedIndex].Value );

            FormAddCar form = new FormAddCar();
            form.RowId = rowId;
            if (form.ShowDialog() == DialogResult.OK)
            {
                RefreshData();
            }
        }

        //Operacja na samochodzie
        private void mnuCarOper_Click(object sender, EventArgs e)
        {
            if (grid.SelectedRows.Count == 0) return;
            int selectedIndex = grid.SelectedRows[0].Index;

            int rowId = Convert.ToInt32(grid["id", selectedIndex].Value);
            String regPlate = grid["registration_plate", selectedIndex].Value.ToString();
            int avail = Convert.ToInt32(grid["avail", selectedIndex].Value);

            FormOperation form = new FormOperation();
            form.CarId = rowId;
            form.RegPlate = regPlate;
            form.OperBack = (avail == 0);

            if (form.ShowDialog() == DialogResult.OK)
            {
                RefreshData();
            }
        }
    }
}

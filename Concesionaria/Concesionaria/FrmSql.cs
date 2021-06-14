using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Concesionaria.Clases;
namespace Concesionaria
{
    public partial class FrmSql : Form
    {
        public FrmSql()
        {
            InitializeComponent();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {  //18-6-2020
            try
            {
                string sql = txtSql.Text;
                DataTable trdo = cDb.ExecuteDataTable(sql);
                Grilla.DataSource = trdo;
                int Ancho = Grilla.Width - 60;
                int Col = trdo.Columns.Count;
                int AnchoFila = Ancho / Col;
                for (int i = 0; i < trdo.Columns.Count; i++)
                {
                    Grilla.Columns[i].Width = AnchoFila;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error en la consulta");
                MessageBox.Show(ex.Message.ToString());
            }
            
                
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = txtSql.Text;
                cDb.ExecutarNonQuery(sql);
                MessageBox.Show("Datos actualizados");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error en la consulta");
                MessageBox.Show(ex.Message.ToString());
            }
            
        }
    }
}

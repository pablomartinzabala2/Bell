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
    public partial class FrmListadoCompra : Form
    {
        public FrmListadoCompra()
        {
            InitializeComponent();
        }

        private void FrmListadoCompra_Load(object sender, EventArgs e)
        {
            DateTime Fecha = DateTime.Now;
            txtFechaHasta.Text = Fecha.ToShortDateString();
            Fecha = Fecha.AddMonths(-1);
            txtFechaDesde.Text  = Fecha.ToShortDateString();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            cFunciones fun = new cFunciones();
            if (fun.ValidarFecha (txtFechaDesde.Text)==false)
            {
                Mensaje("La fecha desde es incorrecta");
                return;
            }
             
            if (fun.ValidarFecha(txtFechaHasta.Text) == false)
            {
                Mensaje("La fecha Hasta es incorrecta");
                return;
            }
            DateTime FechaDesde = Convert.ToDateTime(txtFechaDesde.Text);
            DateTime FechaHasta = Convert.ToDateTime(txtFechaHasta.Text);
            string Patente = txtPatente.Text.Trim();
            cCompra compra = new cCompra();
            DataTable trdo = compra.getComprasxFecha(FechaDesde, FechaHasta, Patente);
            Grilla.DataSource = trdo;
        }

        private void Mensaje(string msj)
        {
            MessageBox.Show(msj, "Sistema");
        }

        private void btnBuscarCompra_Click(object sender, EventArgs e)
        {
            cFunciones fun = new cFunciones();
            if (fun.ValidarFecha(txtFechaDesde.Text) == false)
            {
                Mensaje("La fecha desde es incorrecta");
                return;
            }

            if (fun.ValidarFecha(txtFechaHasta.Text) == false)
            {
                Mensaje("La fecha Hasta es incorrecta");
                return;
            }
            DateTime FechaDesde = Convert.ToDateTime(txtFechaDesde.Text);
            DateTime FechaHasta = Convert.ToDateTime(txtFechaHasta.Text);
            string Patente = txtPatente.Text.Trim();
            cCompra compra = new cCompra();
            DataTable trdo = compra.getComprasxFecha(FechaDesde, FechaHasta, Patente);
            trdo = fun.TablaaMiles(trdo, "ImporteCompra");
            Grilla.DataSource = trdo;
            string Col = "0;20;20;20;20;20";
            fun.AnchoColumnas(Grilla, Col);
            /*
            Grilla.Columns[0].Visible = false;
            Grilla.Columns[2].Width = 150;
            Grilla.Columns[3].Width = 150;
            Grilla.Columns[5].Width = 250;
            */
            Grilla.Columns[5].HeaderText = "Importe Compra";
        }

        private void btnAbrirCompra_Click(object sender, EventArgs e)
        {
            if (Grilla.CurrentRow ==null)
            {
                Mensaje("Debe seleccionar un registro");
                return;
            }
            string CodCompra = Grilla.CurrentRow.Cells[0].Value.ToString();
            Principal.CodCompra = CodCompra;
            FrmAutos frm = new Concesionaria.FrmAutos();
            frm.ShowDialog();
        }
    }
}

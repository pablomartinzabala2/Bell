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
    public partial class FrmListadoAvisos : Form
    {
        private DataTable tbLista;
        public FrmListadoAvisos()
        {
            InitializeComponent();
        }

        private void FrmListadoAvisos_Load(object sender, EventArgs e)
        {
            cCliente cli = new cCliente();
            cli.ActuaizarCumpleanios();
            cFunciones fun = new cFunciones();
            string Col = "Fecha;Apellido;Nombre;Telefono;Texto";
            tbLista = fun.CrearTabla(Col);
            DateTime Hoy = DateTime.Now;
            DateTime Ant = Hoy.AddDays(-3);
            DateTime Fut = Hoy.AddDays(3);
            txtFechaDesde.Text = Ant.ToShortDateString();
            txtFechaHasta.Text = Fut.ToShortDateString();
            GetInfo(Ant,Fut);
            GetInfoPrenda(Ant, Fut);
        }

        private void Mensaje(string msj)
        {
            MessageBox.Show(msj, "Sistema");
        }

       private void GetInfo(DateTime FechaDesde,DateTime FechaHasta)
        {
            cCliente cli = new Clases.cCliente();
            String Fecha = "";  
            string Apellido = "";
            string Nombre = "";
            string Telefono = "";
            string Texto = "";
            string Val = "";
            cFunciones fun = new cFunciones();
            DataTable tbCli = cli.GetCumpleanios(FechaDesde, FechaHasta);
            for (int i=0;i< tbCli.Rows.Count;i++)
            {
                Fecha = tbCli.Rows[i]["FechaCumple"].ToString();
                Apellido = tbCli.Rows[i]["Apellido"].ToString();
                Nombre = tbCli.Rows[i]["Nombre"].ToString();
                Telefono = tbCli.Rows[i]["Telefono"].ToString();
                Texto = "Cumpleaños";
                Val = Fecha + ";" + Apellido;
                Val = Val + ";" + Nombre + ";" + Telefono;
                Val = Val + ";" + Texto;
                tbLista = fun.AgregarFilas(tbLista, Val);
            }
            /*
            Grilla.DataSource = tbLista;
            Grilla.Columns[1].Width = 180;
            Grilla.Columns[2].Width = 180;
            Grilla.Columns[3].Width = 150;
            Grilla.Columns[4].Width = 180;
            Grilla.Columns[4].HeaderText = "Información";
            */
        }

        private void GetInfoPrenda(DateTime FechaDesde, DateTime FechaHasta)
        {
            cPrenda pre = new Clases.cPrenda();
            String Fecha = "";
            string Apellido = "";
            string Nombre = "";
            string Telefono = "";
            string Texto = "";
            string Val = "";
            cFunciones fun = new cFunciones();
            DataTable tbCli = pre.GetPrendasFinalizadas(FechaDesde, FechaHasta);
            for (int i = 0; i < tbCli.Rows.Count; i++)
            {
                Fecha = tbCli.Rows[i]["FechaVencimiento"].ToString();
                Apellido = tbCli.Rows[i]["Apellido"].ToString();
                Nombre = tbCli.Rows[i]["Nombre"].ToString();
                Telefono = tbCli.Rows[i]["Telefono"].ToString();
                Texto = "Vencimiento prenda";
                Val = Fecha + ";" + Apellido;
                Val = Val + ";" + Nombre + ";" + Telefono;
                Val = Val + ";" + Texto;
                tbLista = fun.AgregarFilas(tbLista, Val);
            }
            Grilla.DataSource = tbLista;
            Grilla.Columns[1].Width = 180;
            Grilla.Columns[2].Width = 180;
            Grilla.Columns[3].Width = 150;
            Grilla.Columns[4].Width = 180;
            Grilla.Columns[4].HeaderText = "Información";
        }
    }
}

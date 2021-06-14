using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Concesionaria.Clases;
using System.Data.SqlClient;
namespace Concesionaria
{
    public partial class FrmDetalleAuto : Form
    {
        DataTable tbListaPapeles;
        public FrmDetalleAuto()
        {
            InitializeComponent();
            
            if (Principal.CodigoPrincipalAbm != "")
            {
                txtCodStock.Text = Principal.CodigoPrincipalAbm.ToString();
                CargarAuto(Convert.ToInt32(Principal.CodigoPrincipalAbm));
                CargarCostoxstock(Convert.ToInt32(Principal.CodigoPrincipalAbm));
                CargarGastosGeneralesxCodStoxk(Convert.ToInt32(Principal.CodigoPrincipalAbm));
                CargarCheques(Convert.ToInt32(Principal.CodigoPrincipalAbm));
                GetTelefonoCliente(Convert.ToInt32(Principal.CodigoPrincipalAbm));
                // GetEfectivoPagar(Convert.ToInt32(Principal.CodigoPrincipalAbm));
                CargarPapeles();
                if (txtCodCompra.Text !="")
                {
                    Int32 CodCompra = Convert.ToInt32(txtCodCompra.Text);
                    BuscarPapeles(CodCompra);
                }
            }
        }

        private void CargarAuto(Int32 CodStock)
        {
            Clases.cStockAuto stock = new Clases.cStockAuto();
            DataTable trdoAuto = stock.GetStockxCodigo(CodStock);
            if (trdoAuto.Rows.Count > 0)
            {
                if (trdoAuto.Rows[0]["FechaAlta"].ToString()!="")
                {
                    DateTime FechaIngreso = Convert.ToDateTime(trdoAuto.Rows[0]["FechaAlta"].ToString());
                    txtFechaIngreso.Text = FechaIngreso.ToShortDateString();
                }
                txtPatente.Text = trdoAuto.Rows[0]["Patente"].ToString();
                txtDescripcion.Text = trdoAuto.Rows[0]["Descripcion"].ToString();
                txtkms.Text = trdoAuto.Rows[0]["Kilometros"].ToString();
                txtanio.Text = trdoAuto.Rows[0]["Anio"].ToString();
                txtCiudad.Text = trdoAuto.Rows[0]["Motor"].ToString();
                txtChasis.Text = trdoAuto.Rows[0]["Chasis"].ToString();
                txtMotor.Text = trdoAuto.Rows[0]["Motor"].ToString();
                txtCiudad.Text = trdoAuto.Rows[0]["Ciudad"].ToString();
                txtImporte.Text = trdoAuto.Rows[0]["ImporteCompra"].ToString();
                txtPrecioVenta.Text = trdoAuto.Rows[0]["PrecioVenta"].ToString();
                txtCodCompra.Text = trdoAuto.Rows[0]["CodCompra"].ToString();
                if (txtImporte.Text != "")
                {
                    txtImporte.Text = txtImporte.Text.Replace(",", ".");
                    string[] vec = txtImporte.Text.Split('.');
                    Clases.cFunciones fun = new Clases.cFunciones();
                    txtImporte.Text = fun.FormatoEnteroMiles(vec[0]);
                }

                if (txtPrecioVenta.Text != "")
                {
                    txtPrecioVenta.Text = txtPrecioVenta.Text.Replace(",", ".");
                    string[] vec = txtPrecioVenta.Text.Split('.');
                    Clases.cFunciones fun = new Clases.cFunciones();
                    txtPrecioVenta.Text = fun.FormatoEnteroMiles(vec[0]);
                }
                txtExTitular.Text = trdoAuto.Rows[0]["ApeNom"].ToString();
                txtAutoPartePago.Text = trdoAuto.Rows[0]["DescripcionAutoPartePago"].ToString();
            }

        }

        public void CargarCostoxstock(Int32 CodStock)
        {
            Clases.cFunciones fun = new Clases.cFunciones();
            Clases.cCosto costo = new Clases.cCosto();
            DataTable trdo = costo.GetCostoxCodigoStock(CodStock);
            //agrego el boton
            Grilla.DataSource = fun.TablaaMiles(trdo, "Importe");
            // Grilla.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 14);

            // Grilla.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 12);
            Grilla.Columns[0].Visible = false;
            Grilla.Columns[1].Visible = false;
            Grilla.Columns[2].Width = 420;
            Grilla.Columns[3].Width = 150;
            Grilla.Columns[4].Width = 150;
            Grilla.Columns[3].Width = 100;
            Grilla.Columns[2].HeaderText = "Descripción"; 
            CalcularTotalGeneral();
            Grilla.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight;
            
            // Estilo();

        }

        public void CalcularTotalGeneral()
        {
            int i = 0;
            double Total = 0;
            for (i = 0; i < Grilla.Rows.Count - 1; i++)
            {
                if (Grilla.Rows[i].Cells[4].Value.ToString () != "")
                {
                    Total = Total + Convert.ToDouble(Grilla.Rows[i].Cells[4].Value);
                }
            }
            txtTotal.Text = Total.ToString();
            if (txtTotal.Text != "")
            {
                txtTotal.Text = txtTotal.Text.Replace(",", ".");
                string[] vec = txtTotal.Text.Split('.');
                Clases.cFunciones fun = new Clases.cFunciones();
                txtTotal.Text = fun.FormatoEnteroMiles(vec[0]);
            }
        }

        private void CargarGastosGeneralesxCodStoxk(Int32 CodStock)
        {
            Clases.cFunciones fun = new Clases.cFunciones();
            Clases.cGastosPagar gastos = new Clases.cGastosPagar();
            DataTable trdo = gastos.GetGastosPagarxCodStock(CodStock);
            trdo = fun.TablaaMiles(trdo, "Importe");
            
            GrillaGastosRecepcion.DataSource = trdo;
            GrillaGastosRecepcion.Columns[0].Width = 250;
            GrillaGastosRecepcion.Columns[3].Width = 120;
            GrillaGastosRecepcion.Columns[3].HeaderText = "Fecha Pago"; 
        }

        private void FrmDetalleAuto_Load(object sender, EventArgs e)
        {

        }

        private void CargarCheques(Int32 CodStock)
        {
            Clases.cFunciones fun = new Clases.cFunciones();
            Clases.cCompra compra = new Clases.cCompra();
            Int32 CodCompra = compra.GetCodCompraxCodStock(CodStock);
            Clases.cChequesaPagar cheque = new Clases.cChequesaPagar();
            DataTable trdo = cheque.GetChequesxCodCompra(CodCompra);
            trdo = fun.TablaaMiles(trdo, "Importe");
            GrillaCheques.DataSource  = trdo;
            GrillaCheques.Columns[3].HeaderText = "Fecha Pago";
            GrillaCheques.Columns[3].Width = 100;
            GrillaCheques.Columns[4].Width = 270;
            GrillaCheques.Columns[5].Visible = false;
            GrillaCheques.Columns[6].Visible = false;
            DataTable tComp = compra.GetCompraxCodigo(CodCompra);
            GetEfectivoPagar(CodCompra);
            if (tComp.Rows.Count > 0)
            {
                if (tComp.Rows[0]["ImporteEfectivo"].ToString() != "")
                {
                    txtEfectivo.Text = tComp.Rows[0]["ImporteEfectivo"].ToString();
                    txtEfectivo.Text = fun.SepararDecimales(txtEfectivo.Text);
                    txtEfectivo.Text = fun.FormatoEnteroMiles(txtEfectivo.Text);
                }

                if (tComp.Rows[0]["ImporteAutoPartePago"].ToString() != "")
                {
                    txtImporteAutoPartePago.Text = tComp.Rows[0]["ImporteAutoPartePago"].ToString();
                    txtImporteAutoPartePago.Text = fun.SepararDecimales(txtImporteAutoPartePago.Text);
                    txtImporteAutoPartePago.Text = fun.FormatoEnteroMiles(txtImporteAutoPartePago.Text);
                }

                if (tComp.Rows[0]["CodStockSalida"].ToString() != "")
                {
                    Clases.cStockAuto stock = new Clases.cStockAuto();
                    DataTable tauto = stock.GetStockxCodigo(Convert.ToInt32(tComp.Rows[0]["CodStockSalida"].ToString()));
                    if (tauto.Rows.Count > 0)
                    {
                        txtPatente2.Text = tauto.Rows[0]["Patente"].ToString();
                        txtDescripcion2.Text = tauto.Rows[0]["Descripcion"].ToString();
                    }
                }
                //GetStockxCodigo
            }
        }

        private void txtPrecioVenta_KeyPress(object sender, KeyPressEventArgs e)
        {
            Clases.cFunciones fun = new Clases.cFunciones();
            fun.SoloEnteroConPunto(sender, e);
        }

        private void txtPrecioVenta_Leave(object sender, EventArgs e)
        {
            Clases.cFunciones fun = new Clases.cFunciones();
           txtPrecioVenta.Text= fun.FormatoEnteroMiles(txtPrecioVenta.Text);
        }

        private void btnGrabarPrecio_Click(object sender, EventArgs e)
        {
            if (txtPrecioVenta.Text == "")
            {
                MessageBox.Show("Debe ingresar un precio para continuar", Clases.cMensaje.Mensaje());
                return;
            }
            Clases.cFunciones fun = new Clases.cFunciones();
            double Importe = fun.ToDouble(txtPrecioVenta.Text);
            Int32 CodStock = Convert.ToInt32(Principal.CodigoPrincipalAbm);
            Clases.cStockAuto stock = new Clases.cStockAuto();
            stock.ActualizarPrecioVenta(CodStock, Importe);
            MessageBox.Show("Datos grabados correctamente", Clases.cMensaje.Mensaje());
        }

        private void GetEfectivoPagar(Int32 CodCompra)
        {
            Clases.cFunciones fun = new Clases.cFunciones();
            Clases.cEfectivoaPagar eft = new Clases.cEfectivoaPagar();
            DataTable trdo = eft.GetEfectivoPagarxCodCompra(CodCompra);
            if (trdo.Rows.Count > 0)
            {
                if (trdo.Rows[0]["Importe"].ToString() != "")
                {
                    txtEfectivoPagar.Text = trdo.Rows[0]["Importe"].ToString();
                    txtEfectivoPagar.Text = fun.SepararDecimales(txtEfectivoPagar.Text);
                    txtEfectivoPagar.Text = fun.FormatoEnteroMiles(txtEfectivoPagar.Text);
                }
            }
        }

        private void GetTelefonoCliente(Int32 CodStock)
        {
            Clases.cStockAuto stock = new Clases.cStockAuto();
            DataTable trdo = stock.GetStockxCodigo(CodStock);
            if (trdo.Rows.Count > 0)
            {
                if (trdo.Rows[0]["CodCliente"].ToString() != "")
                {
                    Int32 CodCliente = Convert.ToInt32(trdo.Rows[0]["CodCliente"].ToString());
                    Clases.cCliente cli = new Clases.cCliente();
                    DataTable tbCliente = cli.GetClientesxCodigo(CodCliente);
                    if (tbCliente.Rows.Count > 0)
                    {
                        string telefono = tbCliente.Rows[0]["Telefono"].ToString();
                        string NroDoc = tbCliente.Rows[0]["NroDocumento"].ToString();
                        txtTelefono.Text = telefono;
                        string Nombre = tbCliente.Rows[0]["Nombre"].ToString();
                        string Apellido = tbCliente.Rows[0]["Apellido"].ToString();
                        string NomApe = Nombre + " " + Apellido;
                        txtCliente.Text = NomApe;
                        string Celular = tbCliente.Rows[0]["Celular"].ToString();
                        txtCelular.Text = Celular;
                        txtNroDoc.Text = NroDoc;
                    }
                }
            }
        }

        private void BtnAgregarCheque_Click(object sender, EventArgs e)
        {
            if (txtPrecioVenta.Text == "")
            {
                MessageBox.Show("Debe ingresar un precio para continuar", Clases.cMensaje.Mensaje());
                return;
            }
            Clases.cFunciones fun = new Clases.cFunciones();
            double Importe = fun.ToDouble(txtPrecioVenta.Text);
            Int32 CodStock = Convert.ToInt32(Principal.CodigoPrincipalAbm);
            Clases.cStockAuto stock = new Clases.cStockAuto();
            stock.ActualizarPrecioVenta(CodStock, Importe);
            MessageBox.Show("Datos grabados correctamente", Clases.cMensaje.Mensaje());
        }

        private void CargarPapeles()
        {
            tbListaPapeles = new DataTable();
            cPapeles papel = new cPapeles();
            DataTable tbPapeles = papel.GetPapeles();
            Lista.DataSource = tbPapeles;
            Lista.DisplayMember = "Nombre";
            Lista.ValueMember = "CodPapel";
            txtFechaEntregaPapel.Text = DateTime.Now.ToShortDateString();
            tbListaPapeles = new DataTable();
            tbListaPapeles.Columns.Add("CodPapel");
            tbListaPapeles.Columns.Add("Nombre");
            tbListaPapeles.Columns.Add("Entrego");
            tbListaPapeles.Columns.Add("Texto");
            tbListaPapeles.Columns.Add("Fecha");
            tbListaPapeles.Columns.Add("FechaVencimiento");
        }

        private void BuscarPapeles(Int32 CodCompra)
        {
            cPapeles papel = new Clases.cPapeles();
            DataTable trdo = papel.GetPapelesxCodCompra(CodCompra);
            if (trdo.Rows.Count >0)
            {
                string CodPapel = "";
                string Nombre = "";
                string Texto = "";
                string Entrego = "";
                string Fecha = "";
                string FechaVencimiento = "";
                string Val = "";
                cFunciones fun = new cFunciones();
                for (int i=0;i<trdo.Rows.Count;i++)
                { 
                    CodPapel = trdo.Rows[i]["CodPapel"].ToString();
                    Nombre = trdo.Rows[i]["Nombre"].ToString();
                    Texto = trdo.Rows[i]["Texto"].ToString();
                    Entrego = trdo.Rows[i]["Entrego"].ToString();
                    Fecha = trdo.Rows[i]["Fecha"].ToString();
                    FechaVencimiento = trdo.Rows[i]["FechaVencimiento"].ToString();
                    Val = CodPapel + ";" + Nombre;
                    Val = Val + ";" + Texto + ";" + Entrego;
                    Val = Val + ";" + Fecha + ";" + FechaVencimiento;
                    tbListaPapeles = fun.AgregarFilas(tbListaPapeles, Val);
                }
                GrillaPapeles.DataSource = tbListaPapeles;
                GrillaPapeles.Columns[0].Visible = false;
            }
        }

        private void btnAgregarPapel_Click(object sender, EventArgs e)
        {
            cFunciones fun = new cFunciones();
            string CodPapel = Lista.SelectedValue.ToString();
            string Nombre = Lista.Text;
            string Entrego = "0";
            string Fecha = "";
            string Texto = "No";
            string FechaVencimiento = "";
            if (chkEntrego.Checked == true)
            {
                if (fun.ValidarFecha(txtFechaEntregaPapel.Text) == false)
                {
                    Mensaje("La fecha de entrega del documento es incorrecta");
                }
                Entrego = "1";
                Texto = "Si";
                Fecha = txtFechaEntregaPapel.Text;
            }
            string xx = txtFechaVtoPapel.Text;
            if (fun.ValidarFecha(txtFechaVtoPapel.Text) == true)
            {
                FechaVencimiento = txtFechaVtoPapel.Text;
            }

            if (fun.Buscar(tbListaPapeles, "CodPapel", CodPapel) == true)
            {
                Mensaje("Ya se ha ingresado el documento");
                return;
            }

            string Valor = CodPapel + ";" + Nombre;
            Valor = Valor + ";" + Entrego;
            Valor = Valor + ";" + Texto;
            Valor = Valor + ";" + Fecha;
            Valor = Valor + ";" + FechaVencimiento;
            tbListaPapeles = fun.AgregarFilas(tbListaPapeles, Valor);
            GrillaPapeles.DataSource = tbListaPapeles;
            GrillaPapeles.Columns[0].Visible = false;
            GrillaPapeles.Columns[2].Visible = false;
            GrillaPapeles.Columns[1].Width = 130;
            GrillaPapeles.Columns[3].Width = 80;
            GrillaPapeles.Columns[4].Width = 80;
            GrillaPapeles.Columns[5].Width = 90;
            GrillaPapeles.Columns[5].HeaderText = "Vencimiento";
            GrillaPapeles.Columns[3].HeaderText = "Entrego";
        }

        public void Mensaje(string msj)
        {
            MessageBox.Show(msj, "Sistema");
        }

        private void btnQuitarPapel_Click(object sender, EventArgs e)
        {
            if (GrillaPapeles.CurrentRow == null)
            {
                Mensaje("Debe seleccionar un registro");
                return;
            }
            string CodPapel = GrillaPapeles.CurrentRow.Cells[0].Value.ToString();
            cFunciones fun = new cFunciones();
            tbListaPapeles = fun.EliminarFila(tbListaPapeles, "CodPapel", CodPapel);
            GrillaPapeles.DataSource = tbListaPapeles;
        }

        private void BtnGraparPapel_Click(object sender, EventArgs e)
        {
            cPapeles objPapel = new cPapeles();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Clases.cConexion.Cadenacon();
            con.Open();
            SqlTransaction Transaccion;  
            Int32 CodCompra = Convert.ToInt32(txtCodCompra.Text);
            Int32 CodStock = Convert.ToInt32(txtCodStock.Text); 
            Transaccion = con.BeginTransaction();
            try
            {
                objPapel.BorrarPapeles(con, Transaccion, CodCompra);
                GrabarPapelesxStock(con, Transaccion, CodCompra, CodStock);
                Transaccion.Commit();
                con.Close();
                MessageBox.Show("Datos grabados correctamente", Clases.cMensaje.Mensaje());

            }
            catch (Exception ex)
            {
                string msj = "Hubo un error en el proceso " + ex.Message.ToString();
                MessageBox.Show(msj, Clases.cMensaje.Mensaje());
               
                Transaccion.Rollback();
                con.Close();
            }
        }

        private void GrabarPapelesxStock(SqlConnection con, SqlTransaction Transaccion, Int32 CodCompra, Int32 CodStock)
        {
            int i = 0;
            Int32 CodPapel = 0;
            string Entrego = "";
            string Texto = "";
            DateTime? Fecha = null;
            DateTime? FechaVencimiento = null;
            cFunciones fun = new cFunciones();
            cPapeles papel = new cPapeles();
            for (i = 0; i < tbListaPapeles.Rows.Count; i++)
            {
                CodPapel = Convert.ToInt32(tbListaPapeles.Rows[i]["CodPapel"]);
                Entrego = tbListaPapeles.Rows[i]["Entrego"].ToString();
                Texto = tbListaPapeles.Rows[i]["Texto"].ToString();
                if (fun.ValidarFecha(tbListaPapeles.Rows[i]["Fecha"].ToString()) == true)
                {
                    Fecha = Convert.ToDateTime(tbListaPapeles.Rows[i]["Fecha"].ToString());
                }

                if (fun.ValidarFecha(tbListaPapeles.Rows[i]["FechaVencimiento"].ToString()) == true)
                {
                    FechaVencimiento = Convert.ToDateTime(tbListaPapeles.Rows[i]["FechaVencimiento"].ToString());
                }
                papel.InsertarPapeles(con, Transaccion, CodPapel, CodStock, Entrego, Texto, Fecha, FechaVencimiento, CodCompra);
            }

        }
    }
}

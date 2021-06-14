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
    public partial class FrmVistaPrevia : Form
    {
        public FrmVistaPrevia()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void FrmVistaPrevia_Load(object sender, EventArgs e)
        {
            if (Principal.CodigoPrincipalAbm != null)
            {
                CargarDatos(Convert.ToInt32(Principal.CodigoPrincipalAbm));
                BuscarGastosTransferencia(Convert.ToInt32(Principal.CodigoPrincipalAbm));
               
            }
        }

        private void CargarDatos(Int32 CodVenta)
        {
            string ExTitular = "";
            string Comprador = "";
            GetAutoPartePago(CodVenta);
            Clases.cCliente cliente = new Clases.cCliente();
            Clases.cVenta venta = new Clases.cVenta();
            DataTable trdo = venta.GetVentaxCodigo(CodVenta);
            
            if (trdo.Rows.Count > 0)
            {
                DateTime Fecha = Convert.ToDateTime(trdo.Rows[0]["Fecha"].ToString());
                txtFecha.Text = Fecha.ToShortDateString();
                Int32 CodCliente = Convert.ToInt32(trdo.Rows[0]["CodCliente"].ToString());
                Comprador = GetDatosClientexCod(CodCliente);
                txtComprador.Text = Comprador;
                Int32 CodStock = Convert.ToInt32(trdo.Rows[0]["CodStock"].ToString());
                ExTitular = GetExTitular(CodStock);
                txtExTitular.Text = ExTitular;
                DataTable tcli = cliente.GetClientesxCodigo(CodCliente);
                if (tcli.Rows.Count > 0)
                {
                    string nombre = tcli.Rows[0]["Nombre"].ToString();
                    nombre = nombre + " " + tcli.Rows[0]["Apellido"].ToString();
                    string Direccion = tcli.Rows[0]["Calle"].ToString();
                    Direccion = Direccion + " " + tcli.Rows[0]["Numero"].ToString();
                    txtDireccion.Text = Direccion;
                    txtTelefono.Text = tcli.Rows[0]["Telefono"].ToString();
                    txtNombre.Text = nombre;
                    txtDni.Text = tcli.Rows[0]["NroDocumento"].ToString();
                }
            }
            txtEfectivo.Text = trdo.Rows[0]["ImporteEfectivo"].ToString();
            txtDocumentos.Text = trdo.Rows[0]["ImporteCredito"].ToString(); 
            Int32 CodAuto = Convert.ToInt32(trdo.Rows[0]["CodAutoVendido"].ToString());
            Clases.cAuto auto = new Clases.cAuto();
            DataTable tauto = auto.GetAutoxCodigo(CodAuto);
            {
                if (tauto.Rows.Count > 0)
                {
                    string Descrip ="Marca "+ tauto.Rows[0]["Marca"].ToString();
                    Descrip = Descrip + " con Dominio " + tauto.Rows[0]["Patente"].ToString();
                   // Descrip = Descrip + " AÑO " + tauto.Rows[0]["Anio"].ToString();
                  //  Descrip = Descrip + " DOMINIO " + tauto.Rows[0]["Patente"].ToString();
                  //  Descrip = Descrip + " MOTOR N º" + tauto.Rows[0]["Motor"].ToString();
                  //  Descrip = Descrip + " CHASIS N º" + tauto.Rows[0]["Chasis"].ToString();
                    txtAuto.Text = Descrip; 
                }
            }
            Clases.cFunciones fun = new Clases.cFunciones();
            if (txtEfectivo.Text != "0" && txtEfectivo.Text != "")
            {
                txtEfectivo.Text = fun.SepararDecimales(txtEfectivo.Text);
                txtEfectivo.Text = fun.FormatoEnteroMiles(txtEfectivo.Text);
            }

            if (txtDocumentos.Text != "0" && txtDocumentos.Text != "")
            {
                txtDocumentos.Text = fun.SepararDecimales(txtDocumentos.Text);
                txtDocumentos.Text = fun.FormatoEnteroMiles(txtDocumentos.Text);
                cCuota cuo = new cCuota();
                string tex = cuo.GetTextoCuota(Convert.ToInt32(Principal.CodigoPrincipalAbm));
            }
            Clases.cPrenda prenda = new Clases.cPrenda();
            DataTable trdoPrenda = prenda.GetPrendaxCodVenta(Convert.ToInt32(Principal.CodigoPrincipalAbm));
            if (trdoPrenda.Rows.Count > 0)
            {
                string Importe = trdoPrenda.Rows[0]["Importe"].ToString();
                Importe = fun.SepararDecimales(Importe);
                Importe = fun.FormatoEnteroMiles(Importe);
                txtImportePrenda.Text = Importe;
            }
        }

        private void GrabarDatos()
        {
            string NombreCliente = txtNombre.Text;
            string DniCliente = txtDni.Text;
            string DireccionCliente = txtDireccion.Text;
            string Texto1 = "Entre JOSELO AUTOMOTORES, por cuenta y orden de exTitular ";
            Texto1 = Texto1 + ", adelante la parte  VENDEDORA Y La Sr/a Comprador";
            Texto1 = Texto1 + ", en adelante la parte COMPRADORA, todos mayores de edad y hábiles para contratar, convienen en celebrar el presente contrato";
            Texto1 = Texto1 + " de compraventa de automotor sujeto a las  Cláusulas y concidiones adjuntas";
            Texto1 = Texto1.Replace("Comprador", txtComprador.Text);
            Texto1 = Texto1.Replace("exTitular", txtExTitular.Text);

            string texto2 = "El VENDEDOR vende al COMPRADOR y este adquiere el automotor NombreAuto";
            texto2 = texto2 + " EL vendedor entrega en este acto el vehículo al ";
            texto2 = texto2 + "comprador en el estado que se encuentra que el mismo fue revisado y probado por este último, prestando el ";
            texto2 = texto2 + "comprador su integra conformidad sobre el mismo, por lo cual acuerdan que el VENDEDOR no tendrá ";
            texto2 = texto2 + "ninguna responsabilidad de resarcimiento por vicios ocultos.";
            texto2 = texto2 + "El precio total de la unidad mencionada es de  PESOS " + txtPrecio.Text;
            texto2 = texto2 + txtFormaPago.Text;
            texto2 = texto2.Replace("NombreAuto", txtAuto.Text);

            string texto3 = "El comprador recibe del vendedor el automotor objeto de la presente compraventa, de total conformidad ";
            texto3 = texto3 + "Xº juntamente con la documentación del mismo, declarando el segundo que dicho rodado se encuentra  "; //aca viene el texto de forma pago
            texto3 = texto3 + "libre de toda prenda o gravamen, como exento de medidas cautelares o similares  de carácter judicial.";
            string Texto4 = "El comprador se obliga a realizar todos los tramites  de transferencia del dominio a su nombre ";
            Texto4 = Texto4 + "dentro de los diez (10) días de la firma del presente, mediante las solicitudes del artículo 13 y del artículo ";
            Texto4 = Texto4 + "14, en los términos del 15 y bajo el apercibimiento que prescribe el artículo 27 del Decreto -Ley 6582/58.";



            string texto6 = "Para todos los efectos judiciales y extrajudiciales derivados del presente contrato, el vendedor constituye ";
            texto6 = texto6 + "domicilio en la calle Av. Alvear 474 de la ciudad de Resistencia provincia del Chaco, y el comprador en ";
            texto6 = texto6 + "RUTA 5 KM 28 ALTA GRACIA donde se consideraran validos todas las notificaciones y emplazamientos ";
            texto6 = texto6 + "judiciales o extrajudiciales que se hagan.";

            string Texto7 = "Asimismo ambas partes se someten a la competencia de los Tribunales de la Justicia Ordinaria de ";
            Texto7 = Texto7 + "Resistencia Chaco, con renuncia a todo otro fuero o jurisdicción.";
            Texto7 = Texto7 + "Se firman dos ejemplares de un mismo tenor y a solo efecto y cada parte recibe el suyo en este acto. En ";
            Texto7 = Texto7 + "la ciudad de Resistencia Provincia del Chaco Fecha";
            Texto7 = Texto7.Replace("Fecha", GetTextoFecha());
            string Texto8 = "A partir del día de la fecha, el comprador acepta en forma exclusiva, la responsabilidad civil y ";
            Texto8 = Texto8 + "criminalmente de todas las consecuencias derivadas del uso, posesión y tenencia del vehículo, haciéndose ";
            Texto8 = Texto8 + "además cargo de los gravámenes que pudiesen pesar sobre el mismo a partir de este momento en adelante.";

            string sql = "delete from reporte";
            Clases.cDb.ExecutarNonQuery(sql);
            sql = "Insert into reporte(Parte1,Parte2,Parte3,Parte4,Parte6,Parte7,Parte8) values (" + "'" + Texto1 + "'";
            sql = sql + "," + "'" + texto2 +"'";
            sql = sql + "," + "'" + texto3 + "'";
            sql = sql + "," + "'" + Texto4 + "'";
            sql = sql + "," + "'" + texto6 + "'";
            sql = sql + "," + "'" + Texto7  + "'";
            sql = sql + "," + "'" + Texto8 + "'";
            sql = sql + ")";
            Clases.cDb.ExecutarNonQuery(sql);
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            GrabarDatos();

            FrmReporte form = new FrmReporte();
            form.Show();
        }

        private string GetTextoFecha()
        {
            DateTime Fecha = Convert.ToDateTime(txtFecha.Text);
            string NombreMes = "";
            int dia = Fecha.Day;
            int Mes = Fecha.Month;
            int anio = Fecha.Year;
            switch(Mes)
            {
                case 1:
                    NombreMes = "Enero";
                    break;
                case 2:
                    NombreMes = "Febrero";
                    break;
                case 3:
                    NombreMes = "Marzo";
                    break;
                case 4:
                    NombreMes = "Abril";
                    break;
                case 5:
                    NombreMes = "Mayo";
                    break;
                case 6:
                    NombreMes = "Junio";
                    break;
                case 7:
                    NombreMes = "Julio";
                    break;
                case 8:
                    NombreMes = "Agosto";
                    break;
                case 9:
                    NombreMes = "Septiembre";
                    break;
                case 10:
                    NombreMes = "Octubre";
                    break;
                case 11:
                    NombreMes = "Noviembre";
                    break;
                case 12:
                    NombreMes = "Diciembre";
                    break;
            }
            string texto = dia.ToString() + " de " + NombreMes;
            texto = texto + " de " + anio.ToString();
            return texto;
        }

        private void BuscarGastosTransferencia(Int32 CodVenta)
        {
            Clases.cGastoTransferencia gasto = new Clases.cGastoTransferencia();
            DataTable trdo = gasto.GetGastoTransferenciaxCodVenta(CodVenta);
            if (trdo.Rows.Count > 0)
            {
                for (int i = 0; i < trdo.Rows.Count; i++)
                {
                    string Codigo = trdo.Rows[i]["CodGastoTranasferencia"].ToString();
                    string Descripcion = trdo.Rows[i]["Descripcion"].ToString();
                    string Importe = trdo.Rows[i]["Importe"].ToString();
                    AgregarGasto(Codigo, Descripcion, Importe, "Transferencia");
                }
            }
            //AgregarGasto(CmbGastosTransferencia.SelectedValue.ToString(), Descripcion, txtImporteGastoTransferencia.Text, "Transferencia");
        }

        private void AgregarGasto(string Codigo, string Descripcion, string Importe, string Tipo)
        {
            for (int i = 0; i < GrillaGastos.Rows.Count - 1; i++)
            {
                if (GrillaGastos.Rows[i].Cells[0].Value.ToString() == Codigo.ToString() && GrillaGastos.Rows[i].Cells[2].Value.ToString() == Tipo)
                {
                    MessageBox.Show("Ya se ha ingresado el gasto", Clases.cMensaje.Mensaje());
                    return;
                }
            }
            DataTable tListado = new DataTable();
            tListado.Columns.Add("Codigo");
            tListado.Columns.Add("Descripcion");
            tListado.Columns.Add("Tipo");
            tListado.Columns.Add("Importe");
            for (int i = 0; i < GrillaGastos.Rows.Count - 1; i++)
            {
                string sCodigo = GrillaGastos.Rows[i].Cells[0].Value.ToString();
                string sDescripcion = GrillaGastos.Rows[i].Cells[1].Value.ToString();
                string sTipo = GrillaGastos.Rows[i].Cells[2].Value.ToString();
                string sImporte = GrillaGastos.Rows[i].Cells[3].Value.ToString();
                DataRow r;
                r = tListado.NewRow();
                r[0] = sCodigo;
                r[1] = sDescripcion;
                r[2] = sTipo;
                r[3] = sImporte;
                tListado.Rows.Add(r);
            }
            DataRow r1;
            r1 = tListado.NewRow();
            r1[0] = Codigo;
            r1[1] = Descripcion;
            r1[2] = Tipo;
            r1[3] = Importe;
            tListado.Rows.Add(r1);
            GrillaGastos.DataSource = tListado;
            Clases.cFunciones fun = new Clases.cFunciones();
          // txtTotalGasto.Text = fun.CalcularTotalGrilla(GrillaGastos, "Importe").ToString();
         //   if (txtTotalGasto.Text != "")
        //    {

        //        txtTotalGasto.Text = fun.FormatoEnteroMiles(txtTotalGasto.Text);
        //    }
            //GrillaGastos.Columns[0].Visible = false;
            //GrillaGastos.Columns[2].Visible = false;
            //txtImporteGastoTransferencia.Text = "";
            //txtImporteGastoRecepcion.Text = "";
            //GrillaGastos.Columns[1].Width = 250;

            //txtTotalGastosRecepcion.Text = fun.CalcularTotalGrilla(GrillaGastosRecepcion, "Importe").ToString();
            //if (txtTotalGastosRecepcion.Text != "")
            //{
            //    txtTotalGastosRecepcion.Text = fun.FormatoEnteroMiles(txtTotalGastosRecepcion.Text);
            //}

            //double TotalVenta = 0;
            //double PrecioVenta = 0;
            //double TotalGastos = 0;
            //double TotalGastosRecepcion = 0;

            //if (txtTotalVenta.Text != "")
            //{
            //    PrecioVenta = fun.ToDouble(txtPrecioVenta.Text);
            //}

            //if (txtTotalGasto.Text != "")
            //{
            //    TotalGastos = fun.ToDouble(txtTotalGasto.Text);
            //}

            //if (txtTotalGastosRecepcion.Text != "")
            //{
            //    TotalGastosRecepcion = fun.ToDouble(txtTotalGastosRecepcion.Text);
            //}

            //TotalVenta = PrecioVenta + TotalGastos + TotalGastosRecepcion;
            //txtTotalVenta.Text = TotalVenta.ToString();
            //txtTotalVenta.Text = fun.FormatoEnteroMiles(txtTotalVenta.Text);
            ////CalcularSubTotal(); 
        }

        private string GetFormasPago()
        {
            Clases.cFunciones fun = new Clases.cFunciones ();
            string texto = "";
            int b = 0;
            if (txtSenia.Text != "")
            {
                texto = "Seña adelanto " + txtSenia.Text;
                b = 1;
            }
            if (txtEfectivo.Text != "")
            {
                if (b == 0)
                    texto = " efectivo en este acto " + txtEfectivo.Text;
                else
                    texto = texto + ",efectivo en este acto " + txtEfectivo.Text;
                b = 1;
            }
            if (txtDocumentos.Text != "")
            {
                if (b == 0)
                    texto = " Documento de " + txtDocumentos.Text;
                else
                    texto = texto + ", Documento de " + txtDocumentos.Text;
            }

            if (txtAutoPartePago.Text != "")
                texto = texto + txtAutoPartePago.Text;

            // busco si hubo prenda
            Clases.cPrenda prenda = new Clases.cPrenda();
            DataTable trdo = prenda.GetPrendaxCodVenta(Convert.ToInt32(Principal.CodigoPrincipalAbm));
            if (trdo.Rows.Count > 0)
            {
                string Importe = trdo.Rows[0]["Importe"].ToString();
                Importe = fun.SepararDecimales(Importe);
                Importe = fun.FormatoEnteroMiles(Importe);
                string Descripcion = trdo.Rows[0]["Descripcion"].ToString();
                texto = texto + ",crédito prendario a cargo de " + Descripcion;
                texto = texto + ", por un valor de " + Importe;
            }

            texto = texto + ",sobre el cual se aplicaran los siguientes descuentos:";	
			if (txtRentas.Text !="")
				texto = texto +", rentas :" + txtRentas.Text ;
			 
			if (txtMunicipalidad.Text !="")
				texto = texto +", municipalidad :" + txtMunicipalidad.Text ;
			 
			if (txtMultas.Text !="")
				texto = texto +", multas :" + txtMultas.Text ;	

            if (txtMultas.Text !="")
				texto = texto +", rentas :" + txtMultas.Text ;
	 
             if (txtVerificacion.Text !="")  
				texto = texto +", verificación :" + txtVerificacion.Text ;
             
             if (txtFirmasyForm.Text !="")  
				texto = texto +", firmas y form. :" + txtFirmasyForm.Text ;
             
             if (txtCancelacionPrenda.Text !="")  
				texto = texto +", cancelac prenda. :" + txtCancelacionPrenda.Text ;
             
            if (txtOtros.Text !="")  
				texto = texto +", cancelac prenda. :" + txtOtros.Text ;
																					

            return texto;
        }

        public void GetAutoPartePago(Int32 CodVenta)
        {
            Clases.cFunciones fun = new Clases.cFunciones();
            Clases.cVenta obj = new Clases.cVenta();
            DataTable trdo = obj.GetAutosPartePago(CodVenta);
            if (trdo.Rows.Count > 0)
            {
                if (trdo.Rows[0]["CodAuto"].ToString()!="")
                {
                    string sImporte = trdo.Rows[0]["Importe"].ToString();
                    if (sImporte != "" && sImporte !="0")
                    {
                        sImporte = fun.SepararDecimales(sImporte);
                        sImporte = fun.FormatoEnteroMiles(sImporte);
                    }
                    Int32 CodAuto = Convert.ToInt32(trdo.Rows[0]["CodAuto"].ToString());
                    Clases.cAuto auto = new Clases.cAuto();
                    DataTable tauto = auto.GetAutoxCodigo(CodAuto);
                    {
                        if (tauto.Rows.Count > 0)
                        {
                            string Descrip = " Un vehículo" + tauto.Rows[0]["Marca"].ToString() + " " + tauto.Rows[0]["Descripcion"].ToString();
                            Descrip = Descrip + " MOTOR N º" + tauto.Rows[0]["Motor"].ToString();
                            Descrip = Descrip + " CHASIS N º" + tauto.Rows[0]["Chasis"].ToString();
                            Descrip = Descrip + " AÑO " + tauto.Rows[0]["Anio"].ToString();
                            Descrip = Descrip + " DOMINIO " + tauto.Rows[0]["Patente"].ToString();
                            Descrip = Descrip + " valuado en " + sImporte;
                            txtAutoPartePago.Text = Descrip;

                        }
                    }
                }
            }
        }

        private void txtVerificacion_KeyPress(object sender, KeyPressEventArgs e)
        {
            Clases.cFunciones fun = new Clases.cFunciones ();
            fun.SoloEnteroConPunto(sender, e);
        }

        private void txtFirmasyForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            Clases.cFunciones fun = new Clases.cFunciones();
            fun.SoloEnteroConPunto(sender, e);
        }

        private void txtRentas_KeyPress(object sender, KeyPressEventArgs e)
        {
            Clases.cFunciones fun = new Clases.cFunciones();
            fun.SoloEnteroConPunto(sender, e);
        }

        private void txtMunicipalidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            Clases.cFunciones fun = new Clases.cFunciones();
            fun.SoloEnteroConPunto(sender, e);
        }

        private void txtCancelacionPrenda_KeyPress(object sender, KeyPressEventArgs e)
        {
            Clases.cFunciones fun = new Clases.cFunciones();
            fun.SoloEnteroConPunto(sender, e);
        }

        private void txtOtros_KeyPress(object sender, KeyPressEventArgs e)
        {
            Clases.cFunciones fun = new Clases.cFunciones();
            fun.SoloEnteroConPunto(sender, e);
        }

        private void txtMultas_KeyPress(object sender, KeyPressEventArgs e)
        {
             Clases.cFunciones fun = new Clases.cFunciones();
            fun.SoloEnteroConPunto(sender, e);
        }

        private void txtVerificacion_Leave(object sender, EventArgs e)
        {
            Clases.cFunciones fun = new Clases.cFunciones ();
            fun.FormatoEnteroMiles (txtVerificacion.Text);
        }

        private void txtFirmasyForm_Leave(object sender, EventArgs e)
        {  
            Clases.cFunciones fun = new Clases.cFunciones ();
            fun.FormatoEnteroMiles (txtFirmasyForm.Text);
        }

        private void txtRentas_Leave(object sender, EventArgs e)
        {   
            Clases.cFunciones fun = new Clases.cFunciones ();
            fun.FormatoEnteroMiles (txtRentas.Text);
        }

        private void txtMunicipalidad_Leave(object sender, EventArgs e)
        {   
            Clases.cFunciones fun = new Clases.cFunciones ();
            fun.FormatoEnteroMiles (txtMunicipalidad.Text);
        }

        private void txtCancelacionPrenda_Leave(object sender, EventArgs e)
        {   
            Clases.cFunciones fun = new Clases.cFunciones ();
            fun.FormatoEnteroMiles (txtCancelacionPrenda.Text);
        }

        private void txtMultas_Leave(object sender, EventArgs e)
        {     
            Clases.cFunciones fun = new Clases.cFunciones ();
            fun.FormatoEnteroMiles (txtMultas.Text);
        }

        private void txtOtros_Leave(object sender, EventArgs e)
        {  
            Clases.cFunciones fun = new Clases.cFunciones ();
            fun.FormatoEnteroMiles (txtOtros.Text);
        }

        private string  GetExTitular(Int32 CodStock)
        {
            string Texto = "";
            cStockAuto obj = new cStockAuto();
            DataTable trdo = obj.GetStockxCodigo(CodStock);
            if (trdo.Rows.Count >0)
            {
                Int32 CodCliente = Convert.ToInt32(trdo.Rows[0]["CodCliente"].ToString()); ;
                Texto = GetDatosClientexCod(CodCliente);
            }
            return Texto;
        }

        private string GetDatosClientexCod(Int32 CodCliente)
        {
            string texto = "";
            string Nom = "";
            string Ape = "";
            string NroDoc = "";
            string Calle = "";
            string Numero = "";
            Int32 CodBarrio = 0;
            string Ciudad = "";
            string Provincia = "";
            cCliente cli = new Clases.cCliente();
            DataTable tbcli = cli.GetClientesxCodigo(CodCliente);
            if (tbcli.Rows.Count > 0)
            {
                Nom = tbcli.Rows[0]["Nombre"].ToString();
                Ape = tbcli.Rows[0]["Apellido"].ToString();
                NroDoc = tbcli.Rows[0]["NroDocumento"].ToString();
                Calle = tbcli.Rows[0]["Calle"].ToString();
                Numero = tbcli.Rows[0]["Numero"].ToString();
                if (tbcli.Rows[0]["CodBarrio"].ToString()!="")
                {
                    CodBarrio = Convert.ToInt32(tbcli.Rows[0]["CodBarrio"].ToString());
                    cBarrio bar = new cBarrio();
                    DataTable tbBarrio = bar.GetBarrioCiudadProvincia(CodBarrio);
                    if (tbBarrio.Rows.Count >0)
                    {
                        if (tbBarrio.Rows[0]["Ciudad"].ToString() != "")
                            Ciudad = tbBarrio.Rows[0]["Ciudad"].ToString();
                        if (tbBarrio.Rows[0]["Provincia"].ToString() != "")
                            Provincia = tbBarrio.Rows[0]["Provincia"].ToString();
                    }
                }
            }
            texto = Ape + " " + Nom;
            if (NroDoc != "")
                texto = texto + ", DNI " + NroDoc.ToString();
            if (Calle != "")
                texto = texto + " con domicilio en calle " + Calle;
            if (Numero != "")
                texto = texto + " Numero " + Numero;
            if (Ciudad != "")
                texto = texto + " de la Ciudad de " + Ciudad;
            if (Provincia != "")
                texto = texto + ", Provincia " + Provincia;
            return texto;
        }
    }
}

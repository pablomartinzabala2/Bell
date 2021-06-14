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
    public partial class FrmAbmCliente : Form
    {
        public FrmAbmCliente()
        {
            InitializeComponent();
            InicializarComponentes();
        }

        private void InicializarComponentes()
        {
            Clases.cFunciones fun = new Clases.cFunciones();
            fun.LlenarCombo(cmb_CodTipoDoc, "TipoDocumento", "Nombre", "CodTipoDoc");
            if (cmb_CodTipoDoc.Items.Count > 0)
                cmb_CodTipoDoc.SelectedIndex = 1;
            cmb_CodTipoDoc.Enabled = false;  
            fun.LlenarCombo(cmbProvincia2, "Provincia", "Nombre", "CodProvincia");
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            Clases.cFunciones fun = new Clases.cFunciones();
            if (Validar() == true)
            {
                fun.ModificarGenerico(this, "Cliente", "CodCliente", "1");
                //  if (txtCodCLiente.Text =="")
                //      fun.GuardarNuevoGenerico(this, "Cliente");
            }


        }

        private Boolean Validar()
        {
            if (txt_NroDocumento.Text == "")
            {
                MessageBox.Show("Debe ingresar un número de documento para continuar", Clases.cMensaje.Mensaje());
                return false;
            }

            if (txt_Apellido.Text == "")
            {
                MessageBox.Show("Debe ingresar un apellido para continuar", Clases.cMensaje.Mensaje());
                return false;
            }

            if (txt_Nombre.Text == "")
            {
                MessageBox.Show("Debe ingresar un nombre para continuar", Clases.cMensaje.Mensaje());
                return false;
            }
            return true;
        }

        private void FrmAbmCliente_Load(object sender, EventArgs e)
        {
            Botonera(1);
            Grupo.Enabled = true;
        }

        private void Botonera(int Jugada)
        {
            switch (Jugada)
            {
                //estado inicial
                case 1:
                    btnNuevo.Enabled = true;
                    btnEditar.Enabled = false;
                    btnEliminar.Enabled = false;
                    btnAceptar.Enabled = false;
                    btnCancelar.Enabled = false;

                    break;
                case 2:
                    btnNuevo.Enabled = false;
                    btnEditar.Enabled = false;
                    btnEliminar.Enabled = true;
                    btnAceptar.Enabled = true;
                    btnCancelar.Enabled = true;

                    break;
                case 3:
                    //viene del buscador
                    btnNuevo.Enabled = true;
                    btnEditar.Enabled = true;
                    btnEliminar.Enabled = true;
                    btnAceptar.Enabled = false;
                    btnCancelar.Enabled = false;


                    break;
            }


        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Botonera(2);
            Clases.cFunciones fun = new Clases.cFunciones();
            fun.LimpiarGenerico(this);
            txtCodCLiente.Text = "";
            Grupo.Enabled = true;
            if (cmb_CodTipoDoc.Items.Count   > 0)
                cmb_CodTipoDoc.SelectedIndex = 1;
        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
          //  FrmConsultaCLiente form = new FrmConsultaCLiente();
          //  form.FormClosing += new FormClosingEventHandler(form_FormClosing);
          //  form.ShowDialog();
            //codigo generico
            Principal.OpcionesdeBusqueda = "Nombre;Apellido;NroDocumento";
            Principal.TablaPrincipal = "Cliente";
            Principal.OpcionesColumnasGrilla = "CodCliente;Nombre;Apellido";
            Principal.ColumnasVisibles = "0;1;1";
            Principal.ColumnasAncho = "0;290;290";
            FrmBuscadorGenerico form = new FrmBuscadorGenerico();
            form.FormClosing += new FormClosingEventHandler(form_FormClosing);
            
            form.ShowDialog();
        }

        private void form_FormClosing(object sender, FormClosingEventArgs e)
        {
            Clases.cFunciones fun = new Clases.cFunciones();
            //CargarJugador(Convert.ToInt32(PRINCIPAL.CDOGIO_JUGADOR));
            if (Principal.CodigoPrincipalAbm != null)
            {
                if (Principal.CodigoPrincipalAbm != "")
                {
                    Botonera(3);
                    txtCodCLiente.Text = Principal.CodigoPrincipalAbm.ToString();
                    
                    if (Principal.CodigoPrincipalAbm != "")
                        fun.CargarControles(this, "Cliente", "CodCliente", txtCodCLiente.Text);
                    Grupo.Enabled = false;

                    if (txt_RutaImagen.Text !="")
                    {
                        CargarImaagen();
                    }
                    cCliente cli = new cCliente();
                    DataTable tbCLi = cli.GetClientesxCodigo(Convert.ToInt32(txtCodCLiente.Text));
                    if (tbCLi.Rows.Count >0)
                    {
                        if (tbCLi.Rows[0]["CodBarrio"].ToString ()!="")
                        {
                            Int32 CodBarrio = Convert.ToInt32(tbCLi.Rows[0]["CodBarrio"].ToString());
                            CargarCiudadxBarrio(CodBarrio);
                        }

                    }
                    return;
                }
                
            }
            
            
            if (Principal.CampoIdSecundarioGenerado != "")
            {
                
                switch (Principal.NombreTablaSecundario)
                {
                    case "Barrio":                  
                        Int32 CodCity = Convert.ToInt32(cmbCiudad2.SelectedValue);
                        Int32 CodBarrio = Convert.ToInt32(Principal.CampoIdSecundarioGenerado);
                        cBarrio obj = new cBarrio();
                        obj.ActualizarCiudad(CodBarrio, CodCity);
                        DataTable tbBarrio = obj.GetBarrioxCiudad(CodCity);
                        fun.LlenarComboDatatable(cmb_CodBarrio, tbBarrio, "Nombre", "CodBarrio");
                        // fun.LlenarCombo(CmbBarrio, "Barrio", "Nombre", "CodBarrio");
                        cmb_CodBarrio.SelectedValue = Principal.CampoIdSecundarioGenerado;
                        break;
                    case "Provincia":  
                        fun.LlenarCombo(cmbProvincia2, "Provincia", "Nombre", "CodProvincia");
                        cmbProvincia2.SelectedValue = Principal.CampoIdSecundarioGenerado;
                        break;
                    case "Ciudad":
                        Int32 CodCiudad = 0;
                         CodCiudad = Convert.ToInt32(Principal.CampoIdSecundarioGenerado);
                         Int32 CodProvincia = Convert.ToInt32(cmbProvincia2.SelectedValue);
                         cCiudad city = new Clases.cCiudad();
                         city.ActualizarProvincia(CodCiudad, CodProvincia);
                         DataTable tbCiudad = city.GetCiudadxCodProvincia(CodProvincia);
                         fun.LlenarComboDatatable(cmbCiudad2, tbCiudad, "Nombre", "CodCiudad");
                         cmbCiudad2.SelectedValue = Principal.CampoIdSecundarioGenerado;
                        break;
                }
            }
        }

        private void CargarImaagen()
        {
            try
            {
                string Ruta = txt_RutaImagen.Text;
                Imagen.Image = System.Drawing.Image.FromFile(Ruta);
            }
            catch (Exception)
            {
                MessageBox.Show("No se pudo cargar la imagen");
            }
            
        }
        private void btnEditar_Click(object sender, EventArgs e)
        {
            Botonera(2);
            Grupo.Enabled = true;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Botonera(1);
            Clases.cFunciones fun = new Clases.cFunciones();
            fun.LimpiarGenerico(this);
            txtCodCLiente.Text = "";
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Clases.cFunciones fun = new Clases.cFunciones();
            if (Validar() == true)
            {
                //se usa por las dudas ingreso ya exista el deni
                //y no grabe repetido el documento
                UbicaCliente();
                  if (txtCodCLiente.Text =="")
                      fun.GuardarNuevoGenerico(this, "Cliente");
                  else
                      fun.ModificarGenerico(this, "Cliente", "CodCliente", txtCodCLiente.Text);
                  MessageBox.Show("Datos grabados Correctamente", Clases.cMensaje.Mensaje());
                  Botonera(1);
                  fun.LimpiarGenerico(this);
                  txtCodCLiente.Text = "";  
                if (cmbProvincia2.Items.Count >0)
                    cmbProvincia2.SelectedIndex = 0;
                if (cmbCiudad2.Items.Count > 0)
                    cmbCiudad2.SelectedIndex = 0;
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            string msj = "Confirma eliminar el Cliente ";
            var result = MessageBox.Show(msj, "Información",
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question);

            // If the no button was pressed ...
            if (result == DialogResult.No)
            {
                return;
            }
            Clases.cCliente clie = new Clases.cCliente();
            if (clie.PuedeBorrar(Convert.ToInt32(txtCodCLiente.Text)))
            {
                Clases.cFunciones fun = new Clases.cFunciones();
                fun.EliminarGenerico("Cliente", "CodCliente", txtCodCLiente.Text);
                MessageBox.Show("El cliente se ha eliminado de la base", Clases.cMensaje.Mensaje());
                fun.LimpiarGenerico(this);
                txtCodCLiente.Text = ""; 
                Botonera(1);
            }
            else
            {
                MessageBox.Show("El cliente no se puede eliminar, se perderían datos historicos.", Clases.cMensaje.Mensaje());
            }
        }

        private void txt_NroDocumento_TextChanged(object sender, EventArgs e)
        { 
            /*
            Int32 CodTipoDoc = 0;
            if (cmbDocumento.SelectedIndex > 0)
                CodTipoDoc = Convert.ToInt32(cmbDocumento.SelectedValue);
            string nroDocumento = txt_NroDocumento.Text;
            Clases.cCliente cliente = new Clases.cCliente();
            DataTable trdo = cliente.GetClientesxNroDoc(CodTipoDoc, nroDocumento);
            if (trdo.Rows.Count > 0)
            {
                txt_Nombre.Text = trdo.Rows[0]["Nombre"].ToString();
                txt_Apellido.Text = trdo.Rows[0]["Apellido"].ToString();
                txtM_Telefono.Text = trdo.Rows[0]["Telefono"].ToString();
                txtM_Celular.Text = trdo.Rows[0]["Celular"].ToString();
                txt_Calle.Text = trdo.Rows[0]["Calle"].ToString();
                txt_Numero.Text = trdo.Rows[0]["Numero"].ToString();
                if (trdo.Rows[0]["CodBarrio"].ToString() != "")
                    cmb_CodBarrio.SelectedValue = trdo.Rows[0]["CodBarrio"].ToString();
                txtCodCLiente.Text = trdo.Rows[0]["CodCliente"].ToString();
            }
            else
            {
                txt_Nombre.Text = "";
                txt_Apellido.Text = "";
                txtM_Telefono.Text = "";
                txtM_Celular.Text = "";
                txtCodCLiente.Text = "";
                cmb_CodBarrio.SelectedIndex = 0;
                txt_Calle.Text = "";
                txt_Numero.Text = "";
            }
              */  

                
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNuevoBarrio_Click(object sender, EventArgs e)
        {
            Principal.CodigoPrincipalAbm = null;
            Principal.CampoIdSecundario = "CodBarrio";
            Principal.CampoNombreSecundario = "Nombre";
            Principal.NombreTablaSecundario = "Barrio";
            FrmAltaBasica form = new FrmAltaBasica();
            form.FormClosing += new FormClosingEventHandler(form_FormClosing);
            form.ShowDialog();
        }

        private void UbicaCliente()
        {
            
            Int32 CodTipoDoc = 0;
            if (cmb_CodTipoDoc.SelectedIndex > 0)
                CodTipoDoc = Convert.ToInt32(cmb_CodTipoDoc.SelectedValue);
            string nroDocumento = txt_NroDocumento.Text;
            Clases.cCliente cliente = new Clases.cCliente();
            DataTable trdo = cliente.GetClientesxNroDoc(CodTipoDoc, nroDocumento);
            if (trdo.Rows.Count > 0)
            {
                txtCodCLiente.Text = trdo.Rows[0]["CodCliente"].ToString();
            }
            
              
        }

        private void BarraBotones_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void cmbProvincia2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProvincia2.SelectedIndex < 1)
            {
                return;
            }  
            Int32 CodProvincia = Convert.ToInt32(cmbProvincia2.SelectedValue);
            cCiudad ciudad = new Clases.cCiudad();
            DataTable trdo = ciudad.GetCiudadxCodProvincia(CodProvincia);
            cFunciones fun = new cFunciones();
            fun.LlenarComboDatatable(cmbCiudad2, trdo, "Nombre", "CodCiudad");
        }

        private void cmbCiudad2_SelectedIndexChanged(object sender, EventArgs e)
        {
             
            if (cmbCiudad2.SelectedIndex < 1)
            {
                // MessageBox.Show("Seleccione una ciudad");
                return;
            }
             
            Int32 CodCiudad = Convert.ToInt32(cmbCiudad2.SelectedValue);
            cBarrio barrio = new cBarrio();
            DataTable tbBarrio = barrio.GetBarrioxCiudad(CodCiudad);
            cFunciones fun = new cFunciones();
            fun.LlenarComboDatatable(cmb_CodBarrio, tbBarrio, "Nombre", "CodBarrio");
        }

        private void CargarCiudadxBarrio(Int32 CodBarrio)
        {
            if (CodBarrio >0)
            {
                cBarrio barrio = new cBarrio();
                DataTable tbBarrio = barrio.GetBarrioxId(CodBarrio);
                if (tbBarrio.Rows.Count > 0)
                {
                    if (tbBarrio.Rows[0]["CodCiudad"].ToString() != "")
                    {
                        Int32 CodCiudad = Convert.ToInt32(tbBarrio.Rows[0]["CodCiudad"].ToString());
                        cCiudad objCiudad = new cCiudad();
                        DataTable tbCiudad = objCiudad.GetCiudadxId(CodCiudad);
                        if (tbCiudad.Rows.Count > 0)
                        {
                            if (tbCiudad.Rows[0]["CodProvincia"].ToString() != "")
                            { 
                                Int32 CodProvincia = Convert.ToInt32(tbCiudad.Rows[0]["CodProvincia"].ToString());
                                cmbProvincia2.SelectedValue = CodProvincia.ToString();
                                DataTable trCiudad = objCiudad.GetCiudadxCodProvincia(CodProvincia);
                                cFunciones fun = new cFunciones();
                                fun.LlenarComboDatatable(cmbCiudad2, trCiudad, "Nombre", "CodCiudad");
                                cmbCiudad2.SelectedValue = CodCiudad.ToString();
                                cmb_CodBarrio.SelectedValue = CodBarrio.ToString();
                            }
                        }
                    }
                }
            }
        }

        private void btnAgregarProvincia2_Click(object sender, EventArgs e)
        {
            Principal.CampoIdSecundario = "CodProvincia";
            Principal.CampoNombreSecundario = "Nombre";
            Principal.NombreTablaSecundario = "Provincia";
            Principal.CodigoPrincipalAbm = null;
            FrmAltaBasica form = new FrmAltaBasica();
            form.FormClosing += new FormClosingEventHandler(form_FormClosing);
            form.ShowDialog();
        }

        private void btnAgregarCiudad2_Click(object sender, EventArgs e)
        {
            if (cmbProvincia2.SelectedIndex < 1)
            {
                MessageBox.Show("Debe seleccionar una provincia para continuar");
                return;
            }
           
            Principal.CampoIdSecundario = "CodCiudad";
            Principal.CampoNombreSecundario = "Nombre";
            Principal.NombreTablaSecundario = "Ciudad";
            FrmAltaBasica form = new FrmAltaBasica();
            form.FormClosing += new FormClosingEventHandler(form_FormClosing);
            form.ShowDialog();
        }

        private void btnSubirFotoCliente_Click(object sender, EventArgs e)
        {
            cImagen imgAuto = new cImagen();
            string NroImagen = imgAuto.GetProximaImagen().ToString();
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                string ruta = file.FileName;
                txt_RutaImagen.Text = ruta;
                Imagen.Image = System.Drawing.Image.FromFile(ruta);
                string Extension = System.IO.Path.GetExtension(file.FileName.ToString());
                string RutaGrabar = imgAuto.GetRuta() + NroImagen + "." + Extension;
                Imagen.Image.Save(RutaGrabar);
                imgAuto.Grabar(Convert.ToInt32(NroImagen));
                txt_RutaImagen.Text = RutaGrabar;
            }
            else
            {
                txt_RutaImagen.Text = "";
            }
        }
    }
}
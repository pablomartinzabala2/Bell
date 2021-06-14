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
    public partial class FrmVerFotos : Form
    {
        public FrmVerFotos()
        {
            InitializeComponent();
        }

        private void FrmVerFotos_Load(object sender, EventArgs e)
        { 
            try
            { 
                if (Principal.RutaImagen!="")
                {
                    string Ruta = Principal.RutaImagen;
                    Imagen.Image = System.Drawing.Image.FromFile(Ruta);
                }
                /*
                if (Principal.CodAutoSeleccionado != null)
                {
                    Int32 CodAuto = Convert.ToInt32(Principal.CodAutoSeleccionado);
                    cAuto auto = new Clases.cAuto();
                    DataTable trdo = auto.GetAutoxCodigo(CodAuto);
                    if (trdo.Rows.Count > 0)
                    {
                        if (trdo.Rows[0]["RutaImagen"].ToString() != "")
                        {
                            string Ruta = trdo.Rows[0]["RutaImagen"].ToString();
                            Imagen.Image = System.Drawing.Image.FromFile(Ruta);
                            this.Text = trdo.Rows[0]["Patente"].ToString();
                        }
                    }
                }
                */
            }
            catch (Exception ex)
            {
                MessageBox.Show("No s pudo cargar la imagen");
            }
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Concesionaria
{
    public partial class FrmBorrarTablas : Form
    {
        public FrmBorrarTablas()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        { 
            if (textBox1.Text.ToUpper() != "ADMIN")
            {
                MessageBox.Show("Ingresar clave");
                return;
            }

            string msj = "Confirma eliminar";
            var result = MessageBox.Show(msj, "Información",
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question);

            // If the no button was pressed ...
            if (result == DialogResult.No)
            {
                return;
            }

            Clases.cConfiguracion.BorrarTablas();
            MessageBox.Show("datos borrados", Clases.cMensaje.Mensaje()); 
        }
    }
}

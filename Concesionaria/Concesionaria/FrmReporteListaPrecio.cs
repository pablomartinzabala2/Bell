using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
namespace Concesionaria
{
    public partial class FrmReporteListaPrecio : Form
    {
        public FrmReporteListaPrecio()
        {
            InitializeComponent();
        }

        private void FrmReporteListaPrecio_Load(object sender, EventArgs e)
        {
            PageSettings pg = new PageSettings();
            pg.Margins.Left = 0;
            pg.Margins.Right = 0;
            pg.Margins.Top = 0;
            pg.Margins.Bottom = 0;
            this.reportViewer1.SetPageSettings(pg);

            var setup = this.reportViewer1.GetPageSettings();
            setup.Margins = new System.Drawing.Printing.Margins(1, 1, 1, 1);
            this.reportViewer1.SetPageSettings(setup);

            // TODO: This line of code loads data into the 'CONCESIONARIADataSet.ReporteAuto' table. You can move, or remove it, as needed.
            this.ReporteAutoTableAdapter.Fill(this.CONCESIONARIADataSet.ReporteAuto);
            // TODO: This line of code loads data into the 'CONCESIONARIADataSet.Reporte' table. You can move, or remove it, as needed.
            this.ReporteTableAdapter.Fill(this.CONCESIONARIADataSet.Reporte);

            this.reportViewer1.RefreshReport();
        }
    }
}

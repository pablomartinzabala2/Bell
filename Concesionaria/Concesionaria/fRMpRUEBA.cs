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
    public partial class fRMpRUEBA : Form
    {
        public fRMpRUEBA()
        {
            InitializeComponent();
        }

        private void fRMpRUEBA_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }
    }
}

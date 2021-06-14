using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace Concesionaria.Clases
{
    public static  class cConexion
    {
        public static  string Cadenacon()
        {
            //*****CASA**********DESKTOP-BI5616B\SQLEXPRESS
          //  string cadena = "Data Source=DESKTOP-BI5616B\\SQLEXPRESS;Initial Catalog=CHACO;Integrated Security=True";
            string cadena = Concesionaria.Properties.Settings.Default.CONCESIONARIAConnectionString;
            //CHACO
            //  string cadena = "Data Source=DESKTOP-4I4D3O9\\SQLEXPRESS;Initial Catalog=CHACO;Integrated Security=True";
          


            return cadena;
        }
    }
}

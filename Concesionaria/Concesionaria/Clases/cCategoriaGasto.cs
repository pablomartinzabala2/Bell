using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace Concesionaria.Clases
{
    public class cCategoriaGasto
    {
        public string GetGastoxId(Int32 CodCategoriaGasto)
        {
            string Nombre = "";
            string sql = "select Nombre ";
            sql = sql + " from CategoriaGasto";
            sql = sql + " where CodCategoriaGasto =" + CodCategoriaGasto.ToString();
            DataTable trdo = cDb.ExecuteDataTable(sql);
            if (trdo.Rows.Count > 0)
                Nombre = trdo.Rows[0]["Nombre"].ToString();
            return Nombre;
        }
    }
}

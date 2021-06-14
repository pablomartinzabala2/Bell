using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace Concesionaria.Clases
{
    public class cCiudad
    {
        public void ActualizarProvincia(Int32 CodCiudad,Int32 CodProvincia)
        {
            string sql = "Update Ciudad ";
            sql = sql + " set CodProvincia=" + CodProvincia.ToString();
            sql = sql + " where CodCiudad=" + CodCiudad.ToString();
            cDb.ExecutarNonQuery(sql);
        }

        public DataTable GetCiudadxCodProvincia(Int32 CodProvincia)
        {
            string sql = " select * from Ciudad ";
            sql = sql + " where CodProvincia =" + CodProvincia.ToString();
            sql = sql + " order by Nombre";
            return cDb.ExecuteDataTable(sql);
        }

        public DataTable GetProvinciaxCodCiudad(Int32 CodCiudad)
        {
            string sql = " select p.* ";
            sql = sql + " from provincia p,ciudad c ";
            sql = sql + " where p.CodProvincia = c.CodProvincia ";
            sql = sql + " and c.CodCiudad=" + CodCiudad.ToString();
            return cDb.ExecuteDataTable(sql);
        }

        public DataTable GetCiudadxId(Int32 CodCiudad)
        {
            string sql = "select * from ciudad ";
            sql = sql + " where CodCiudad=" + CodCiudad.ToString();
            return cDb.ExecuteDataTable(sql);
        }
    }
}

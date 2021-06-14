using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
namespace Concesionaria.Clases
{
    public class cImagen
    {
        public string GetRuta()
        {
            string Ruta = "c:\\ARCHIVO\\";
            return Ruta;
        }

        public int GetProximaImagen()
        {
            int CodImagen = 0;
            int b = 0;
            string sql = "select * from Imagen";
            DataTable trdo = cDb.ExecuteDataTable(sql);
            if (trdo.Rows.Count >0)
            {
                if (trdo.Rows[0]["CodImagen"].ToString ()!="")
                {
                    b = 1;
                    CodImagen = Convert.ToInt32(trdo.Rows[0]["CodImagen"].ToString());
                }
            }
            if (b ==0)
            {
                sql = "Insert into Imagen(CodImagen) values(" + CodImagen.ToString() + ")";
                cDb.ExecutarNonQuery(sql);
            }
            CodImagen++;
            return CodImagen;
        }

        public void GrabarTransaccion(SqlConnection con, SqlTransaction Transaccion,Int32 CodImagen)
        {
            string sql = "Update Imagen set CodImagen=" + CodImagen.ToString();
            cDb.EjecutarNonQueryTransaccion(con, Transaccion, sql);
        }

        public void Grabar(Int32 CodImagen)
        {
            string sql = "Update Imagen set CodImagen=" + CodImagen.ToString();
            cDb.ExecutarNonQuery(sql);
        }
    }
}

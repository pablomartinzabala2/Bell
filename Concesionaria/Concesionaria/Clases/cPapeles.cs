using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
namespace Concesionaria.Clases
{
    public class cPapeles
    {
        public DataTable GetPapeles()
        {
            string sql = "select * from papeles";
            return cDb.ExecuteDataTable(sql);
        } 

        public void  InsertarPapeles(SqlConnection con, SqlTransaction Transaccion,Int32 CodPapel,Int32 CodStock,
            string Entrego,string Texto,DateTime? Fecha,DateTime? FechaVencimiento,Int32? CodCompra)
        {
            string sql = "insert into PapelesxStock";
            sql = sql + "(CodPapel, CodStock,Entrego";
            sql = sql + ", Texto,  Fecha, FechaVencimiento,CodCompra";
            sql = sql + ")";
            sql = sql + " values(";
            sql = sql + CodPapel.ToString();
            sql = sql + "," + CodStock.ToString();
            sql = sql + "," + "'" + Entrego + "'";
            sql = sql + "," + "'" + Texto + "'";
            if (Fecha !=null)
            {
                DateTime Fec = Convert.ToDateTime(Fecha);
                sql = sql + "," + "'" + Fec.ToShortDateString() + "'";
            }
            else
            {
                sql = sql + ",null";
            }

            if (FechaVencimiento != null)
            {
                DateTime Fec = Convert.ToDateTime(FechaVencimiento);
                sql = sql + "," + "'" + Fec.ToShortDateString() + "'";
            }
            else
            {
                sql = sql + ",null";
            }
            if (CodCompra !=null)
            {
                sql = sql + "," + CodCompra.ToString();
            }
            else
            {
                sql = sql + ",null";
            }
            
            sql = sql + ")";
            cDb.EjecutarNonQueryTransaccion (con, Transaccion, sql);
        }

        public DataTable GetPapelesxCodCompra(Int32  CodCompra)
        {
            string sql = " select ps.CodPapel,p.Nombre,";
            sql = sql + "ps.Entrego,ps.Texto,ps.Fecha,ps.FechaVencimiento";
            sql = sql + " from Papeles p,PapelesxStock ps";
            sql = sql + " where p.CodPapel = ps.CodPapel";
            sql = sql + " and CodCompra=" + CodCompra.ToString();
            return cDb.ExecuteDataTable(sql);
        }

        public void BorrarPapeles(SqlConnection con, SqlTransaction Transaccion,Int32 CodCompra)
        {
            string sql = "delete from PapelesxStock ";
            sql = sql + " where CodCompra=" + CodCompra.ToString ();
            cDb.EjecutarNonQueryTransaccion(con, Transaccion, sql);
        }

        public DataTable GetPapelesxCodStock(Int32 CodStock)
        {
            string sql = " select ps.CodPapel,p.Nombre,";
            sql = sql + "ps.Entrego,ps.Texto,ps.Fecha,ps.FechaVencimiento";
            sql = sql + " from Papeles p,PapelesxStock ps";
            sql = sql + " where p.CodPapel = ps.CodPapel";
            sql = sql + " and CodStock=" + CodStock.ToString();
            return cDb.ExecuteDataTable(sql);
        }
    }
}

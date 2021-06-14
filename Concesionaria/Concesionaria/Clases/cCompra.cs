using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace Concesionaria.Clases
{
    public class cCompra
    {
        public Int32 GetCodCompraxCodStock(Int32 CodStock)
        {
            Int32 CodCompra = -1;
            string sql = "select CodCompra from Compra";
            sql = sql + " where CodStockEntrada=" + CodStock.ToString ();
            DataTable trdo = cDb.ExecuteDataTable(sql);
            if (trdo.Rows.Count > 0)
            {
                if (trdo.Rows[0]["CodCompra"].ToString() != "")
                    CodCompra = Convert.ToInt32(trdo.Rows[0]["CodCompra"].ToString());
            }
            return CodCompra;
        }

        public DataTable GetCompraxCodigo(Int32 CodCompra)
        {
            string sql = "select * from compra";
            sql = sql + " where CodCompra=" + CodCompra.ToString ();
            return cDb.ExecuteDataTable(sql);
        }

        public DataTable getComprasxFecha(DateTime FechaDesde,DateTime FechaHasta,string Patente)
        {
            string sql = " select c.CodCompra, a.Patente,a.Descripcion,m.nombre,c.Fecha,c.ImporteCompra";
            sql = sql + " From Compra c,StockAuto s, auto a,Marca m";
            sql = sql + " where c.CodStockEntrada= s.CodStock";
            sql = sql + " and s.CodAuto=a.CodAuto";
            sql = sql + " and a.CodMarca= m.CodMarca";
            sql = sql + " and c.Fecha >=" + "'" + FechaDesde.ToShortDateString() + "'";
            sql = sql + " and c.Fecha <=" + "'" + FechaHasta.ToShortDateString() + "'";
            if (Patente !="")
            {
                sql = sql + " and a.Patente like " + "'%" + Patente + "%'";
            }
            return cDb.ExecuteDataTable(sql);
        }
    }
}

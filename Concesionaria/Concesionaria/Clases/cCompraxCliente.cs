using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
namespace Concesionaria.Clases
{
   public  class cCompraxCliente
    {
        public void Insertar(SqlConnection con, SqlTransaction Transaccion, Int32 CodCompra,Int32 CodCliente)
        {
            string sql = "insert into CompraxCliente(CodCompra,CodCliente)";
            sql = sql + " values(" + CodCompra.ToString();
            sql = sql + "," + CodCliente.ToString();
            sql = sql + ")";
            cDb.EjecutarNonQueryTransaccion(con, Transaccion, sql);
        }

        public DataTable GetClientexCodComrpa(Int32 CodCompra)
        {
            string sql = "select c.CodCliente,c.Apellido,c.Nombre";
            sql = sql + ",c.NroDocumento,c.Telefono";
            sql = sql + " from Cliente c,CompraxCliente cc";
            sql = sql + " where c.CodCliente= cc.CodCliente ";
            sql = sql + " and cc.CodCompra=" + CodCompra.ToString();
            return cDb.ExecuteDataTable(sql);
        }
    }
}

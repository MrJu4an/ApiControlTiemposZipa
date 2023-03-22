using System;
using System.Configuration;
using System.Data;
using CtecCore.Data.Oracle;
using System.Data.OracleClient;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Web.Configuration;
using Ctec;

namespace ApiZipaquira.QueryData
{
    public class ConBd
    {
        
        Conector db;
        DataTable dt1 = new DataTable();
        Configuration MiAppConf = WebConfigurationManager.OpenWebConfiguration("~");
        ConnectionStringsSection MiSession;
        ConnectionStringSettings ConfigStr;

        public ConBd()
        {
            MiSession = (ConnectionStringsSection)MiAppConf.GetSection("connectionStrings");
            ConfigStr = MiSession.ConnectionStrings["ConectionString"];
            db = new Conector(ConfigStr.ConnectionString);
            db.ValidarConexion();
        }

        public DataTable getTable(string SQL)
        {
            dt1 = null;
            try
            {
                //using (var dbs = new Conector(_ConectionString))
                //{
                //    lock (dbs)
                //    {
                //        dt1 = dbs.GetTable(SQL);
                //    }
                //}

                lock (db)
                {
                    dt1 = db.GetTable(SQL);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return dt1;
        }

        /// <summary>
        /// Ejecuta consutla sql retorna numero
        /// </summary>
        /// <param name="QRY">intruccion sql (consulta)</param>
        /// <returns>int</

        public int ExecuteQRY(string QRY)
        {
            int res = 0;
            try { 
           
                lock (db)
                {
                    res = db.ExecuteQRY(QRY);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return res;
        }

        public DataTable consultarTabla(string sql)
        {
            DataTable consulta = getTable(sql);
            return consulta;
        }

        public int ejecutarConsulta()
        {
            string resultado = "";
            int tamanio = 0;
            int R = 0;

            try
            {
                Configuration MiAppConf = WebConfigurationManager.OpenWebConfiguration("~");
                ConnectionStringsSection MiSession = (ConnectionStringsSection)MiAppConf.GetSection("connectionStrings");
                ConnectionStringSettings ConfigStr = MiSession.ConnectionStrings["ConectionString"];
                db = new Conector(ConfigStr.ConnectionString);
                OracleConnection conexion = new OracleConnection(ConfigStr.ConnectionString);
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conexion;
                cmd.CommandText = "RES_CONTABLE";
                cmd.CommandType = CommandType.StoredProcedure;
               // cmd.Parameters.Add("RESULTADO", OracleType.Int32).Direction = ParameterDirection.Output;
                conexion.Open();
                cmd.ExecuteNonQuery();
              //  R = (int)cmd.Parameters["RESULTADO"].Value;
             //   resultado = (string)R.ToString();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return R;
        }

        public DataTable consultaVehiculos(string sqlConsulta)
        {
            Configuration MiAppConf = WebConfigurationManager.OpenWebConfiguration("~");
            ConnectionStringsSection MiSession = (ConnectionStringsSection)MiAppConf.GetSection("connectionStrings");
            ConnectionStringSettings ConfigStr = MiSession.ConnectionStrings["ConectionString"];
           // db = new Conector(ConfigStr.ConnectionString);
            OracleConnection conexion = new OracleConnection(ConfigStr.ConnectionString);
            //  command.Parameters.Add(":username", OracleDbType.NVarchar2).Value = username;
            conexion.Open();
            OracleCommand command = new OracleCommand(sqlConsulta, conexion);
            OracleDataReader reader = command.ExecuteReader();
            DataTable tabla = new DataTable();
            // Variables para las columnas y las filas
            DataColumn column;
            DataRow row;
            // Se tiene que crear primero la columna asignandole Nombre y Tipo de datos    
            column = new DataColumn();
            //column.DataType = System.Type.GetType("System.Int32");
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "codigo";
            tabla.Columns.Add(column);
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "tipo";
            tabla.Columns.Add(column);
            while (reader.Read())
                {
                row = tabla.NewRow();
                row["codigo"] = reader["codigo"].ToString();
                row["tipo"] = reader["tipo"].ToString();
                tabla.Rows.Add(row);
            }
            if (reader==null)
            {
                int i = 0;
            }
            return tabla;
            }

        public DataRow GetRow(string SQL)
        {
            DataRow dr1 = null;
            dr1 = null;
            try
            {
                //using (var dbs = new Conector(_ConectionString))
                //{
                //    lock (dbs)
                //    {
                //        dr1 = dbs.GetRow(SQL);
                //    }
                //}

                lock (db)
                {
                    dr1 = db.GetRow(SQL);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return dr1;
        }

        /// <summary>
        /// Ejecuta consulta sql y retona valor string
        /// </summary>
        /// <param name="SQL">Consulta sql</param>
        /// <returns>valor string</returns>
        public string GetValue(string SQL)
        {
            string str1 = "";
            str1 = null;
            try
            {
                //using (Conector dbs = new Conector(_ConectionString))
                //{
                //    lock (dbs)
                //    {
                //        str1 = dbs.GetValue(SQL);
                //    }                    
                //}
                lock (db)
                {
                    str1 = db.GetValue(SQL);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return str1;
        }

        /// <summary>
        /// Funcion que se creo para ejecutar isos
        /// </summary>
        /// <param name="Nombre">Nombre del iso</param>
        /// <param name="parametros">Parametros del iso (trama)</param>
        /// <param name="puerto">Puerto de la movil</param>
        /// <returns></returns>
        public string executeIso(string Nombre, string parametros, string puerto)
        {
            string resultado = "";
            int tamanio = 0;
            try
            {
                OracleConnection conn = new OracleConnection(ConfigStr.ConnectionString);
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = Nombre;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("PARAMETROS", OracleType.NVarChar).Value = parametros;
                cmd.Parameters.Add("PUERTO", OracleType.Int32).Value = puerto;
                cmd.Parameters.Add("RESULTADO", OracleType.Int32).Direction = ParameterDirection.Output;

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    int R = (int)cmd.Parameters["RESULTADO"].Value;
                    resultado = (string)R.ToString();
                }
                catch (Exception ex)
                {
                    throw (ex);
                }

            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return resultado;

        }
    }
}

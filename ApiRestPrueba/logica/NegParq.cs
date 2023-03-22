using ApiZipaquira.Models;
using ApiZipaquira.QueryData;
using ApiZipaquira.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ApiZipaquira.logica
{
    public class NegParq
    {
        string sql;
        public RespLoguin validaUsuario(string nombre, string contraseña)
        {
            DataRow drUsuario = null;
            ConBd consulta = new ConBd();
            RespLoguin resp = new RespLoguin();
            Seguridad seg = new Seguridad();
            string clavDesencriptada = null;
            clavDesencriptada = seg.EncriptarAnterior(contraseña);

            sql = $@"SELECT NOMBRE, LOGINN, PASSWORDD, ESTADO,NOMBRE 
                          FROM USUARIO_OPERADOR 
                          WHERE LOGINN = '{ nombre }'";

            drUsuario = null;
            drUsuario = consulta.GetRow(sql);
            if (drUsuario == null)
            {
                resp.estado = false;
                resp.mensaje = "Usuario no existe Por favor verifique!";

                return resp;
            }

            if (!drUsuario["ESTADO"].Equals("A"))
            {
                resp.estado = false;
                resp.mensaje = "Usuario inactivo";

                return resp;
            }

          

            if (!drUsuario["PASSWORDD"].Equals(clavDesencriptada))
            {
                resp.estado = false;
                resp.mensaje = "Usuario o contraseña no valida";

                return resp;
            }
            else
            {

               // registrarTurno(nombre);
                RespLoguin resCambio = new RespLoguin();
                resCambio.estado = true;
                resCambio.mensaje = "Validado";
                resCambio.respuesta = drUsuario["NOMBRE"].ToString();
                return resCambio;
            }

        }

        public DataTable consultarEmpresas()
        {
            DataTable tablaEmpresas = new DataTable();
            ConBd consulta = new ConBd();
            sql = "SELECT CODEMPRE, NOMEMPRE, NITEMPRE " +
                   "FROM EMPRESA WHERE estado = 'A' ORDER BY nomempre ";
              tablaEmpresas = consulta.getTable(sql);
            return tablaEmpresas;
        }

        public RespLoguin consultaNomEmpresa()
        {
            RespLoguin resp = new RespLoguin();
            ConBd consulta = new ConBd();
            DataRow dtEmp = null;
            string empresa = null;
            sql = $@"SELECT * FROM GEPARSIS WHERE PSCOD='NOMEMPRESA' AND PSEST='A'";
            dtEmp = consulta.GetRow(sql);
            empresa = dtEmp["PSVAL"].ToString();           
            if (dtEmp == null)
            {
                resp.estado = false;
                resp.mensaje = "Error al consultar parametro de nombre de la empresa";
            }
            else
            {
                resp.estado = true;
                resp.respuesta = empresa;
            }

            return resp;

        }      

        internal DataTable consultarDestinos()
        {
            DataTable tablDest = new DataTable();
            ConBd consulta = new ConBd();
            sql = "SELECT CDCODDP, NOMCIUDAD, ESTADO " +
                   "FROM CIUDAD WHERE estado = 'A' ORDER BY nomciudad";
            tablDest = consulta.getTable(sql);
            return tablDest;

        }

        internal DataTable consultarVehiculos()
        {
            DataTable tablDest = new DataTable();
            ConBd consulta = new ConBd();
            sql = "SELECT PLACA,NUMINTERNO,ESTADO,CODEMPRESA " +
                   "FROM VEHICULOS WHERE estado = 'A' AND veproali ='False' ORDER BY placa";
            tablDest = consulta.getTable(sql);
            return tablDest;

        }

        internal DataTable consultarTarjetas()
        {
            DataTable tablDest = new DataTable();
            ConBd consulta = new ConBd();
            sql = "SELECT tarjeta,placa,TAESTCARD,TATIPTAR " +
                   "FROM vehiculos,GETARDET WHERE tarjeta = taidpk AND estado = 'A' ORDER BY tarjeta";
            tablDest = consulta.getTable(sql);
            return tablDest;

        }

        public RespLoguin horaServidor()
        {
            DataRow drHora = null;
            ConBd consulta = new ConBd();
            RespLoguin resp = new RespLoguin();
            string horasis = null;
            sql = $@"SELECT TO_CHAR(SYSDATE,'MM/DD/YYYY HH24:MI:SS') As HORASIS FROM dual";

            drHora = consulta.GetRow(sql);
            if (drHora == null)
            {
                resp.estado = false;
                resp.mensaje = "Error al consultar servicio, intente mas tarde";
            }
            else
            {
                resp.estado = true;
                resp.fecHora = drHora["HORASIS"].ToString();
            }
            return resp;
        }

        public RespLoguin consultarResolucion(string puerto)
        {
            DataRow drResolucion = null;
            ConBd consulta = new ConBd();
            RespLoguin resp = new RespLoguin();
            sql = $@"SELECT RFNUMRES, RFFECRES, RFNUMDES, RFNUMHAS,RFPREFIJ FROM FARESAGE WHERE RFCODCAJ='{puerto}'";

            drResolucion = consulta.GetRow(sql);
            if (drResolucion == null)
            {
                resp.estado = false;
                resp.mensaje = "Error al consultar servicio, intente mas tarde";
            }
            else
            {
                resp.estado = true;
                resp.respuesta = drResolucion["RFNUMRES"].ToString()+";" + drResolucion["RFFECRES"].ToString() + ";" + drResolucion["RFNUMDES"].ToString() + ";" + drResolucion["RFNUMHAS"].ToString() + ";" + drResolucion["RFPREFIJ"].ToString();
            }
            return resp;
        }

        public RespLoguin consultarNumAgencia(string puerto)
        {
              ConBd consulta = new ConBd();
            RespLoguin resp = new RespLoguin();
            string numAgencia = "";
            sql = $@"SELECT TOAGENCIA FROM LTTORNIQUETEM WHERE TOPUERTO='{puerto}'";

            numAgencia = consulta.GetValue(sql);
            if (numAgencia.Equals(""))
            {
                resp.estado = false;
                resp.mensaje = "Error al consultar servicio, intente mas tarde";
            }
            else
            {
                resp.estado = true;
                resp.respuesta = numAgencia;
            }
            return resp;
        }

        public RespLoguin consultarConsecutivos(string puerto)
        {
            ConBd consulta = new ConBd();
            RespLoguin resp = new RespLoguin();
            string numAgencia = consultarNumAgencia(puerto).respuesta;
            string consFac = "";
            string consRec = "";
            sql =   "SELECT MAX(FANUMFAC) "+ 
                    "FROM FAFACMAS "+
                    $@"WHERE FACODPRE = '{numAgencia}' "+ 
                    $@"AND FANUMCAJ= '{puerto}' AND FACODDOC='FAC'";

            consFac = consulta.GetValue(sql);

            sql = "SELECT MAX(FANUMFAC)"+ 
                    "FROM FAFACMAS "+
                    $@"WHERE FACODPRE = '{numAgencia}'" +
                    $@"AND FANUMCAJ = '{puerto}' AND FACODDOC = 'REC'";

            consRec = consulta.GetValue(sql);

            // Actualizamos consecutivos en el torniquete

            if (consFac.Equals(""))
            {
                consFac = "1";               
            }

            if (consRec.Equals(""))
            {
                consRec = "1";
            }
            

                sql = $@"UPDATE LTTORNIQUETEM SET TONUMFACT = '{consFac}' WHERE TOAGENCIA = '{numAgencia}' AND TOPUERTO = '{puerto}'";
                consulta.ExecuteQRY(sql);
                sql = $@"UPDATE LTTORNIQUETEM SET TONUMREC = '{consRec}' WHERE TOAGENCIA = '{numAgencia}' AND TOPUERTO = '{puerto}'";
                consulta.ExecuteQRY(sql);
                resp.estado = true;
                resp.respuesta = consFac+";"+consRec;
            
            return resp;
        }


        public RespLoguin consultarParametro(string parametro)
        {
            ConBd consulta = new ConBd();
            RespLoguin resp = new RespLoguin();
            string resPar = "";

            sql = "SELECT PSVAL " +
                    "FROM GEPARSIS " +
                    $@"WHERE PSCOD = '{parametro}' ";

            resPar = consulta.GetValue(sql);


            if (resPar.Equals(""))
            {
                resp.estado = false;
                resp.mensaje = "Error al consultar servicio, intente mas tarde";
            }
            else
            {
                resp.estado = true;
                resp.respuesta = resPar;
            }
            return resp;
        }

        public DataTable consultarCatalogo(string catalogo)
        {
            ConBd consulta = new ConBd();
            RespLoguin resp = new RespLoguin();
            DataTable resCat=null;

            sql = "SELECT DSDES FROM GEDETSUPTIP " +
                  "WHERE DSCODTIP = (SELECT STCODTIP FROM GESUPTIP WHERE STDES = '"+catalogo+"') " +
                   "AND GEDETSUPTIP.DSEST = 'A'";

            resCat = consulta.getTable(sql);

            return resCat;
        }

        internal DataTable consultarConceptos(string puerto)
        {
            String numAgencia = consultarNumAgencia(puerto).respuesta;
            DataTable tblConc = new DataTable();
            ConBd consulta = new ConBd();
            sql = "SELECT CFCODCON,CFNOMCON,DAVALCON,DAPORIVA,DAVALIVA FROM FADETAGE " +
                   "INNER JOIN FACONFAC ON DACODCON = CFCODCON "+
                   "WHERE DACODAGE= '"+ numAgencia + "' ORDER BY cfcodcon";
            tblConc = consulta.getTable(sql);
            return tblConc;

        }

        internal DataTable consultarPuntos()
        {
            DataTable tblConc = new DataTable();
            ConBd consulta = new ConBd();
            sql = "SELECT CODAGENCIA, CODCIUDAD, NOMAGENCIA, TOPUERTO,TOAGENASO " +
                  "FROM agencias, lttorniquetem " +
                  "WHERE modagencia = 'SPE' " +
                  "AND estado = 'A' " +
                  "AND toagencia = codagencia " +
                  "ORDER BY nomagencia";
            tblConc = consulta.getTable(sql);
            return tblConc;

        }

        internal DataTable consultarFrecuencias(string puerto)
        {
            DataTable tblConc = new DataTable();
            ConBd consulta = new ConBd();
            string agencia = consultarNumAgencia(puerto).respuesta;
            sql = "SELECT CTCODORI,CTCODAGE,CTHORINI,CTHORFIN,CTHORFRE FROM FACONTIE " +
                  "WHERE(CTNOMDIA = (SELECT TRANSLATE(TRIM(TO_CHAR(TO_DATE(SYSDATE), 'DAY', 'NLS_DATE_LANGUAGE=SPANISH')), 'ÁÉ', 'AE')  FROM DUAL) OR " +
                  "CTNOMDIA = (SELECT TRIM(TO_CHAR(TO_DATE(SYSDATE), 'DAY', 'NLS_DATE_LANGUAGE=SPANISH'))  FROM DUAL)) " +
                  $@"AND CTCODAGE = '{agencia}'";
            tblConc = consulta.getTable(sql);
            return tblConc;

        }

        internal string ejecISO(string puerto, string ISO, string parametro)
        { 
            ConBd consulta = new ConBd();
            return consulta.executeIso(ISO,parametro,puerto);
        }

        public DataTable consultarControles(string Interno, string Empresa)
        {
            DataTable tablaEmpresas = new DataTable();
            ConBd consulta = new ConBd();
            sql = $"SELECT * FROM FAREGTIE WHERE TRUNC(RTFECREG) BETWEEN TO_DATE('{DateTime.Now.Month.ToString("00")}/{DateTime.Now.Day.ToString("00")}/{DateTime.Now.Year}','MM/DD/YYYY') AND TO_DATE('{DateTime.Now.Month.ToString("00")}/{DateTime.Now.Day.ToString("00")}/{DateTime.Now.Year}','MM/DD/YYYY') AND RTNUMINT = '{Interno}' AND RTCODEMP ={Empresa} ORDER BY RTHORPTO ";
            tablaEmpresas = consulta.getTable(sql);
            return tablaEmpresas;
        }

    }
    }

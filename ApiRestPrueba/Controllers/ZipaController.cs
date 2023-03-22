using ApiZipaquira.logica;
using ApiZipaquira.Models;
using ApiZipaquira.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ApiZipaquira.Controllers

{

    [RoutePrefix("api/stszipa")]
    public class ZipaController : ApiController
    {
        [HttpGet]
        [Route("prueba")]
        public int prueba()
        {
            return 10;
            
        }
        RegistrarLog log = new RegistrarLog();
        /// <summary>
        /// Valida el usuario
        /// </summary>
        /// <param name="user">Datos de usuario</param>
        /// <returns></returns>
        /// <response estado>Si se realiza la consulta true o false</response>
        /// <response  mensaje>notificando la consulta</response>
        [Route("validaUsuario")]
        [HttpPost]
        public RespLoguin validaUsuario(Usuario usuario)
        {
            NegParq logica = new NegParq();
            RespLoguin resp = new RespLoguin();
            try
            {
                resp = logica.validaUsuario(usuario.nombre, usuario.contraseña);
            }
            catch (Exception ex)
            {
                log.registrar("stsZipa.ZipaController", "validaUsuario(Usuario usario)", 1, ex.Message + " " + ex.InnerException, 0);
                resp.estado = false;
                resp.mensaje = "Error del servidor al consultar los datos";
                return resp;

            }
            return resp;

        }

        /// <summary>
        /// Obtiene la lista de empresas
        /// </summary>
        /// <param name="serial">Serial movil</param>
        /// <param name="Puerto">Puerto Movil</param>
        /// <returns>lista de empresas</returns>
        /// <response code="400">sin datos</response>
        /// <response code="200">true</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Models.Empresa>))]
        [Route("descargaEmpresas")]
        public HttpResponseMessage descargaEmpresas(string Puerto)
        {         
            List<Models.Empresa> empresas = new List<Models.Empresa>();
            var req = Request.Properties;
            NegParq logica = new NegParq();
            DataTable dt1 = null;

            if (( Puerto == null) || (Puerto == ""))
            {
               
                return Request.CreateResponse(HttpStatusCode.BadRequest, "No se enviaron datos validos");
            }
            try
            {
                log.registrar("stsZipa.ZipaController", "descargaEmpresas",1, " PUERTO:" + Puerto, 0, Puerto != "" ? Puerto : "");

                dt1 = logica.consultarEmpresas();
                if (dt1 != null)
                {
                    foreach (DataRow dRow in dt1.Rows)
                    {
                        Models.Empresa emp = new Models.Empresa();
                        emp.codigo = dRow["CODEMPRE"].ToString();
                        emp.nombre = dRow["NOMEMPRE"].ToString();
                        emp.nit = dRow["NITEMPRE"].ToString();

                        empresas.Add(emp);
                    }
                }
            }
            catch (Exception ex)
            {
                log.registrar("stsZipa.ZipaController", "descargaEmpresas", 2,(ex.Message), 2, Puerto != "" ? Puerto : "");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            log.registrar("stsZipa.ZipaController", "descargaEmpresas", 3,"Empresas" ,2, Puerto != "" ? Puerto : "");
            return Request.CreateResponse(HttpStatusCode.OK, empresas);
        }

        /// <summary>
        /// Obtiene la lista de destinos
        /// </summary>
        /// <param name="serial">Serial movil</param>
        /// <param name="Puerto">Puerto Movil</param>
        /// <returns>lista de destinos</returns>
        /// <response code="400">sin datos</response>
        /// <response code="200">true</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Models.destinos>))]
        [Route("descargaDestinos")]
        public HttpResponseMessage descargaDestinos(string Puerto)
        {
            List<Models.destinos> destinos = new List<Models.destinos>();
            var req = Request.Properties;
            NegParq logica = new NegParq();
            DataTable dt1 = null;

            if ((Puerto == null) || (Puerto == ""))
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, "No se enviaron datos validos");
            }
            try
            {
                log.registrar("stsZipa.ZipaController", "descargaDestinos", 1, " PUERTO:" + Puerto, 0, Puerto != "" ? Puerto : "");

                dt1 = logica.consultarDestinos();
                if (dt1 != null)
                {
                    foreach (DataRow dRow in dt1.Rows)
                    {
                        Models.destinos des = new Models.destinos();
                        des.codigo = dRow["CDCODDP"].ToString();
                        des.nombre = dRow["NOMCIUDAD"].ToString();
                        des.estado = dRow["ESTADO"].ToString();

                        destinos.Add(des);
                    }
                }
            }
            catch (Exception ex)
            {
                log.registrar("stsZipa.ZipaController", "descargaDestinos", 2, (ex.Message), 2, Puerto != "" ? Puerto : "");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            log.registrar("stsZipa.ZipaController", "descargaDestinos", 3, "Destinos", 2, Puerto != "" ? Puerto : "");
            return Request.CreateResponse(HttpStatusCode.OK, destinos);
        }

        /// <summary>
        /// Obtiene la lista de vehiculos
        /// </summary>
        /// <param name="serial">Serial movil</param>
        /// <param name="Puerto">Puerto Movil</param>
        /// <returns>lista de empresas</returns>
        /// <response code="400">sin datos</response>
        /// <response code="200">true</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Models.Vehiculo>))]
        [Route("descargaVehiculos")]
        public HttpResponseMessage descargaVehiculos(string Puerto)
        {
            List<Models.Vehiculo> vehiculos = new List<Models.Vehiculo>();
            var req = Request.Properties;
            NegParq logica = new NegParq();
            DataTable dt1 = null;

            if ((Puerto == null) || (Puerto == ""))
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, "No se enviaron datos validos");
            }
            try
            {
                log.registrar("zipa", "descargaVehiculos", 1, " PUERTO:" + Puerto, 0, Puerto != "" ? Puerto : "");

                dt1 = logica.consultarVehiculos();
                if (dt1 != null)
                {
                    foreach (DataRow dRow in dt1.Rows)
                    {
                       
                            Models.Vehiculo veh = new Models.Vehiculo();
                            veh.placa = dRow["PLACA"].ToString();
                            veh.numInterno = dRow["NUMINTERNO"].ToString();
                            veh.codEmpresa = dRow["CODEMPRESA"].ToString();
                            veh.estado = dRow["ESTADO"].ToString();

                            vehiculos.Add(veh);
                        
                    }
                }
            }
            catch (Exception ex)
            {
                log.registrar("stsZipa.ZipaController", "descargaVehiculos", 2, (ex.Message), 2, Puerto != "" ? Puerto : "");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            log.registrar("stsZipa.ZipaController", "descargaVehiculos", 3, "Vehiculos", 2, Puerto != "" ? Puerto : "");
            return Request.CreateResponse(HttpStatusCode.OK, vehiculos);
        }

        /// <summary>
        /// Obtiene la lista de tarjetas
        /// </summary>
        /// <param name="serial">Serial movil</param>
        /// <param name="Puerto">Puerto Movil</param>
        /// <returns>lista de empresas</returns>
        /// <response code="400">sin datos</response>
        /// <response code="200">OK</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Models.Tarjeta>))]
        [Route("descargaTarjetas")]
        public HttpResponseMessage descargaTarjetas(string Puerto)
        {
            List<Models.Tarjeta> tarjetas = new List<Models.Tarjeta>();
            var req = Request.Properties;
            NegParq logica = new NegParq();
            DataTable dt1 = null;

            if ((Puerto == null) || (Puerto == ""))
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, "No se enviaron datos validos");
            }
            try
            {
                log.registrar("stsZipa.ZipaController", "descargaTarjeta", 1, " PUERTO:" + Puerto, 0, Puerto != "" ? Puerto : "");

                dt1 = logica.consultarTarjetas();
                int count = 0;
                if (dt1 != null)
                {
                    foreach (DataRow dRow in dt1.Rows)
                    {
                        
                        
                            Models.Tarjeta tar = new Models.Tarjeta();
                            tar.placa = dRow["placa"].ToString();
                            tar.id = dRow["tarjeta"].ToString();
                            tar.estado = dRow["TAESTCARD"].ToString();
                            tar.tipo = dRow["TATIPTAR"].ToString();

                            tarjetas.Add(tar);
                            count++;
                        
                        
                    }
                }
            }
            catch (Exception ex)
            {
                log.registrar("stsZipa.ZipaController", "descargaTarjeta", 2, (ex.Message), 2, Puerto != "" ? Puerto : "");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            log.registrar("stsZipa.ZipaController", "descargaTarjeta", 3, "Tarjetas", 2, Puerto != "" ? Puerto : "");
            return Request.CreateResponse(HttpStatusCode.OK, tarjetas);
        }

        /// <summary>
        /// Consulta las resoluciones de la factura
        /// </summary>
        /// <param name="user">Datos de usuario</param>
        /// <returns></returns>
        /// <response estado>Si se realiza la consulta true o false</response>
        /// <response  mensaje>Envia datos o mensaje de error:Error al consultar servicio, intente mas tarde</response>
        [Route("consultarResolucion")]
        [HttpGet]
        public RespLoguin consultarResolucion(string puerto)
        {
            NegParq logica = new NegParq();
            RespLoguin resp = new RespLoguin();
            try
            {
                resp = logica.consultarResolucion(puerto);
            }
            catch (Exception ex)
            {
                log.registrar("stsZipa.ZipaController", "consultarResolucion", 1, " PUERTO:" + puerto, 0, puerto != "" ? puerto : "");
                resp.estado = false;
                resp.mensaje = "Error del servidor al consultar los datos";
                return resp;

            }
            return resp;

        }

        /// <summary>
        /// Consulta el codigo de agencia en la tabla LTORNIQUETE
        /// </summary>
        /// <param name="user">Datos de usuario</param>
        /// <returns></returns>
        /// <response estado>Si se realiza la consulta true o false</response>
        /// <response  mensaje>actualización de la hora del sistema</response>
        [Route("consultarNumAgencia")]
        [HttpGet]
        public RespLoguin consultarNumAgencia(string puerto)
        {
            NegParq logica = new NegParq();
            RespLoguin resp = new RespLoguin();
            try
            {
                resp = logica.consultarNumAgencia(puerto);
            }
            catch (Exception ex)
            {

                
                resp.estado = false;
                log.registrar("stsZipa.ZipaController", "consultarNumAgencia", 1, " PUERTO:" + puerto, 0, puerto != "" ? puerto : "");
                return resp;

            }
            return resp;

        }

        /// <summary>
        /// Obtiene la lista de conceptos de cobro
        /// </summary>
        /// <param name="Puerto">Puerto Movil</param>
        /// <returns>lista de conceptos</returns>
        /// <response code="400">sin datos</response>
        /// <response code="200">truea</response>
        /// <response code="500">false</response>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Models.Concepto>))]
        [Route("descargaConceptos")]
        public HttpResponseMessage descargaConceptos(string Puerto)
        {
            List<Models.Concepto> conceptos = new List<Models.Concepto>();
            var req = Request.Properties;
            NegParq logica = new NegParq();
            DataTable dt1 = null;

            if ((Puerto == null) || (Puerto == ""))
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, "No se enviaron datos validos");
            }
            try
            {
                log.registrar("stsZipa.ZipaController", "descargaConceptos", 1, " PUERTO:" + Puerto, 0, Puerto != "" ? Puerto : "");

                dt1 = logica.consultarConceptos(Puerto);
                if (dt1 != null)
                {
                    foreach (DataRow dRow in dt1.Rows)
                    {
                        Models.Concepto concepto = new Models.Concepto();
                        concepto.codigo = dRow["CFCODCON"].ToString();
                        concepto.nombre = dRow["CFNOMCON"].ToString();
                        concepto.valTot = dRow["DAVALCON"].ToString();
                        concepto.porcentajeIva = dRow["DAPORIVA"].ToString();
                        concepto.valIva = dRow["DAVALIVA"].ToString();

                        conceptos.Add(concepto);
                    }
                }
            }
            catch (Exception ex)
            {
                log.registrar("stsZipa.ZipaController", "descargaConceptos", 2, (ex.Message), 2, Puerto != "" ? Puerto : "");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            log.registrar("stsZipa.ZipaController", "descargaConceptos", 3, "Conceptos", 2, Puerto != "" ? Puerto : "");
            return Request.CreateResponse(HttpStatusCode.OK, conceptos);
        }

        /// <summary>
        /// Obtiene la lista de puntos de venta
        /// </summary>
        /// <param name="serial">Serial movil</param>
        /// <param name="Puerto">Puerto Movil</param>
        /// <returns>lista de puntos de venta</returns>
        /// <response code="400">sin datos</response>
        /// <response code="200">true</response>
        /// <response code="500">false</response>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Models.puntoVenta>))]
        [Route("descargaPuntosVenta")]
        public HttpResponseMessage descargaPuntosVenta(string Puerto)
        {
            List<Models.puntoVenta> puntos = new List<Models.puntoVenta>();
            var req = Request.Properties;
            NegParq logica = new NegParq();
            DataTable dt1 = null;

            if ((Puerto == null) || (Puerto == ""))
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, "No se enviaron datos validos");
            }
            try
            {
                log.registrar("stsZipa.ZipaController", "descargaPuntosVenta", 1, " PUERTO:" + Puerto, 0, Puerto != "" ? Puerto : "");

                dt1 = logica.consultarPuntos();
                if (dt1 != null)
                {
                    foreach (DataRow dRow in dt1.Rows)
                    {
                        Models.puntoVenta punto = new Models.puntoVenta();
                        punto.codPunto = dRow["CODAGENCIA"].ToString();
                        punto.codCiudad = dRow["CODCIUDAD"].ToString();
                        punto.nombre = dRow["NOMAGENCIA"].ToString();
                        punto.puerto = dRow["TOPUERTO"].ToString();
                        punto.codAgenAso = dRow["TOAGENASO"].ToString();



                        puntos.Add(punto);
                    }
                }
            }
            catch (Exception ex)
            {
                log.registrar("stsZipa.ZipaController", "descargaConceptos", 2, (ex.Message), 2, Puerto != "" ? Puerto : "");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            log.registrar("stsZipa.ZipaController", "descargaConceptos", 3, "Conceptos", 2, Puerto != "" ? Puerto : "");
            return Request.CreateResponse(HttpStatusCode.OK, puntos);
        }

        /// <summary>
        /// Consulta los consecutivos de recibos y de facturas del puerto especificado
        /// </summary>
        /// <param name="puerto">Puerto</param>
        /// <returns></returns>
        /// <response estado>Si se realiza la consulta true o false</response>
        /// <response  mensaje>vacio o mensaje de error</response>
        [Route("consultarConsecutivos")]
        [HttpGet]
        public RespLoguin consultarConsecutivos(string puerto)
        {
            NegParq logica = new NegParq();
            RespLoguin resp = new RespLoguin();
            try
            {
                resp = logica.consultarConsecutivos(puerto);
            }
            catch (Exception ex)
            {
                log.registrar("stsZipa.ZipaController", "consultarConsecutivos", 1, " PUERTO:" + puerto, 0, puerto != "" ? puerto : "");
                resp.estado = false;
                resp.mensaje = "Error del servidor al consultar los datos";
                return resp;

            }
            return resp;

        }

        /// <summary>
        /// Consulta el parametro especificado de la base de datos
        /// </summary>
        /// <param name="parametro">parametro a buscar</param>
        /// <returns></returns>
        /// <response estado>Si se realiza la consulta true o false</response>
        /// <response  mensaje>vacio o mensaje de error</response>
        [Route("consultarParametro")]
        [HttpGet]
        public RespLoguin consultarParametro(string parametro)
        {
            NegParq logica = new NegParq();
            RespLoguin resp = new RespLoguin();
            try
            {
                resp = logica.consultarParametro(parametro);
            }
            catch (Exception ex)
            {
                log.registrar("stsZipa.ZipaController", "consultarParametro", 1, " parametro:" + parametro, 0, parametro != "" ? parametro : "");
                resp.estado = false;
                resp.mensaje = "Error del servidor al consultar los datos";
                return resp;

            }
            return resp;

        }

        /// <summary>
        /// Consulta los parametros correspondientes a la aplicacion
        /// </summary>
        /// <param name="puerto">puerto</param>
        /// <returns></returns>
        /// <response estado>Si se realiza la consulta true o false</response>
        /// <response  mensaje>vacio o mensaje de error</response>
        [Route("consultarParametros")]
        [HttpGet]
        public RespLoguin consultarParametros(string puerto)
        {
            NegParq logica = new NegParq();
            RespLoguin resp = new RespLoguin();
            try
            {
                string iva = logica.consultarParametro("PORCEIVA").respuesta;
                string actEcon = logica.consultarParametro("ACTECOFAC").respuesta;
                string conRecargas= logica.consultarParametro("CODCONREC").respuesta;
                string codControl = logica.consultarParametro("CODCONCT").respuesta;
                
                
                resp.estado = true;
                resp.respuesta = iva +";"+ actEcon+ ";" + conRecargas + ";" + codControl;
            }
            catch (Exception ex)
            {
                log.registrar("stsZipa.ZipaController", "consultarParametros", 1, " PUERTO:" + puerto, 0, puerto != "" ? puerto : "");
                resp.estado = false;
                resp.mensaje = "Error del servidor al consultar los datos";
                return resp;

            }
            return resp;

        }

        /// <summary>
        /// Obtiene la lista de frecuencias
        /// </summary>
        /// <param name="Puerto">puerto</param>
        /// <returns>lista de frecuencias</returns>
        /// <response code="400">sin datos</response>
        /// <response code="200">true</response>
        /// <response code="500">false</response>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Models.Frecuencia>))]
        [Route("descargaFrecuencias")]
        public HttpResponseMessage descargaFrecuencias(string Puerto)
        {
            List<Models.Frecuencia> frecuencias = new List<Models.Frecuencia>();
            var req = Request.Properties;
            NegParq logica = new NegParq();
            DataTable dt1 = null;

            if ((Puerto == null) || (Puerto == ""))
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, "No se enviaron datos validos");
            }
            try
            {
                log.registrar("stsZipa.ZipaController", "descargaFrecuencias", 1, " PUERTO:" + Puerto, 0, Puerto != "" ? Puerto : "");

                dt1 = logica.consultarFrecuencias(Puerto);
                if (dt1 != null)
                {
                    foreach (DataRow dRow in dt1.Rows)
                    {
                        Models.Frecuencia frecuencia = new Models.Frecuencia();
                        frecuencia.codOrigen = dRow["CTCODORI"].ToString();
                        frecuencia.codAgencia= dRow["CTCODAGE"].ToString();
                        frecuencia.horaIni = dRow["CTHORINI"].ToString();
                        frecuencia.horaFin = dRow["CTHORFIN"].ToString();
                        frecuencia.tiempFrec = dRow["CTHORFRE"].ToString();



                        frecuencias.Add(frecuencia);
                    }
                }
            }
            catch (Exception ex)
            {
                log.registrar("stsZipa.ZipaController", "descargaConceptos", 2, (ex.Message), 2, Puerto != "" ? Puerto : "");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            log.registrar("stsZipa.ZipaController", "descargaConceptos", 3, "Empresas", 2, Puerto != "" ? Puerto : "");
            return Request.CreateResponse(HttpStatusCode.OK, frecuencias);
        }

        /// <summary>
        /// Consulta datos de un catalogo de la base de datos
        /// </summary>
        /// <param name="catalogo">nombre del catalogo</param>
        /// <returns></returns>
        /// <response estado>Si se realiza la consulta true o false</response>
        /// <response  mensaje>actualización de la hora del sistema</response>
        [Route("consultarCatalogo")]
        [HttpGet]
        public RespLoguin consultarCatalogo(string catalogo)
        {
            NegParq logica = new NegParq();
            RespLoguin resp = new RespLoguin();
            DataTable dt1 = new DataTable();
            try
            {
                dt1 = logica.consultarCatalogo(catalogo);
                if (dt1 != null)
                {
                    String respuesta = "";
                    foreach (DataRow dRow in dt1.Rows)
                    {
                        respuesta = dRow["DSDES"].ToString() + ";";
                        resp.respuesta = respuesta + resp.respuesta;
                        ;

                    }

                }
            }
            catch (Exception ex)
            {
                log.registrar("stsZipa.ZipaController", "consultarCatalogo", 2, (ex.Message), 2, catalogo != "" ? catalogo : "");
                resp.estado = false;
                resp.mensaje = "Error en la consulta";
                return resp;
            }
            log.registrar("stsZipa.ZipaController", "consultarEmpNoCobro", 3, "EmpresasNoCobro", 2,"");
            resp.estado = true;
            return resp;

        }

        /// <summary>
        /// Ejecuta el iso que se envia como parametro
        /// </summary>
        /// <param name="DatosEnvio">puerto, iso y parametro</param>
        /// <returns></returns>
        /// <response estado>Si se realiza la consulta true o false</response>
        /// <response  mensaje>vacio si true o mensaje de error si false</response>
        [Route("ejecutaISO")]
        [HttpPost]
        public RespLoguin ejecutaISO(DatosEnvio datos)
        {
            NegParq logica = new NegParq();
            DataTable dt1 = null;
            RespLoguin resp = new RespLoguin();
            try
            {
                log.registrar("stsZipa.ZipaController", "ejecutaISO", 1, " PUERTO:" + datos.puerto, 0, datos.puerto != "" ? datos.puerto : "");

                string respuesta = logica.ejecISO(datos.puerto, datos.ISO, datos.parametro);
  
            }
            catch (Exception ex)
            {
                log.registrar("stsZipa.ZipaController", "ejecutaISO", 2, (ex.Message), 2, datos.puerto != "" ? datos.puerto : "");
                resp.estado = false;
                resp.mensaje = "Error en la consulta";
                return resp;
            }
            log.registrar("stsZipa.ZipaController", "ejecutaISO", 3, "EmpresasNoCobro", 2, datos.puerto != "" ? datos.puerto : "");
            resp.estado = true;
            return resp;

        }


        /// <summary>
        /// Consultar las empresas que no se realiza cobro de tarifa
        /// </summary>
        /// <param name="Puerto">puerto</param>
        /// <returns></returns>
        /// <response estado>Si se realiza la consulta true o false</response>
        /// <response  mensaje>actualización de la hora del sistema</response>
        [Route("consultarEmpNoCobro")]
        [HttpGet]
        public RespLoguin consultarEmpNoCobro(string Puerto)
        {
            NegParq logica = new NegParq();
            DataTable dt1 = null;
            RespLoguin resp = new RespLoguin();
            try
            {
                log.registrar("stsZipa.ZipaController", "consultarEmpNoCobro", 1, " PUERTO:" + Puerto, 0, Puerto != "" ? Puerto : "");

                dt1 = logica.consultarCatalogo("EMPRESAS EXCLUIDAS DE COBRO");
                if (dt1 != null)
                {
                    String respuesta = "";
                    foreach (DataRow dRow in dt1.Rows)
                    {
                        respuesta = dRow["DSDES"].ToString()+";";
                        resp.respuesta = respuesta + resp.respuesta;
                      ;
              
                    }
                    
                }
            }
            catch (Exception ex)
            {
                log.registrar("stsZipa.ZipaController", "consultarEmpNoCobro", 2, (ex.Message), 2, Puerto != "" ? Puerto : "");
                resp.estado = false;
                resp.mensaje="Error en la consulta";
                return resp;  
            }
            log.registrar("stsZipa.ZipaController", "stsZipa.ZipaController", 3, "EmpresasNoCobro", 2, Puerto != "" ? Puerto : "");
            resp.estado = true;
            return resp;

        }


        /// <summary>
        /// Consulta la hora del servidor
        /// </summary>
        /// <returns></returns>
        /// <response estado>Si se realiza la consulta true o false</response>
        /// <response  mensaje>actualización de la hora del sistema</response>
        [Route("horaServidor")]
        [HttpGet]
        public RespLoguin horaServidor()
        {
            NegParq logica = new NegParq();
            RespLoguin resp = new RespLoguin();
            try
            {
                resp = logica.horaServidor();
            }
            catch (Exception ex)
            {
                log.registrar("stsZipa.ZipaController", "horaServidor", 1, ex.Message + " " + ex.InnerException, 3);
                resp.estado = false;
                resp.mensaje = "Error del servidor al consultar los datos";
                return resp;

            }
            return resp;

        }

        [HttpGet]
        [Route("GetControles")]
        [ResponseType(typeof(IEnumerable<Control>))]
        public HttpResponseMessage GetControles(string Puerto, string Interno, string Empresa)
        {
            List<Control> controles = new List<Control>();
            var req = Request.Properties;
            NegParq logica = new NegParq();
            DataTable dt1 = null;

            if ((Interno == null) || (Interno == "") || (Interno == null) || (Interno == "") || (Empresa == null) || (Empresa == ""))
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, "No se enviaron datos validos");
            }
            try
            {
                log.registrar("stsZipa.ZipaController", "GetControles", 1, " PUERTO:" + Puerto, 0, Puerto != "" ? Puerto : "");

                dt1 = logica.consultarControles(Interno,Empresa);
                if (dt1 != null)
                {
                    foreach (DataRow dRow in dt1.Rows)
                    {
                        Control control = new Control();
                        control.placa = dRow["RTPLACA"].ToString();
                        control.agenciaOri = dRow["RTCODORI"].ToString();
                        control.fecha = dRow["RTFECREG"].ToString();
                        control.horaOrigen = dRow["RTHORORI"].ToString();
                        control.horaAgencia = dRow["RTHORPTO"].ToString();
                        control.demora = dRow["RTTIEDEM"].ToString();
                        control.frecuencia = dRow["RTFREREC"].ToString();
                        control.placaAnt = dRow["RTPLACAANT"].ToString();
                        control.fechaAnt = dRow["RTFECREGANT"].ToString();
                        control.horaOrigenAnt = dRow["RTHORORIANT"].ToString();
                        control.horaAgenciaAnt = dRow["RTHORPTOANT"].ToString();
                        control.demoraAnt = dRow["RTTIEDEMANT"].ToString();
                        control.codPto = dRow["RTCODPTO"].ToString();
                        controles.Add(control);
                    }
                }
            }
            catch (Exception ex)
            {
                log.registrar("stsZipa.ZipaController", "GetControles", 2, (ex.Message), 2, Puerto != "" ? Puerto : "");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            log.registrar("stsZipa.ZipaController", "GetControles", 3, "Controles", 2, Puerto != "" ? Puerto : "");
            return Request.CreateResponse(HttpStatusCode.OK, controles);
        }

    }
}

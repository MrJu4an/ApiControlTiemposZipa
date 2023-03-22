using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace ApiZipaquira.Utils
{
    public class RegistrarLog
    {
        //Se define las constantes para la ejecución del borrado de archivos logs
        const string HORAINICIO = "09:00:00";
        const string HORAFIN = "09:30:00";
        const int DIFERENCIA = 90;
        const string ASTERISK = "**************************************************";

        /// <summary>
        /// Guarda registro de log en archivo correspondiente
        /// </summary>
        /// <param name="espacio">Formulario, controlador, vista, 
        /// namespace o clase donde se genera el mensaje</param>
        /// <param name="funcion">funcion de la clase que genera el mensaje</param>
        /// <param name="paso">Paso de la funcion en al cual se genera el mensaje</param>
        /// <param name="mensaje">Mensaje que se desea registrar</param>
        /// <param name="tipo">indica el valor para colocar[*] (0) no, (1) inicio, (2) final, 
        /// (3) inicio y final</param>
        /// <param name="puerto">Puerto de la movil</param>
        public void registrar(string espacio, string funcion, int paso,
            string mensaje, int tipo, string puerto = "")
        {
            string año = DateTime.Now.Year.ToString(), mes = DateTime.Now.Month.ToString(),
                dia = DateTime.Now.Day.ToString(), segundo = DateTime.Now.Second.ToString(), ruta = "";
            string fecha = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            string hora = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
            try
            {
                Thread th = new Thread(new ParameterizedThreadStart(BorrarLog));

                var appPath = HttpContext.Current.Request.ApplicationPath;
                var physicalPath = HttpContext.Current.Request.MapPath(appPath);

                if (puerto != "")
                {
                    ruta = physicalPath + @"\Logs\Log_[" + puerto + "](" +
                        dia + " - " + mes + " - " + año + ").log";
                }
                else
                {
                    ruta = physicalPath + @"\Logs\Log_(" +
                        dia + " - " + mes + " - " + año + ").log";
                }
                // validamos que existe la ruta y se crea de no existir.
                if (Directory.Exists(physicalPath + @"\Logs") == false)
                {
                    Directory.CreateDirectory(physicalPath + @"\Logs");
                }

                if (Convert.ToDateTime(hora) > Convert.ToDateTime(HORAINICIO) &&
                    Convert.ToDateTime(hora) < Convert.ToDateTime(HORAFIN))
                {
                    // se valida el estado del hilo para no volverlo a ejecutar
                    if (th.ThreadState == ThreadState.Unstarted |
                        th.ThreadState == ThreadState.Aborted |
                        th.ThreadState == ThreadState.Stopped)
                    {
                        th.Start(physicalPath + @"\Logs");
                    }
                }

                var streamWriter = new StreamWriter(ruta, true);
                string cadena = ASTERISK + ASTERISK + ASTERISK + ASTERISK + ASTERISK + ASTERISK;
                string tab = " | ";

                // Escribe en el archivo de log.                
                switch (tipo)
                {
                    case 1:
                        streamWriter.WriteLine();
                        streamWriter.WriteLine(cadena);
                        break;
                    case 3:
                        streamWriter.WriteLine(cadena);
                        break;
                }


                streamWriter.WriteLine(fecha + tab + espacio + tab + funcion + tab +
                    "Paso:" + paso + tab + mensaje);

                if (tipo == 2)
                {
                    streamWriter.WriteLine(cadena);
                }

                if (tipo == 3)
                {
                    streamWriter.WriteLine(cadena);
                }

                streamWriter.Flush();
                streamWriter.Close();

            }
            catch (Exception ex)
            {
                var appPath = HttpContext.Current.Request.ApplicationPath;
                var physicalPath = HttpContext.Current.Request.MapPath(appPath);
                var file = new FileInfo(physicalPath + @"\Logs\");
                Directory.CreateDirectory(file.DirectoryName);
                registrar("LOGS", "registrarLog", 1, "Error al registar log en el archivo: " + ex.Message, 3);
            }


        }

        /// <summary>
        /// Borra los arhivos contenidos en la ruta enviada, 
        /// siempre y cuando el archivo tenga  fecha mayor a la constante DIFERENCIA
        /// </summary>
        /// <param name="ruta">Ruta del directorio</param>
        private void BorrarLog(object ruta)
        {
            try
            {
                DateTime fecha = DateTime.Now;

                foreach (string file in Directory.GetFiles(ruta.ToString()))
                {
                    DateTime fechaArchivo = File.GetLastWriteTime(file);
                    var diferencia = fechaArchivo.Subtract(fecha).TotalDays;
                    if (diferencia > DIFERENCIA)
                    {
                        File.Delete(file);
                    }

                }

            }
            catch (Exception ex)
            {
                registrar("LOGS", "BorarLog", 1, "Error al borrar el archivo: " + ex.Message, 3);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiZipaquira.Utils
{
    public class Seguridad
    {
        RegistrarLog log = new RegistrarLog();
        /// <summary>
        /// Descripta con el nuevo metodo el texto enviado
        /// </summary>
        /// <param name="cadena">texto a desencriptar</param>
        /// <returns>texto desencriptado</returns>
        public string DesencriptaNuevo(string cadena)
        {
            string datoEncriptado = "";
            int random = 0;
            int aux, numeric;
            int tamañoDato = cadena.Length - 1;
            char[] caracters = cadena.ToCharArray();
            try
            {

                random = Convert.ToInt32(caracters[tamañoDato].ToString());

                for (int i = 0; i <= ((cadena.Length) - 2); i++)
                {
                    tamañoDato -= 1;
                    char ca = caracters[tamañoDato];
                    numeric = (int)ca;

                    if ((cadena.Length % 2) == 0)
                    {

                        if ((i % 2) == 0)
                        {
                            aux = numeric - random;
                        }
                        else
                        {
                            aux = numeric + random;
                        }

                    }
                    else
                    {
                        if ((i % 2) == 0)
                        {
                            aux = numeric + random;
                        }
                        else
                        {
                            aux = numeric - random;
                        }
                    }
                    char d = Convert.ToChar(aux);
                    datoEncriptado += d;
                }
            }
            catch (Exception ex)
            {
                log.registrar("Seguridad", "newDecrypt", 1, ex.Message, 3);
            }
            return datoEncriptado;

        }

        /// <summary>
        /// Encripta con el nuevo metodo el texto enviado
        /// </summary>
        /// <param name="cadena">texto a encriptar</param>
        /// <returns>texto encriptado</returns>
        public string EncriptaNuevo(string cadena)
        {
            string datoEncriptado = "";
            int random = 0;
            int aux;
            int tamañoDato = cadena.Length;
            var rand = new Random();
            char[] caracters = cadena.ToCharArray();
            int numeric;
            try
            {
                random = Convert.ToInt32(rand.Next(1, 9) + 1);

                for (int i = 0; i <= cadena.Length - 1; i++)
                {
                    tamañoDato -= 1;
                    char ca = caracters[tamañoDato];
                    numeric = (int)ca;
                    if (i % 2 == 0)
                    {
                        aux = numeric + random;
                    }
                    else
                    {
                        aux = numeric - random;
                    }

                    if (i < cadena.Length - 1)
                    {
                        datoEncriptado += Convert.ToChar(aux);
                    }
                    else
                    {
                        datoEncriptado += Convert.ToChar(aux);
                        datoEncriptado += random;
                    }
                }
            }
            catch (Exception ex)
            {
                log.registrar("Seguridad", "encrypt", 1, ex.Message, 3);
            }
            return datoEncriptado;

        }

        /// <summary>
        /// Encripta con el metodo anterior el texto enviado 
        /// </summary>
        /// <param name="cadena">texto a encriptar</param>
        /// <returns>texto encriptado</returns>
        public string EncriptarAnterior(string cadena)
        {
            string resp = "";
            try
            {
                int j = 0, valor = 0;
                char[] dato = cadena.ToCharArray();
                for (int i = 0; i <= cadena.Length - 1; i++)
                {
                    valor = valor + Convert.ToInt32(dato[i]);
                }
                j = 5000 - valor + dato.Length;
                resp = "" + j;

            }
            catch (Exception ex)
            {
                log.registrar("Seguridad", "Descrypt_Anterior", 1, ex.Message, 3);
            }
            return resp;
        }

        /// <summary>
        /// Desencripta con el metodo anterior el texto enviado
        /// </summary>
        /// <param name="cadena">texto a desencriptar</param>
        /// <returns>texto desencriptado</returns>
        public string DesencriptaAnterior(string cadena)
        {
            string resp = "";
            try
            {
                int j = 0, valor = 0;
                char[] dato = cadena.ToCharArray();
                for (int i = 0; i <= cadena.Length - 1; i++)
                {
                    valor += Convert.ToChar(dato[i]);
                }
                j = 5000 - valor + dato.Length;
                resp = "" + j;

            }
            catch (Exception ex)
            {
                log.registrar("Seguridad", "Encrypt_anterior", 1, ex.Message, 3);
            }
            return resp;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiZipaquira.Models
{
    public class RespLoguin
    {
        public bool estado { get; set; }
        public string mensaje { get; set; }
        public string respuesta { get; set; }
        public string fecHora { get; set; }

        public string[] placas { get; set; }
        public String factura { get; set; }
    }
}
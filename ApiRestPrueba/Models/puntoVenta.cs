using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiZipaquira.Models
{
    public class puntoVenta
    {
        public string codPunto { get; set; }
        public string codCiudad { get; set; }
        public string nombre { get; set; }
        public string puerto { get; set; }
        public string codAgenAso { get; set; }
    }
}
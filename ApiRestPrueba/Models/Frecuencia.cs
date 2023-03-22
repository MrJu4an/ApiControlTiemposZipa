using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiZipaquira.Models
{
    public class Frecuencia
    {
        public string codOrigen { get; set; }
        public string codAgencia { get; set; }
        public string horaIni { get; set; }
        public string horaFin { get; set; }
        public string tiempFrec { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiZipaquira.Models
{
    public class model_data
    {
        public string CEDULA { get; set; }
        public string NOMTERCERO { get; set; }
        public string CUENTA { get; set; }
        public string NOMCUENTA { get; set; }
        public string MES { get; set; }

        public double VALORDEBITO { get; set; }
        public string VALORCREDITO { get; set; }
       

    }
}
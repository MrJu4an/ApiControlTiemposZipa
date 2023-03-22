namespace ApiZipaquira.Models
{
    public class Concepto
    {
        public string codigo { get; set; }
        public string nombre { get; set; }
        public string valTot { get; set; }
        public string porcentajeIva { get; set; }
        public string valIva { get; set; }
    }
    public class Control
    {
        public string placa { get; set; }

        public string agenciaOri { get; set; }

        public string fecha { get; set; }

        public string secuencia { get; set; }

        public string horaOrigen { get; set; }

        public string horaAgencia { get; set; }

        public string demora { get; set; }

        public string frecuencia { get; set; }

        public string enCadena { get; set; }

        public string placaAnt { get; set; }

        public string fechaAnt { get; set; }

        public string horaOrigenAnt { get; set; }

        public string horaAgenciaAnt { get; set; }

        public string demoraAnt { get; set; }

        public string codPto { get; set; }
    }
}
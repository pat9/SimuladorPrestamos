using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimuladorPrestamos.BO
{
    public class SimulacionBO
    {
        public int CodigoPrestamo { get; set; }
        public int CodigoCliente { get; set; }
        public double Monto { get; set; }
        public double TazaInteres { get; set; }
        public int PlazoPago { get; set; }
        public DateTime FechaIncio { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimuladorPrestamos.BO
{
    public class ClientesBO
    {
        public int CodigoCliente { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        
    }
}
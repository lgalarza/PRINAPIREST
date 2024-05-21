using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRINAPIREST.Models
{
    public class Menu
    {
        public short codigoEmpresa { get; set; }
        public string razonSocial { get; set; }
        public short codigoPerfil { get; set; }
        public string nombrePerfil { get; set; }
        public short codigoFuncion { get; set; }
        public string nombreFuncion { get; set; }
        public short codigoTransaccion { get; set; }
        public string nombreTransaccion { get; set; }
        public string actividad { get; set; }
    }
}

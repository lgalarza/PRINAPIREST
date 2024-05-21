using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRINAPIREST.Models
{
    public class Usuario
    {
        public string codigoUsuario { get; set; }
        public string numeroIdentificacion { get; set; }
        public string nombreUsuario { get; set; }
        public string apellidoUsuario { get; set; }
        public byte[] claveUsuario { get; set; }
        public string correoElectronico { get; set; }
        public string numeroTelefono { get; set; }
        public string codigoEmpresa { get; set; }
        public string razonSocial { get; set; }
        public string estadoUsuario { get; set; }
        public string codigoPerfil { get; set; }
        public string nombrePerfil { get; set; }
    }
}

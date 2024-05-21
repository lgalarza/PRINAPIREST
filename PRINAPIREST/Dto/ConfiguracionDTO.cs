using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRINAPIREST.Dto
{
    public class CatalogoDTO
    {
        public short? codigoCatalogo { get; set; }
        public string? nombreCatalogo { get; set; }
        public bool? estadoCatalogo { get; set; }
        public string? descripcionEstadoCatalogo { get; set; }
        public string? usuarioTransaccion { get; set; }
        public string? equipoTransaccion { get; set; }
        public string? opcion { get; set; }
    }

    public class DetalleCatalogoDTO
    {
        public short codigoDetalleCatalogo { get; set; }
        public short codigoCatalogo { get; set; }
        public string nombreDetalleCatalogo { get; set; }
        public decimal valoracion { get; set; }
        public short valoracion2 { get; set; }
        public string valoracion3 { get; set; }
        public bool valoracion4 { get; set; }
        public bool estadoDetalleCatalogo { get; set; }
        public string descripcionEstadoDetalleCatalogo { get; set; }
        public string usuarioTransaccion { get; set; }
        public string equipoTransaccion { get; set; }
        public string opcion { get; set; }
    }
}

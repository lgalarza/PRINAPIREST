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

    public class TipoCacaoDTO
    {
        public short? codigoTipoCacao { get; set; }
        public string? nombreTipoCacao { get; set; }
        public bool? estadoTipoCacao { get; set; }
        public string? descripcionEstadoTipoCacao { get; set; }
        
    }

    public class GruposDTO
    {
        public short? codigoGrupo { get; set; }
        public string? nombreGrupo { get; set; }
        public string? codigoZona { get; set; }
        public bool? estadoGrupo { get; set; }
        public string? descripcionEstadoGrupo { get; set; }
    }

    public class RespuestaDTO
    {
        private Int32 codigoError;
        private string mensajeError;
        private string body;
        public Int32 CodigoError { get => codigoError; set => codigoError = value; }
        public string? MensajeError { get => mensajeError; set => mensajeError = value; }
        public string Body { get => body; set => body = value; }

        public RespuestaDTO(Int32 codigoError, string mensajeError, string body)
        {
            this.codigoError = codigoError;
            this.mensajeError = mensajeError;
            this.body = body;
        }
    }
}

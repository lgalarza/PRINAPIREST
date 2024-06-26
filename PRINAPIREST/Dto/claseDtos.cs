using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRINAPIREST.Dto
{
    public class AsociarTicketGabetaDto
    {
        public string codigoAtencion { get; set; }
        public string codigoGabeta { get; set; }
    }
    public class CalidadDto
    {
        public string codigoAtencion { get; set; }
        public string codigoGabeta { get; set; }
    }
    public class LineasNotaPesoDto
    {
        public int rowNumber { get; set; }
        public string ticket { get; set; }
        public string codigoItem { get; set; }
        public string descripcionItem { get; set; }
        public int lineaNotaPeso { get; set; }
        public Int16 numeroPartida { get; set; }
        public string tipoCacao { get; set; }
        public decimal numeroSacos { get; set; }
        public decimal humedad { get; set; }
        public decimal buenFermento { get; set; }
        public decimal ligeroFermento { get; set; }
        public decimal pizarra { get; set; }
        public decimal violeta { get; set; }
        public decimal moho { get; set; }
        public decimal insecto { get; set; }
        public decimal peso100Pepas { get; set; }
        public decimal granosMultiples { get; set; }
        public decimal materialCacaoResiduos { get; set; }
        public decimal granoPlano { get; set; }
        public decimal materialextranio { get; set; }
        public decimal trituradoTamisado { get; set; }
        public decimal sedoHumedad { get; set; }
        public decimal sedoImpurezas { get; set; }
        public bool isExport{ get; set; }
        public string definitive { get; set; }
        public decimal granoQuebrado { get; set; }
        public bool itchgrass { get; set; }
        public int accionSecado { get; set; }
        public int accionImpureza { get; set; }
    }
    public class ListaLineas
    {
        public string usuario { get; set; }
        public string observacion { get; set; }

        public IList<LineasNotaPesoDto> LineasNotaPesoDto { get; set; }
    }
    public class UsuarioDto
    {
        public string codigo { get; set; }
        public string clave { get; set; }
    }
    public class ConsultaCalidadDto
    {
        public DateTime fechaInicial { get; set; }
        public DateTime fechaFinal { get; set; }
        public string codigo { get; set; }
    }
    public class InformeCalidadDto
    {
        public string codigoAtencion { get; set; }
        public string codigo { get; set; }
        public string tipoInforme { get; set; }
    }
    public class EvidenciaDto
    {
        public long rowNumber { get; set; }
        public long lineaNotaPeso { get; set; }
        public Int16 numeroPartida { get; set; }
        public Int16 sectionPhoto { get; set; }
        public string file { get; set; }
    }
    public class ListaEvidencia
    {
        public IList<EvidenciaDto> LineasEvidencia { get; set; }
    }

    public class LoginRespuestaDTO
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
        public bool estadoUsuario { get; set; }
        public string codigoPerfil { get; set; }
        public string nombrePerfil { get; set; }

    }

    public class PerfilDTO
    {
        public short codigoPerfil { get; set; }
        public string nombrePerfil { get; set; }
        public bool estadoPerfil { get; set; }
        public string descripcionEstadoPerfil { get; set; }

    }

}

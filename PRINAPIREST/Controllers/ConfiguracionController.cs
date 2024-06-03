using Microsoft.AspNetCore.Mvc;
using PRINAPIREST.Data;
using PRINAPIREST.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRINAPIREST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfiguracionController : Controller
    {
        private readonly ConfiguracionData _data;
        public ConfiguracionController(ConfiguracionData repositorio)
        {
            _data = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        [HttpPost("ObtenerCatalogoxNombre")]
        public async Task<IActionResult> ObtenerCatalogoxNombre(CatalogoDTO catalogo)
        {
            return Ok(await _data.obtenerCatalogoxNombre(catalogo));
        }

        #region Tipo de Cacao

        [HttpPost("MantenimientoObtenerTipoCacao")]
        public async Task<IActionResult> MantenimientoObtenerTipoCacao([FromBody] TipoCacaoDTO dato)
        {
            return Ok(await _data.mantenimientoObtenerTipoCacao(dato));

        }

        [HttpPost("MantenimientoGrabarTipoCacao")]
        public async Task<IActionResult> MantenimientoGrabarTipoCacao(TipoCacaoDTO dato)
        {
            return Ok(await _data.mantenimientoGrabarTipoCacao(dato));

        }

        [HttpPost("ObtenerTipoCacao")]
        public async Task<IActionResult> ObtenerTipoCacao()
        {
            return Ok(await _data.obtenerTipoCacao());
        }


        #endregion

        #region Grupos
        [HttpPost("MantenimientoObtenerGrupo")]
        public async Task<IActionResult> MantenimientoObtenerGrupo([FromBody] GruposDTO dato)
        {
            return Ok(await _data.mantenimientoObtenerGrupo(dato));

        }

        [HttpPost("MantenimientoGrabarGrupo")]
        public async Task<IActionResult> MantenimientoGrabarGrupo(GruposDTO dato)
        {
            return Ok(await _data.mantenimientoGrabarGrupo(dato));

        }

        [HttpPost("ObtenerGrupo")]
        public async Task<IActionResult> ObtenerGrupo()
        {
            return Ok(await _data.obtenerGrupo());
        }

        #endregion

        #region Certificacion
        [HttpPost("MantenimientoObtenerCertificacion")]
        public async Task<IActionResult> MantenimientoObtenerCertificacion([FromBody] CertificacionesDTO dato)
        {
            return Ok(await _data.mantenimientoObtenerCertificacion(dato));

        }

        [HttpPost("MantenimientoGrabarCertificacion")]
        public async Task<IActionResult> MantenimientoGrabarCertificacion(CertificacionesDTO dato)
        {
            return Ok(await _data.mantenimientoGrabarCertificacion(dato));

        }

        [HttpPost("ObtenerCertificacion")]
        public async Task<IActionResult> ObtenerCertificacion()
        {
            return Ok(await _data.obtenerCertificacion());
        }
        #endregion

        #region Programas
        [HttpPost("MantenimientoObtenerPrograma")]
        public async Task<IActionResult> MantenimientoObtenerPrograma([FromBody] ProgramasDTO dato)
        {
            return Ok(await _data.mantenimientoObtenerPrograma(dato));

        }

        [HttpPost("MantenimientoGrabarPrograma")]
        public async Task<IActionResult> MantenimientoGrabarPrograma(ProgramasDTO dato)
        {
            return Ok(await _data.mantenimientoGrabarPrograma(dato));

        }

        [HttpPost("ObtenerPrograma")]
        public async Task<IActionResult> ObtenerPrograma()
        {
            return Ok(await _data.obtenerPrograma());
        }

        #endregion

        #region Grupo Vendor Factura
        [HttpPost("MantenimientoObtenerGrupoVendorFactura")]
        public async Task<IActionResult> MantenimientoObtenerGrupoVendorFactura([FromBody] GrupoVendorFacturaDTO dato)
        {
            return Ok(await _data.mantenimientoObtenerGrupoVendorFactura(dato));

        }

        [HttpPost("MantenimientoGrabarVendorFactura")]
        public async Task<IActionResult> MantenimientoGrabarVendorFactura(GrupoVendorFacturaDTO dato)
        {
            return Ok(await _data.mantenimientoGrabarVendorFactura(dato));

        }
        #endregion

        #region Pronóstico Cosecha
        [HttpPost("MantenimientoObtenerPronosticoCosecha")]
        public async Task<IActionResult> MantenimientoObtenerPronosticoCosecha([FromBody] PronosticoCosechaDTO dato)
        {
            return Ok(await _data.mantenimientoObtenerPronosticoCosecha(dato));

        }

        [HttpPost("MantenimientoGrabarPronosticoCosecha")]
        public async Task<IActionResult> MantenimientoGrabarPronosticoCosecha(PronosticoCosechaDTO dato)
        {
            return Ok(await _data.mantenimientoGrabarPronosticoCosecha(dato));

        }
        #endregion
    }
}

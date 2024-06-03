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


    }
}

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
    }
}

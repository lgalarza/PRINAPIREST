using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRINAPIREST.Data;
using PRINAPIREST.Models;
using PRINAPIREST.Dto;

namespace PRINAPIREST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsociarTicketController : ControllerBase
    {
        private readonly PRINData _data;

        public AsociarTicketController(PRINData repositorio)
        {
            _data = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        [HttpGet("ObtenerListaGabetas")]
        public async Task<IActionResult> ObtenerListaGabetas()
        {
            var response = await _data.ObtenerListaGabetas();
            if (response == null)
                return NotFound();
            else
            {
                if (response.Count == 0)
                    return NotFound();
                else
                {
                    string respuestaJson = JsonConvert.SerializeObject(response);
                    return Ok(respuestaJson);
                }
            }
        }

        [HttpGet("ObtenerListaGabetasAsigandas")]
        public async Task<IActionResult> ObtenerListaGabetasAsigandas()
        {
            var response = await _data.ObtenerListaGabetasAsigandas();
            if (response == null)
                return NotFound();
            else
            {
                if (response.Count == 0)
                    return NotFound();
                else
                {
                    string respuestaJson = JsonConvert.SerializeObject(response);
                    return Ok(respuestaJson);
                }
            }
        }

        [HttpPost("ObtenerLineasNota")]
        public async Task<IActionResult> ObtenerLineasNota(AsociarTicketGabetaDto datos)
        {
            var response = await _data.ObtenerLineasNota(datos.codigoAtencion);
            if (response == null)
                return NotFound();
            else
            {
                if (response.Count == 0)
                    return NotFound();
                else
                {
                    string respuestaJson = JsonConvert.SerializeObject(response);
                    return Ok(respuestaJson);
                }
            }
        }

        [HttpPost("AsociarGabetaTicket")]
        public async Task<IActionResult> AsociarGabetaTicket(AsociarTicketGabetaDto datos)
        {
            var response = await _data.AsociarGabetaTicket(datos.codigoAtencion,datos.codigoGabeta);
            if (response.Equals(0))
                return NotFound();
            else
            {
                //string respuestaJson = JsonConvert.SerializeObject(response);
                return Ok();
            }
        }

        [HttpPost("ObtenerGabeta")]
        public async Task<IActionResult> ObtenerGabeta(AsociarTicketGabetaDto datos)
        {
            var response = await _data.ObtenerGabeta(datos.codigoGabeta);
            if (response == null)
                return NotFound("Gabeta se encuentra asignada");
            else
            {
                if (response.Count == 0)
                    return NotFound("Gabeta se encuentra asignada");
                else
                {
                    string respuestaJson = JsonConvert.SerializeObject(response);
                    return Ok(respuestaJson);
                }
            }
        }
    }

 }


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Newtonsoft.Json;
using PRINAPIREST.Data;
using PRINAPIREST.Models;
using PRINAPIREST.Dto;
using CalidadDominioDTO;
using static CalidadDominioDTO.SeguridadDTO;

namespace PRINAPIREST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeguridadController : ControllerBase
    {
        private readonly PRINData _data;

        public SeguridadController(PRINData repositorio)
        {
            _data = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        [HttpPost("ValidarUsuario")]
        public async Task<IActionResult> ValidarUsuario(UsuarioDto dtoUsuario)
        {
            var response = await _data.validarUsuario(dtoUsuario.codigo);
            string passwordCryp = "1234567891234567";
            //byte[] claveRecibida = Convert.FromBase64String(dtoUsuario.clave);
            //Encryptar _ecryptar = new Encryptar("1234567891234567");
            //string claveRecibidaString = _ecryptar.Decrypt(dtoUsuario.clave);
            string claveRecibidaString = Encoding.UTF8.GetString(B4XEncryption.B4XCipher.Decrypt(Convert.FromBase64String(dtoUsuario.clave), passwordCryp));
            byte[] claveUsuarioBD = response[0].claveUsuario;
            string claveString = SSG.Framework.Utilidades.ProteccionDatos.Desencriptar(claveUsuarioBD);

            if (response == null)
                return NotFound("Usuario o clave no existen");
            else
            {
                if (response.Count == 0)
                    return NotFound("Usuario o clave no existen");
                else
                {
                    //Compara la clave recibida con la obtenida y desincriptada de la base de datos
                    if (claveRecibidaString.Trim().Equals(claveString.Trim()))
                    {
                        string respuestaJson = JsonConvert.SerializeObject(response);
                        return Ok(respuestaJson);

                    }
                    else
                    {
                        return NotFound();
                    }

                }
            }
        }

        [HttpPost("ValidarUsuarioWeb")]
        public async Task<IActionResult> ValidarUsuarioWeb(UsuarioDto dtoUsuario)
        {
            var response = await _data.validarUsuarioWeb(dtoUsuario.codigo);
            var datoDevuelto = JsonConvert.DeserializeObject<LoginRespuestaDTO[]>(response.Body);
            //string passwordCryp = "1234567891234567";
            string claveRecibidaString = dtoUsuario.clave; //Encoding.UTF8.GetString(B4XEncryption.B4XCipher.Decrypt(Convert.FromBase64String(dtoUsuario.clave), passwordCryp));
            byte[] claveUsuarioBD = datoDevuelto[0].claveUsuario;
            string claveString = SSG.Framework.Utilidades.ProteccionDatos.Desencriptar(claveUsuarioBD);

            if (response == null)
                return NotFound("Usuario o clave no existen");
            else
            {
                if (response.CodigoError != 0)
                    return NotFound("Usuario o clave no existen");
                else
                {
                    //Compara la clave recibida con la obtenida y desincriptada de la base de datos
                    if (claveRecibidaString.Trim().Equals(claveString.Trim()))
                    {
                        string respuestaJson = JsonConvert.SerializeObject(response);
                        return Ok(respuestaJson);

                    }
                    else
                    {
                        return NotFound();
                    }

                }
            }
        }

        [HttpPost("ObtieneMenu")]
        public async Task<IActionResult> ObtieneMenu(UsuarioDto dtoUsuario)
        {
            var response = await _data.ObtieneMenu(dtoUsuario.codigo);
            if (response == null)
                return NotFound("Usuario no tiene permisos para ingresar a la App");
            else
            {
                if (response.Count == 0)
                    return NotFound("Usuario no tiene permisos para ingresar a la App");
                else
                {
                    string respuestaJson = JsonConvert.SerializeObject(response);
                    return Ok(respuestaJson);
                }
            }
        }

        [HttpPost("ObtieneMenuWeb")]
        public async Task<IActionResult> ObtieneMenuWeb(UsuarioDto dtoUsuario)
        {
            var response = await _data.ObtieneMenuWeb(dtoUsuario.codigo);
            if (response == null)
                return NotFound("Usuario no tiene permisos para ingresar a la plataforma");
            else
            {
                string respuestaJson = JsonConvert.SerializeObject(response);
                return Ok(respuestaJson);
            }
        }

    }
}

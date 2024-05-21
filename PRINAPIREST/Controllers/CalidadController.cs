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
using System.Text.Json;
using static System.Text.Json.JsonElement;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using AspNetCore.Reporting;
using System.Drawing;
using System.Drawing.Imaging;
using NPOI.HSSF.UserModel;

namespace PRINAPIREST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalidadController : ControllerBase
    {
        private readonly PRINData _data;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CalidadController(PRINData repositorio, IWebHostEnvironment webHostEnvironment)
        {
            _data = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("ObtenerLineasNotaGabeta")]
        public async Task<IActionResult> ObtenerLineasNotaGabeta(AsociarTicketGabetaDto datos)
        {
            var response = await _data.ObtenerLineasNotaGabeta(datos.codigoGabeta);
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

        [HttpGet("ObtenerListaTipoCacao")]
        public async Task<IActionResult> ObtenerListaTipoCacao()
        {
            var response = await _data.ObtenerListaTipoCacao();
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

        [HttpPost("GrabarCalidad")]
        public async Task<IActionResult> GrabarCalidad([FromBody] ListaLineas lstDatosRecibidos)
        {
            #region Setear Valores
            List<LineasNotaPeso> lstDatosEnviar = new List<LineasNotaPeso>();
            foreach (var datoLeer in lstDatosRecibidos.LineasNotaPesoDto)
            {
                LineasNotaPeso datoEnviar = new LineasNotaPeso();
                datoEnviar.rowNumber = datoLeer.rowNumber;
                datoEnviar.ticket = datoLeer.ticket;
                datoEnviar.codigoItem = datoLeer.codigoItem;
                datoEnviar.descripcionItem = datoLeer.descripcionItem;
                datoEnviar.lineaNotaPeso = datoLeer.lineaNotaPeso;
                datoEnviar.numeroPartida = datoLeer.numeroPartida;
                datoEnviar.tipoCacao = datoLeer.tipoCacao;
                datoEnviar.numeroSacos = datoLeer.numeroSacos;
                datoEnviar.humedad = datoLeer.humedad;
                datoEnviar.buenFermento = datoLeer.buenFermento;
                datoEnviar.ligeroFermento = datoLeer.ligeroFermento;
                datoEnviar.pizarra = datoLeer.pizarra;
                datoEnviar.violeta = datoLeer.violeta;
                datoEnviar.moho = datoLeer.moho;
                datoEnviar.insecto = datoLeer.insecto;
                datoEnviar.peso100Pepas = datoLeer.peso100Pepas;
                datoEnviar.granosMultiples = datoLeer.granosMultiples;
                datoEnviar.materialCacaoResiduos = datoLeer.materialCacaoResiduos;
                datoEnviar.granoPlano = datoLeer.granoPlano;
                datoEnviar.materialextranio = datoLeer.materialextranio;
                datoEnviar.trituradoTamisado = datoLeer.trituradoTamisado;
                datoEnviar.sedoHumedad = datoLeer.sedoHumedad;
                datoEnviar.sedoImpurezas = datoLeer.sedoImpurezas;
                datoEnviar.isExport= datoLeer.isExport;
                datoEnviar.granoQuebrado = datoLeer.granoQuebrado;
                datoEnviar.itchgrass = datoLeer.itchgrass;
                datoEnviar.accionSecado = datoLeer.accionSecado;
                datoEnviar.accionImpureza = datoLeer.accionImpureza;
                lstDatosEnviar.Add(datoEnviar);
            }
            #endregion
            var response = await _data.GrabarCalidad(lstDatosEnviar, lstDatosRecibidos.usuario, lstDatosRecibidos.observacion);
            if (response.Equals(0))
                return NotFound();
            else
            {
                //string respuestaJson = JsonConvert.SerializeObject(response);
                return Ok();
            }
        }

        [HttpPost("ObtieneListaCalidad")]
        public async Task<IActionResult> ObtieneListaCalidad(ConsultaCalidadDto dtoConsultaCalidad)
        {
            var response = await _data.ObtieneListaCalidad(dtoConsultaCalidad.fechaInicial, dtoConsultaCalidad.fechaFinal, dtoConsultaCalidad.codigo);
            if (response == null)
                return NotFound("No existen datos para el criterio de consulta ingresado");
            else
            {
                if (response.Count == 0)
                    return NotFound("No existen datos para el criterio de consulta ingresado");
                else
                {
                    string respuestaJson = JsonConvert.SerializeObject(response);
                    return Ok(respuestaJson);
                }
            }
        }

        [HttpPost("DescargarExcelRangoFecha")]
        public async Task<IActionResult> DescargarExcelRangoFecha(ConsultaCalidadDto dtoConsultaCalidad)
        {
            string base64String = string.Empty;
            var dsInformeCalidad = await _data.ObtieneReporteCalidadxRangoFechas(dtoConsultaCalidad.fechaInicial, dtoConsultaCalidad.fechaFinal);
            if (dsInformeCalidad == null)
                return NotFound("No existen datos");
            else
            {
                try
                {
                    HSSFWorkbook? FileXls = null;
                    clsNPOI writer = new clsNPOI();
                    FileXls = clsNPOI.DataTableToExcel(dsInformeCalidad.Tables[0]);
                    if (FileXls != null)
                    {
                        using (var memoria = new MemoryStream())
                        {
                            FileXls.Write(memoria);
                            base64String = Convert.ToBase64String(memoria.ToArray(), 0, memoria.ToArray().Length);
                        }
                    }
                    return Ok(base64String);
                }
                catch (Exception ex)
                {
                    string error = ex.Message.ToString();
                    return null;
                }

            }
        }

        [HttpPost("DescargarPdf")]
        public async Task<IActionResult> DescargarPdf(InformeCalidadDto datos)
        {
            string base64String = string.Empty;
            var dsInformeCalidad = await _data.ObtieneReporteCalidad(datos.codigoAtencion, datos.codigo);
            var dsHumedad = await _data.ObtieneImagenes(datos.codigoAtencion);
            if (dsInformeCalidad == null)
                return NotFound("No existen datos");
            else
            {
                try 
                {
                    if(datos.tipoInforme.Equals("I"))
                    {
                        string mimtype = "";
                        int extension = 1;
                        var path = $"{_webHostEnvironment.WebRootPath }\\Reports\\InformeCalidad.rdlc";
                        LocalReport informeCalidad = new LocalReport(path);
                        informeCalidad.AddDataSource("dsInformeCalidad", dsInformeCalidad.Tables[0]);
                        informeCalidad.AddDataSource("dsImagenes", dsHumedad.Tables[0]);
                        var result = informeCalidad.Execute(RenderType.Pdf, extension, null, mimtype);
                        //Para pruebas en PostMan
                        //return File(result.MainStream, "application/pdf");
                        //Para publicar el servicio
                        base64String = Convert.ToBase64String(result.MainStream, 0, result.MainStream.Length);
                    }
                    else
                    {
                        if(datos.tipoInforme.Equals("E"))
                        {
                            HSSFWorkbook? FileXls = null;
                            clsNPOI writer = new clsNPOI();
                            FileXls = clsNPOI.DataTableToExcel(dsInformeCalidad.Tables[0]);
                            if (FileXls != null)
                            {
                                using (var memoria = new MemoryStream())
                                {
                                    FileXls.Write(memoria);
                                    base64String = Convert.ToBase64String(memoria.ToArray(), 0, memoria.ToArray().Length);
                                }
                            }
                        }
                    }
                    return Ok(base64String);
                }
                catch (Exception ex)
                {
                    string error = ex.Message.ToString();
                    return null;
                }
               
            }
        }

        //crear otro metodo de consulta por fechas y que aplique a que el excel saque calidades de un rango de fechas


        [HttpPost("GrabarEvidencia")]
        public async Task<IActionResult> GrabarEvidencia([FromBody] ListaEvidencia lstDatosRecibidos)
        {
            #region Setear Valores
            List<Evidencia> lstDatosEnviar = new List<Evidencia>();
            foreach (var datoLeer in lstDatosRecibidos.LineasEvidencia)
            {
                Evidencia datoEnviar = new Evidencia();
                datoEnviar.rowNumber = datoLeer.rowNumber;
                datoEnviar.lineaNotaPeso = datoLeer.lineaNotaPeso;
                datoEnviar.numeroPartida = datoLeer.numeroPartida;
                datoEnviar.sectionPhoto = datoLeer.sectionPhoto;
                datoEnviar.file = datoLeer.file;
                lstDatosEnviar.Add(datoEnviar);
            }
            #endregion
            var rutaArchivo = $"{_webHostEnvironment.WebRootPath }\\Evidencia";

            var response = await _data.GrabarEvidencia(lstDatosEnviar);
            if (response.Equals(0))
                return NotFound();
            else
            {
                //string respuestaJson = JsonConvert.SerializeObject(response);
                return Ok();
            }
        }

        [HttpPost("ObtenerLineasNotaAtencion")]
        public async Task<IActionResult> ObtenerLineasNotaAtencion(AsociarTicketGabetaDto datos)
        {
            var response = await _data.ObtenerLineasNotaAtencion(datos.codigoAtencion);
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

        [HttpPost("ActualizarCalidad")]
        public async Task<IActionResult> ActualizarCalidad([FromBody] ListaLineas lstDatosRecibidos)
        {
            #region Setear Valores
            List<LineasNotaPeso> lstDatosEnviar = new List<LineasNotaPeso>();
            foreach (var datoLeer in lstDatosRecibidos.LineasNotaPesoDto)
            {
                LineasNotaPeso datoEnviar = new LineasNotaPeso();
                datoEnviar.rowNumber = datoLeer.rowNumber;
                datoEnviar.lineaNotaPeso = datoLeer.lineaNotaPeso;
                datoEnviar.numeroPartida = datoLeer.numeroPartida;
                datoEnviar.sedoHumedad = datoLeer.sedoHumedad;
                datoEnviar.sedoImpurezas = datoLeer.sedoImpurezas;
                datoEnviar.definitive = datoLeer.definitive;
                lstDatosEnviar.Add(datoEnviar);
            }
            #endregion
            var response = await _data.ActualizarCalidad(lstDatosEnviar);
            if (response.Equals(0))
                return NotFound();
            else
            {
                //string respuestaJson = JsonConvert.SerializeObject(response);
                return Ok();
            }
        }

        [HttpPost("ApruebaNegociacion")]
        public async Task<IActionResult> ApruebaNegociacion([FromBody] ListaLineas lstDatosRecibidos)
        {
            #region Setear Valores
            List<LineasNotaPeso> lstDatosEnviar = new List<LineasNotaPeso>();
            foreach (var datoLeer in lstDatosRecibidos.LineasNotaPesoDto)
            {
                LineasNotaPeso datoEnviar = new LineasNotaPeso();
                datoEnviar.rowNumber = datoLeer.rowNumber;
                datoEnviar.lineaNotaPeso = datoLeer.lineaNotaPeso;
                datoEnviar.numeroPartida = datoLeer.numeroPartida;
                datoEnviar.sedoHumedad = datoLeer.sedoHumedad;
                datoEnviar.sedoImpurezas = datoLeer.sedoImpurezas;
                datoEnviar.definitive = datoLeer.definitive;
                lstDatosEnviar.Add(datoEnviar);
            }
            #endregion
            var response = await _data.ApruebaNegociacion(lstDatosEnviar);
            if (response.Equals(0))
                return NotFound();
            else
            {
                //string respuestaJson = JsonConvert.SerializeObject(response);
                return Ok();
            }
        }

    }

}

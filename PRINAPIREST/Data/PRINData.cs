using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PRINAPIREST.Models;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using PRINAPIREST.Dto;

namespace PRINAPIREST.Data
{
    public class PRINData
    {
        private readonly string _cadenaConexion;

        public PRINData(IConfiguration configuracion)
        {
            _cadenaConexion = configuracion.GetConnectionString("defaultConnection");
        }

        public async Task<List<Gabetas>> ObtenerListaGabetas()
        {
            try
            {
                using SqlConnection sql = new SqlConnection(_cadenaConexion);
                using SqlCommand cmd = new SqlCommand("select boxCode, boxName from Gabeta where status='D'", sql);
                cmd.CommandType = System.Data.CommandType.Text;
                var response = new List<Gabetas>();
                await sql.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    #region Setear Valores
                    Gabetas itemGabeta = new Gabetas();
                    itemGabeta.codigoGabeta = reader["boxCode"].ToString();
                    itemGabeta.descripcionGabeta = reader["boxName"].ToString();
                    #endregion
                    response.Add(itemGabeta);
                }
                return response;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
        }

        public async Task<List<LineasNotaPeso>> ObtenerLineasNota(string codigoAtencion)
        {
            try
            {
                string strComando = string.Empty;
                strComando = "select wn.[Item No_] itemCode, it.Description itemName, wn.[Line No_] lineNumber from QualityService qa ";
                strComando = strComando + "join WeightNoteStaging wn on qa.[Movement No_] = wn.[Movement No_] join Item it on wn.[Item No_] = it.No_ ";
                strComando = strComando + "where qa.atentionCodeString = '" + codigoAtencion + "'";
                using SqlConnection sql = new SqlConnection(_cadenaConexion);
                using SqlCommand cmd = new SqlCommand(strComando, sql);
                cmd.CommandType = System.Data.CommandType.Text;
                var response = new List<LineasNotaPeso>();
                await sql.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    #region Setear Valores
                    LineasNotaPeso itemLinea = new LineasNotaPeso();
                    itemLinea.codigoItem = reader["itemCode"].ToString();
                    itemLinea.descripcionItem = reader["itemName"].ToString();
                    itemLinea.lineaNotaPeso = long.Parse(reader["lineNumber"].ToString());
                    #endregion
                    response.Add(itemLinea);
                }
                return response;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
        }

        public async Task<int> AsociarGabetaTicket(string codigoAtencion, string codigoGabeta)
        {
            using SqlConnection sql = new SqlConnection(_cadenaConexion);
            using SqlCommand cmd = new SqlCommand("addAnalisysBoxes", sql);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            await sql.OpenAsync();
            SqlTransaction sqlTransaccion = sql.BeginTransaction();
            cmd.Transaction = sqlTransaccion;
            try
            {
                cmd.Parameters.Add("@atentionCodeString", SqlDbType.VarChar, 30);
                cmd.Parameters["@atentionCodeString"].Value = codigoAtencion;
                cmd.Parameters.Add("@boxCode", SqlDbType.VarChar, 10);
                cmd.Parameters["@boxCode"].Value = codigoGabeta;
                var respuesta = await cmd.ExecuteNonQueryAsync();
                sqlTransaccion.Commit();
                return respuesta;
            }
            catch (SqlException ex)
            {
                try
                {
                    sqlTransaccion.Rollback();
                    throw new Exception(ex.Message.ToString());
                }
                catch (Exception ex2)
                {
                    throw new Exception(ex2.Message.ToString());
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                sql.Close();
            }
        }

        public async Task<List<Gabetas>> ObtenerListaGabetasAsigandas()
        {
            try
            {
                using SqlConnection sql = new SqlConnection(_cadenaConexion);
                using SqlCommand cmd = new SqlCommand("select boxCode, boxName from Gabeta where status='A'", sql);
                cmd.CommandType = System.Data.CommandType.Text;
                var response = new List<Gabetas>();
                await sql.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    #region Setear Valores
                    Gabetas itemGabeta = new Gabetas();
                    itemGabeta.codigoGabeta = reader["boxCode"].ToString();
                    itemGabeta.descripcionGabeta = reader["boxName"].ToString();
                    #endregion
                    response.Add(itemGabeta);
                }
                return response;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
        }

        public async Task<List<LineasNotaPeso>> ObtenerLineasNotaGabeta(string codigoGabeta)
        {
            try
            {
                string strComando = string.Empty;
                strComando = "select distinct qa.rowNumber rowNumber, qa.atentionCodeString ticket, wn.[Item No_] itemCode, it.Description itemName, wn.[Line No_] lineNumber, ab.[startingNumber],  wn.[Packing Quantity] numeroSacos from AnalisysBoxes ab ";
                strComando = strComando + "join QualityService qa on ab.rowNumber = qa.rowNumber join WeightNoteStaging wn on qa.[Movement No_] = wn.[Movement No_] and ab.lineNumber = wn.[Line No_] join Item it on wn.[Item No_] = it.No_ and qa.status = 'A' ";
                //strComando = strComando + "join Gabeta ga on ab.boxCode = ga.boxCode and ga.status = 'D' ";
                strComando = strComando + "where ab.boxCode = '" + codigoGabeta + "'";
                using SqlConnection sql = new SqlConnection(_cadenaConexion);
                using SqlCommand cmd = new SqlCommand(strComando, sql);
                cmd.CommandType = System.Data.CommandType.Text;
                var response = new List<LineasNotaPeso>();
                await sql.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    #region Setear Valores
                    LineasNotaPeso itemLinea = new LineasNotaPeso();
                    itemLinea.rowNumber = long.Parse(reader["rowNumber"].ToString());
                    itemLinea.ticket = reader["ticket"].ToString();
                    itemLinea.codigoItem = reader["itemCode"].ToString();
                    itemLinea.descripcionItem = reader["itemName"].ToString();
                    itemLinea.lineaNotaPeso = long.Parse(reader["lineNumber"].ToString());
                    itemLinea.numeroPartida = Int16.Parse(reader["startingNumber"].ToString());
                    itemLinea.tipoCacao = "";
                    itemLinea.numeroSacos = decimal.Parse(reader["numeroSacos"].ToString());
                    itemLinea.humedad = 0;
                    itemLinea.buenFermento = 0;
                    itemLinea.ligeroFermento = 0;
                    itemLinea.pizarra = 0;
                    itemLinea.violeta = 0;
                    itemLinea.moho = 0;
                    itemLinea.insecto = 0;
                    itemLinea.peso100Pepas = 0;
                    itemLinea.granosMultiples = 0;
                    itemLinea.materialCacaoResiduos = 0;
                    itemLinea.granoPlano = 0;
                    itemLinea.materialextranio = 0;
                    itemLinea.trituradoTamisado = 0;
                    itemLinea.sedoHumedad = 0;
                    itemLinea.sedoImpurezas = 0;
                    #endregion
                    response.Add(itemLinea);
                }
                return response;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
        }

        public async Task<List<LineasNotaPeso>> ObtenerLineasNotaAtencion(string codigoAtencion)
        {
            try
            {
                string strComando = string.Empty;
                strComando = "select distinct qa.rowNumber rowNumber, qa.atentionCodeString ticket, wn.[Item No_] itemCode, it.Description itemName, wn.[Line No_] lineNumber, ab.[startingNumber],  wn.[Packing Quantity] numeroSacos, ";
                strComando = strComando + "ab.typeCocoa, ab.humidity, ab.goodFerment, ab.slightFerment, ab.board, ab.purple, ab.mold, ab.insect, ab.weight100Seeds, ab.multipleGrains,";
                strComando = strComando + "ab.materialCocoaResidue, ab.flatGrain, ab.strangeMaterial, ab.crushed5mml, ab.silkMoisture, ab.silkImpurity,";
                strComando = strComando + "ab.totalFerment, ab.totalRating, ab.finalScore, ab.totalCut, ab.totalDefects, ab.[action], ab.destinationWineries,";
                strComando = strComando + "ab.observation, ab.processed, ab.isExport, ab.brokenGrain,isnull(ab.itchgrass, 0) itchgrass, isnull(ab.accionSecado, 0) accionSecado, isnull(ab.accionImpureza, 0) accionImpureza from QualityAnalisys ab ";
                strComando = strComando + "join QualityService qa on ab.rowNumber = qa.rowNumber join WeightNoteStaging wn on qa.[Movement No_] = wn.[Movement No_] and ab.lineNumber = wn.[Line No_] join Item it on wn.[Item No_] = it.No_ and qa.status = 'C' ";
                strComando = strComando + "where qa.atentionCodeString = '" + codigoAtencion + "' and ab.Definitive = 'P' ";
                using SqlConnection sql = new SqlConnection(_cadenaConexion);
                using SqlCommand cmd = new SqlCommand(strComando, sql);
                cmd.CommandType = System.Data.CommandType.Text;
                var response = new List<LineasNotaPeso>();
                await sql.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    #region Setear Valores
                    LineasNotaPeso itemLinea = new LineasNotaPeso();
                    itemLinea.rowNumber = long.Parse(reader["rowNumber"].ToString());
                    itemLinea.ticket = reader["ticket"].ToString();
                    itemLinea.codigoItem = reader["itemCode"].ToString();
                    itemLinea.descripcionItem = reader["itemName"].ToString();
                    itemLinea.lineaNotaPeso = long.Parse(reader["lineNumber"].ToString());
                    itemLinea.numeroPartida = Int16.Parse(reader["startingNumber"].ToString());
                    itemLinea.tipoCacao = reader["typeCocoa"].ToString(); 
                    itemLinea.numeroSacos = decimal.Parse(reader["numeroSacos"].ToString());
                    itemLinea.humedad = decimal.Parse(reader["humidity"].ToString());
                    itemLinea.buenFermento = decimal.Parse(reader["goodFerment"].ToString());
                    itemLinea.ligeroFermento = decimal.Parse(reader["slightFerment"].ToString());
                    itemLinea.pizarra = decimal.Parse(reader["board"].ToString());
                    itemLinea.violeta = decimal.Parse(reader["purple"].ToString());
                    itemLinea.moho = decimal.Parse(reader["mold"].ToString());
                    itemLinea.insecto = decimal.Parse(reader["insect"].ToString());
                    itemLinea.peso100Pepas = decimal.Parse(reader["weight100Seeds"].ToString());
                    itemLinea.granosMultiples = decimal.Parse(reader["multipleGrains"].ToString());
                    itemLinea.materialCacaoResiduos = decimal.Parse(reader["materialCocoaResidue"].ToString());
                    itemLinea.granoPlano = decimal.Parse(reader["flatGrain"].ToString());
                    itemLinea.materialextranio = decimal.Parse(reader["strangeMaterial"].ToString());
                    itemLinea.trituradoTamisado = decimal.Parse(reader["crushed5mml"].ToString());
                    itemLinea.sedoHumedad = decimal.Parse(reader["silkMoisture"].ToString());
                    itemLinea.sedoImpurezas = decimal.Parse(reader["silkImpurity"].ToString());
                    itemLinea.granoQuebrado = decimal.Parse(reader["brokenGrain"].ToString());
                    #endregion
                    response.Add(itemLinea);
                }
                return response;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
        }

        public async Task<List<TipoCacao>> ObtenerListaTipoCacao()
        {
            try
            {
                using SqlConnection sql = new SqlConnection(_cadenaConexion);
                using SqlCommand cmd = new SqlCommand("select typeCococaCode, typeCococaName from TypeCocoa", sql);
                cmd.CommandType = System.Data.CommandType.Text;
                var response = new List<TipoCacao>();
                await sql.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    #region Setear Valores
                    TipoCacao itemTipoCacao = new TipoCacao();
                    itemTipoCacao.codigoTipoCacao = reader["typeCococaCode"].ToString();
                    itemTipoCacao.nombreTipoCacao = reader["typeCococaName"].ToString();
                    #endregion
                    response.Add(itemTipoCacao);
                }
                return response;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
        }

        public async Task<int> GrabarCalidad(List<LineasNotaPeso> lstDatos, string usuario, string observacion)
        {
            int respuesta = 0;
            using SqlConnection sql = new SqlConnection(_cadenaConexion);
            using SqlCommand cmd = new SqlCommand("addQualityAnalisys", sql);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            await sql.OpenAsync();
            SqlTransaction sqlTransaccion = sql.BeginTransaction();
            cmd.Transaction = sqlTransaccion;
            try
            {
                foreach (LineasNotaPeso dato in lstDatos)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@rowNumber", SqlDbType.BigInt);
                    cmd.Parameters["@rowNumber"].Value = dato.rowNumber;
                    cmd.Parameters.Add("@lineNumber", SqlDbType.Int);
                    cmd.Parameters["@lineNumber"].Value = dato.lineaNotaPeso;
                    cmd.Parameters.Add("@startingNumber", SqlDbType.SmallInt);
                    cmd.Parameters["@startingNumber"].Value = dato.numeroPartida;
                    cmd.Parameters.Add("@typeCocoa", SqlDbType.VarChar,100);
                    cmd.Parameters["@typeCocoa"].Value = dato.tipoCacao;
                    cmd.Parameters.Add("@humidity", SqlDbType.Decimal);
                    cmd.Parameters["@humidity"].Value = dato.humedad;
                    cmd.Parameters.Add("@goodFerment", SqlDbType.Decimal);
                    cmd.Parameters["@goodFerment"].Value = dato.buenFermento;
                    cmd.Parameters.Add("@slightFerment", SqlDbType.Decimal);
                    cmd.Parameters["@slightFerment"].Value = dato.ligeroFermento;
                    cmd.Parameters.Add("@board", SqlDbType.Decimal);
                    cmd.Parameters["@board"].Value = dato.pizarra;
                    cmd.Parameters.Add("@purple", SqlDbType.Decimal);
                    cmd.Parameters["@purple"].Value = dato.violeta;
                    cmd.Parameters.Add("@mold", SqlDbType.Decimal);
                    cmd.Parameters["@mold"].Value = dato.moho;
                    cmd.Parameters.Add("@insect", SqlDbType.Decimal);
                    cmd.Parameters["@insect"].Value = dato.insecto;
                    cmd.Parameters.Add("@weight100Seeds", SqlDbType.Decimal);
                    cmd.Parameters["@weight100Seeds"].Value = dato.peso100Pepas;
                    cmd.Parameters.Add("@multipleGrains", SqlDbType.Decimal);
                    cmd.Parameters["@multipleGrains"].Value = dato.granosMultiples;
                    cmd.Parameters.Add("@materialCocoaResidue", SqlDbType.Decimal);
                    cmd.Parameters["@materialCocoaResidue"].Value = dato.materialCacaoResiduos;
                    cmd.Parameters.Add("@flatGrain", SqlDbType.Decimal);
                    cmd.Parameters["@flatGrain"].Value = dato.granoPlano;
                    cmd.Parameters.Add("@strangeMaterial", SqlDbType.Decimal);
                    cmd.Parameters["@strangeMaterial"].Value = dato.materialextranio;
                    cmd.Parameters.Add("@crushed5mml", SqlDbType.Decimal);
                    cmd.Parameters["@crushed5mml"].Value = dato.trituradoTamisado;
                    cmd.Parameters.Add("@silkMoisture", SqlDbType.Decimal);
                    cmd.Parameters["@silkMoisture"].Value = dato.sedoHumedad;
                    cmd.Parameters.Add("@silkImpurity", SqlDbType.Decimal);
                    cmd.Parameters["@silkImpurity"].Value = dato.sedoImpurezas;
                    cmd.Parameters.Add("@observation", SqlDbType.VarChar, 250);
                    cmd.Parameters["@observation"].Value = observacion;
                    cmd.Parameters.Add("@creatorUser", SqlDbType.VarChar,20);
                    cmd.Parameters["@creatorUser"].Value = usuario;
                    cmd.Parameters.Add("@isExport", SqlDbType.Bit);
                    cmd.Parameters["@isExport"].Value = dato.isExport;
                    cmd.Parameters.Add("@packingQuantity", SqlDbType.Decimal);
                    cmd.Parameters["@packingQuantity"].Value = dato.numeroSacos;
                    cmd.Parameters.Add("@brokenGrain", SqlDbType.Decimal);
                    cmd.Parameters["@brokenGrain"].Value = dato.granoQuebrado;
                    cmd.Parameters.Add("@itchgrass", SqlDbType.Bit);
                    cmd.Parameters["@itchgrass"].Value = dato.itchgrass;
                    cmd.Parameters.Add("@accionSecado", SqlDbType.SmallInt);
                    cmd.Parameters["@accionSecado"].Value = dato.accionSecado;
                    cmd.Parameters.Add("@accionImpureza", SqlDbType.SmallInt);
                    cmd.Parameters["@accionImpureza"].Value = dato.accionImpureza;


                    respuesta = await cmd.ExecuteNonQueryAsync();
                }
                sqlTransaccion.Commit();
                return respuesta;
            }
            catch (SqlException ex)
            {
                try
                {
                    sqlTransaccion.Rollback();
                    throw new Exception(ex.Message.ToString());
                }
                catch (Exception ex2)
                {
                    throw new Exception(ex2.Message.ToString());
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                sql.Close();
            }
        }

        public async Task<List<Gabetas>> ObtenerGabeta(string codigoGabeta)
        {
            try
            {
                using SqlConnection sql = new SqlConnection(_cadenaConexion);
                using SqlCommand cmd = new SqlCommand("select boxCode, boxName from Gabeta where status='D' and boxCodeCodeString = '" + codigoGabeta + "'", sql);
                cmd.CommandType = System.Data.CommandType.Text;
                var response = new List<Gabetas>();
                await sql.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    #region Setear Valores
                    Gabetas itemGabeta = new Gabetas();
                    itemGabeta.codigoGabeta = reader["boxCode"].ToString();
                    itemGabeta.descripcionGabeta = reader["boxName"].ToString();
                    #endregion
                    response.Add(itemGabeta);
                }
                return response;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
        }

        public async Task<List<Usuario>> validarUsuario(string codigoUsuario)
        {
            try
            {
                using SqlConnection sql = new SqlConnection(_cadenaConexion);
                using SqlCommand cmd = new SqlCommand("existeUsuario", sql);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@codigoUsuario", SqlDbType.VarChar, 20);
                cmd.Parameters["@codigoUsuario"].Value = codigoUsuario;
                cmd.Parameters.Add("@mostrarMensaje", SqlDbType.Bit);
                cmd.Parameters["@mostrarMensaje"].Value = false;
                var response = new List<Usuario>();
                await sql.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    response.Add(setarValores(reader));
                }
                return response;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
        }

        public async Task<RespuestaDTO> validarUsuarioWeb(string codigoUsuario)
        {
            try
            {
                using SqlConnection sql = new SqlConnection(_cadenaConexion);
                using SqlCommand cmd = new SqlCommand("existeUsuarioWeb", sql);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@codigoUsuario", SqlDbType.VarChar, 20);
                cmd.Parameters["@codigoUsuario"].Value = codigoUsuario;
                cmd.Parameters.Add("@co_msg", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@ds_msg", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output;


                await sql.OpenAsync();
                var reader = await cmd.ExecuteReaderAsync();
                DataTable dtDatos = new DataTable();
                dtDatos.Load(reader);
                reader.Close();

                return new RespuestaDTO(
                Convert.ToInt32(cmd.Parameters["@co_msg"].Value),
                cmd.Parameters["@ds_msg"].Value.ToString(),
                JsonConvert.SerializeObject(dtDatos)
                );
              
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
        }

        private Usuario setarValores(SqlDataReader reader)
        {
            return new Usuario()
            {
                codigoUsuario = reader["codigoUsuario"].ToString(),
                numeroIdentificacion = reader["numeroIdentificacion"].ToString(),
                nombreUsuario = reader["nombreUsuario"].ToString(),
                apellidoUsuario = reader["apellidoUsuario"].ToString(),
                claveUsuario = (byte[])reader["claveUsuario"],
                correoElectronico = reader["correoElectronico"].ToString(),
                numeroTelefono = reader["numeroTelefono"].ToString(),
                codigoEmpresa = reader["codigoEmpresa"].ToString(),
                razonSocial = reader["razonSocial"].ToString(),
                estadoUsuario = reader["estadoUsuario"].ToString(),
                codigoPerfil = reader["codigoPerfil"].ToString(),
                nombrePerfil = reader["nombrePerfil"].ToString()
            };
        }

        public async Task<List<Menu>> ObtieneMenu(string codigoUsuario)
        {
            try
            {
                using SqlConnection sql = new SqlConnection(_cadenaConexion);
                using SqlCommand cmd = new SqlCommand("usuarioAccede", sql);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@codigoUsuario", SqlDbType.VarChar, 20);
                cmd.Parameters["@codigoUsuario"].Value = codigoUsuario;
                cmd.Parameters.Add("@codigoDetalleCatalogo", SqlDbType.SmallInt);
                cmd.Parameters["@codigoDetalleCatalogo"].Value = 3;
                var response = new List<Menu>();
                await sql.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    #region Setear Valores
                    Menu itemMenu = new Menu();
                    itemMenu.codigoEmpresa = short.Parse(reader["codigoEmpresa"].ToString());
                    itemMenu.razonSocial = reader["razonSocial"].ToString();
                    itemMenu.codigoPerfil = short.Parse(reader["codigoPerfil"].ToString());
                    itemMenu.nombrePerfil = reader["nombrePerfil"].ToString();
                    itemMenu.codigoFuncion = short.Parse(reader["codigoFuncion"].ToString());
                    itemMenu.nombreFuncion = reader["nombreFuncion"].ToString();
                    itemMenu.codigoTransaccion = short.Parse(reader["codigoTransaccion"].ToString());
                    itemMenu.nombreTransaccion = reader["nombreTransaccion"].ToString();
                    itemMenu.actividad = reader["urlPagina"].ToString();
                    #endregion
                    response.Add(itemMenu);
                }
                return response;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
        }

        public async Task<RespuestaDTO> ObtieneMenuWeb(string codigoUsuario)
        {
            try
            {
                using SqlConnection sql = new SqlConnection(_cadenaConexion);
                using SqlCommand cmd = new SqlCommand("usuarioAccedeWeb", sql);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@codigoUsuario", SqlDbType.VarChar, 20);
                cmd.Parameters["@codigoUsuario"].Value = codigoUsuario;
                cmd.Parameters.Add("@codigoDetalleCatalogo", SqlDbType.SmallInt);
                cmd.Parameters["@codigoDetalleCatalogo"].Value = 4;
                cmd.Parameters.Add("@co_msg", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@ds_msg", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output;

                await sql.OpenAsync();
                var reader = await cmd.ExecuteReaderAsync();
                DataTable dtDatos = new DataTable();
                dtDatos.Load(reader);
                reader.Close();

                return new RespuestaDTO(
                Convert.ToInt32(cmd.Parameters["@co_msg"].Value),
                cmd.Parameters["@ds_msg"].Value.ToString(),
                JsonConvert.SerializeObject(dtDatos)
                );
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
        }

        public async Task<List<CalidadServicio>> ObtieneListaCalidad(DateTime fechaInicial, DateTime fechaFinal, string codigoUsuario)
        {
            try
            {
                using SqlConnection sql = new SqlConnection(_cadenaConexion);
                using SqlCommand cmd = new SqlCommand("getQualityService", sql);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@starDate", SqlDbType.DateTime);
                cmd.Parameters["@starDate"].Value = fechaInicial;
                cmd.Parameters.Add("@endDate", SqlDbType.DateTime);
                cmd.Parameters["@endDate"].Value = fechaFinal;
                cmd.Parameters.Add("@creatorUser", SqlDbType.VarChar, 20);
                cmd.Parameters["@creatorUser"].Value = codigoUsuario;
                var response = new List<CalidadServicio>();
                await sql.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    #region Setear Valores
                    CalidadServicio itemCalidad = new CalidadServicio();
                    itemCalidad.codigoAtencion = reader["CodigoAtencion"].ToString();
                    itemCalidad.calificador = reader["Calificador"].ToString();
                    //DateTime dtFecha = DateTime.ParseExact(reader["Fecha"].ToString(),"yyyy/MM/dd", CultureInfo.InvariantCulture);
                    itemCalidad.fecha = reader["Fecha"].ToString();
                    //DateTime dtHora = DateTime.ParseExact(reader["Fecha"].ToString(),"H:m:s",null);
                    itemCalidad.hora = reader["Hora"].ToString(); 
                    #endregion
                    response.Add(itemCalidad);
                }
                return response;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
        }

        public async Task<DataSet> ObtieneReporteCalidadxRangoFechas(DateTime fechaInicial, DateTime fechaFinal)
        {
            try
            {
                using SqlConnection sql = new SqlConnection(_cadenaConexion);
                using SqlCommand cmd = new SqlCommand("getDataQualityServicebyDate", sql);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@startDate", SqlDbType.DateTime);
                cmd.Parameters["@startDate"].Value = fechaInicial;
                cmd.Parameters.Add("@endDate", SqlDbType.DateTime);
                cmd.Parameters["@endDate"].Value = fechaFinal;

                using SqlDataAdapter da = new SqlDataAdapter(cmd);
                using DataSet dsDatos = new DataSet("dsInformeCalidad");
                //da.Fill(dsDatos);
                await Task.Run(() => da.Fill(dsDatos));
                return dsDatos;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
        }

        public async Task<DataSet> ObtieneReporteCalidad(string codigoAtencion, string codigoUsuario)
        {
            try
            {
                using SqlConnection sql = new SqlConnection(_cadenaConexion);
                using SqlCommand cmd = new SqlCommand("getDataQualityService", sql);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@atentionCodeString", SqlDbType.VarChar,30);
                cmd.Parameters["@atentionCodeString"].Value = codigoAtencion;
                cmd.Parameters.Add("@creatorUser", SqlDbType.VarChar, 20);
                cmd.Parameters["@creatorUser"].Value = codigoUsuario;

                using SqlDataAdapter da = new SqlDataAdapter(cmd);
                using DataSet dsDatos = new DataSet("dsInformeCalidad");
                //da.Fill(dsDatos);
                await Task.Run(() => da.Fill(dsDatos));
                return dsDatos;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
        }

        public async Task<DataSet> ObtieneImagenes(string codigoAtencion)
        {
            try
            {
                using SqlConnection sql = new SqlConnection(_cadenaConexion);
                using SqlCommand cmd = new SqlCommand("getDataEvidence", sql);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@atentionCodeString", SqlDbType.VarChar, 30);
                cmd.Parameters["@atentionCodeString"].Value = codigoAtencion;
                using SqlDataAdapter da = new SqlDataAdapter(cmd);
                string dataset = "dsImagenes";
                using DataSet dsDatos = new DataSet(dataset);
                //da.Fill(dsDatos);
                await Task.Run(() => da.Fill(dsDatos));
                return dsDatos;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
        }

        public async Task<DataSet> obtieneCalificacion(string codigoAtencion)
        {
            try
            {
               
                using DataSet dsDatos = new DataSet();
                //da.Fill(dsDatos);
               // await Task.Run(() => da.Fill(dsDatos));
                return dsDatos;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
        }

        public async Task<int> GrabarEvidencia(List<Evidencia> lstDatos)
        {
            int respuesta = 0;
            using SqlConnection sql = new SqlConnection(_cadenaConexion);
            using SqlCommand cmd = new SqlCommand("addEvidenceAnalisys", sql);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            await sql.OpenAsync();
            SqlTransaction sqlTransaccion = sql.BeginTransaction();
            cmd.Transaction = sqlTransaccion;
            /*
            if (!Directory.Exists(rutaArchivo))
            {
                Directory.CreateDirectory(rutaArchivo);
            }
            */
            try
            {
                Int16 secuencial = 1;
                foreach (Evidencia dato in lstDatos)
                {
                    string NombreArchivo = dato.rowNumber.ToString() + "_" + dato.sectionPhoto.ToString() + "_" + DateTime.Now.ToString("dd-MM-yyyy") + "_" + secuencial.ToString() + ".jpg";
                    //string RutaWebCompleta = rutaArchivo + "\\" + NombreArchivo;

                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@rowNumber", SqlDbType.BigInt);
                    cmd.Parameters["@rowNumber"].Value = dato.rowNumber;
                    cmd.Parameters.Add("@lineNumber", SqlDbType.Int);
                    cmd.Parameters["@lineNumber"].Value = dato.lineaNotaPeso;
                    cmd.Parameters.Add("@startingNumber", SqlDbType.SmallInt);
                    cmd.Parameters["@startingNumber"].Value = dato.numeroPartida;
                    cmd.Parameters.Add("@sequential", SqlDbType.Int);
                    cmd.Parameters["@sequential"].Value = secuencial;
                    cmd.Parameters.Add("@sectionPhoto", SqlDbType.SmallInt);
                    cmd.Parameters["@sectionPhoto"].Value = dato.sectionPhoto;
                    cmd.Parameters.Add("@file", SqlDbType.VarChar);
                    cmd.Parameters["@file"].Value = dato.file;

                    string base64imageString = dato.file;

                    //Funcion para grabar la imagen a base 64
                    //File.WriteAllBytes(RutaWebCompleta, Convert.FromBase64String(base64imageString));

                    
                    respuesta = await cmd.ExecuteNonQueryAsync();
                    secuencial += 1;
                }
                sqlTransaccion.Commit();
                return respuesta;
            }
            catch (SqlException ex)
            {
                try
                {
                    sqlTransaccion.Rollback();
                    throw new Exception(ex.Message.ToString());
                }
                catch (Exception ex2)
                {
                    throw new Exception(ex2.Message.ToString());
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                sql.Close();
            }
        }

        public async Task<int> ActualizarCalidad(List<LineasNotaPeso> lstDatos)
        {
            int respuesta = 0;
            using SqlConnection sql = new SqlConnection(_cadenaConexion);
            using SqlCommand cmd = new SqlCommand("updQualityAnalisys", sql);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            await sql.OpenAsync();
            SqlTransaction sqlTransaccion = sql.BeginTransaction();
            cmd.Transaction = sqlTransaccion;
            try
            {
                foreach (LineasNotaPeso dato in lstDatos)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@rowNumber", SqlDbType.BigInt);
                    cmd.Parameters["@rowNumber"].Value = dato.rowNumber;
                    cmd.Parameters.Add("@lineNumber", SqlDbType.Int);
                    cmd.Parameters["@lineNumber"].Value = dato.lineaNotaPeso;
                    cmd.Parameters.Add("@startingNumber", SqlDbType.SmallInt);
                    cmd.Parameters["@startingNumber"].Value = dato.numeroPartida;
                    cmd.Parameters.Add("@silkMoisture", SqlDbType.Decimal);
                    cmd.Parameters["@silkMoisture"].Value = dato.sedoHumedad;
                    cmd.Parameters.Add("@silkImpurity", SqlDbType.Decimal);
                    cmd.Parameters["@silkImpurity"].Value = dato.sedoImpurezas;
                    cmd.Parameters.Add("@definitive", SqlDbType.Char,1);
                    cmd.Parameters["@definitive"].Value = dato.definitive;

                    respuesta = await cmd.ExecuteNonQueryAsync();
                }
                sqlTransaccion.Commit();
                return respuesta;
            }
            catch (SqlException ex)
            {
                try
                {
                    sqlTransaccion.Rollback();
                    throw new Exception(ex.Message.ToString());
                }
                catch (Exception ex2)
                {
                    throw new Exception(ex2.Message.ToString());
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                sql.Close();
            }
        }
        public async Task<int> ApruebaNegociacion(List<LineasNotaPeso> lstDatos)
        {
            int respuesta = 0;
            using SqlConnection sql = new SqlConnection(_cadenaConexion);
            using SqlCommand cmd = new SqlCommand("updNegotiatedQualityAnalisys", sql);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            await sql.OpenAsync();
            SqlTransaction sqlTransaccion = sql.BeginTransaction();
            cmd.Transaction = sqlTransaccion;
            try
            {
                foreach (LineasNotaPeso dato in lstDatos)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@rowNumber", SqlDbType.BigInt);
                    cmd.Parameters["@rowNumber"].Value = dato.rowNumber;
                    cmd.Parameters.Add("@lineNumber", SqlDbType.Int);
                    cmd.Parameters["@lineNumber"].Value = dato.lineaNotaPeso;
                    cmd.Parameters.Add("@startingNumber", SqlDbType.SmallInt);
                    cmd.Parameters["@startingNumber"].Value = dato.numeroPartida;
                    cmd.Parameters.Add("@negotiationStatus", SqlDbType.Char, 1);
                    cmd.Parameters["@negotiationStatus"].Value = dato.definitive;

                    respuesta = await cmd.ExecuteNonQueryAsync();
                }
                sqlTransaccion.Commit();
                return respuesta;
            }
            catch (SqlException ex)
            {
                try
                {
                    sqlTransaccion.Rollback();
                    throw new Exception(ex.Message.ToString());
                }
                catch (Exception ex2)
                {
                    throw new Exception(ex2.Message.ToString());
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                sql.Close();
            }
        }

    }
}

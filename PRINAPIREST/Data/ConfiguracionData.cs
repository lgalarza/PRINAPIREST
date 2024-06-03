using Microsoft.Extensions.Configuration;
using PRINAPIREST.Dto;
using PRINAPIREST.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PRINAPIREST.Data
{
    public class ConfiguracionData
    {
        private readonly string _cadenaConexion;
        public ConfiguracionData(IConfiguration configuracion)
        {
            _cadenaConexion = configuracion.GetConnectionString("defaultConnection");
        }
        public async Task<List<DetalleCatalogo>> obtenerCatalogoxNombre(CatalogoDTO catalogo)
        {
            try
            {
                using SqlConnection sql = new SqlConnection(_cadenaConexion);
                using SqlCommand cmd = new SqlCommand("dbo.obtenerCatalogoxNombre", sql);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@nombreCatalogo", SqlDbType.VarChar, 250);
                cmd.Parameters["@nombreCatalogo"].Value = catalogo.nombreCatalogo;
                
                var response = new List<DetalleCatalogo>();
                await sql.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    #region Setear Valores
                    DetalleCatalogo item = new DetalleCatalogo();
                    item.codigoDetalleCatalogo = reader["codigoCatalogo"].ToString();
                    item.descripcionDetalleCatalogo = reader["nombreCatalogo"].ToString();
                    #endregion
                    response.Add(item);
                }
                return response;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
        }

        #region Tipo Cacao
        public async Task<RespuestaDTO> mantenimientoObtenerTipoCacao(TipoCacaoDTO dato)
        {
            try
            {
                using SqlConnection sql = new SqlConnection(_cadenaConexion);
                using SqlCommand cmd = new SqlCommand("dbo.mantenimientoTipoCacao", sql);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@opcion", SqlDbType.Char, 2);
                cmd.Parameters["@opcion"].Value = "CO";
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
                return new RespuestaDTO(-1, e.Message, "");
            }
        }
        public async Task<RespuestaDTO> mantenimientoGrabarTipoCacao(TipoCacaoDTO dato)
        {
            int respuesta = 0;
            using SqlConnection sql = new SqlConnection(_cadenaConexion);
            using SqlCommand cmd = new SqlCommand("dbo.mantenimientoTipoCacao", sql);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            await sql.OpenAsync();
            SqlTransaction sqlTransaccion = sql.BeginTransaction();
            cmd.Transaction = sqlTransaccion;
            try
            {
                string opcion = string.Empty;

                if (dato.codigoTipoCacao == 0)
                    opcion = "IN";
                else
                    opcion = "AC";

                cmd.Parameters.Add("@codigoTipoCacao", SqlDbType.SmallInt);
                cmd.Parameters["@codigoTipoCacao"].Value = dato.codigoTipoCacao;
                cmd.Parameters.Add("@nombreTipoCacao", SqlDbType.VarChar, 255);
                cmd.Parameters["@nombreTipoCacao"].Value = dato.nombreTipoCacao;
                cmd.Parameters.Add("@estadoTipoCacao", SqlDbType.Bit);
                cmd.Parameters["@estadoTipoCacao"].Value = dato.estadoTipoCacao;
                cmd.Parameters.Add("@opcion", SqlDbType.Char, 2);
                cmd.Parameters["@opcion"].Value = opcion;
                cmd.Parameters.Add("@co_msg", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@ds_msg", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output;
                respuesta = await cmd.ExecuteNonQueryAsync();
                sqlTransaccion.Commit();

                return new RespuestaDTO(
                    Convert.ToInt32(cmd.Parameters["@co_msg"].Value),
                    Convert.ToString(cmd.Parameters["@ds_msg"].Value),
                    ""
                    );

            }
            catch (SqlException ex)
            {
                try
                {
                    sqlTransaccion.Rollback();
                    return new RespuestaDTO(ex.ErrorCode, ex.Message, "");
                }
                catch (Exception ex2)
                {
                    return new RespuestaDTO(ex.ErrorCode, ex2.Message, "");
                }
            }
            catch (Exception e)
            {
                return new RespuestaDTO(-1, e.Message, "");
            }
            finally
            {
                sql.Close();
            }


        }
        public async Task<RespuestaDTO> obtenerTipoCacao()
        {
            try
            {
                using SqlConnection sql = new SqlConnection(_cadenaConexion);
                using SqlCommand cmd = new SqlCommand("dbo.obtenerTipoCacao", sql);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
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
                return new RespuestaDTO(-1, e.Message, "");
            }
        }
        #endregion

        #region Grupos
        public async Task<RespuestaDTO> mantenimientoObtenerGrupo(GruposDTO dato)
        {
            try
            {
                using SqlConnection sql = new SqlConnection(_cadenaConexion);
                using SqlCommand cmd = new SqlCommand("dbo.mantenimientoGrupos", sql);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@opcion", SqlDbType.Char, 2);
                cmd.Parameters["@opcion"].Value = "CO";
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
                return new RespuestaDTO(-1, e.Message, "");
            }
        }
        public async Task<RespuestaDTO> mantenimientoGrabarGrupo(GruposDTO dato)
        {
            int respuesta = 0;
            using SqlConnection sql = new SqlConnection(_cadenaConexion);
            using SqlCommand cmd = new SqlCommand("dbo.mantenimientoGrupos", sql);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            await sql.OpenAsync();
            SqlTransaction sqlTransaccion = sql.BeginTransaction();
            cmd.Transaction = sqlTransaccion;
            try
            {
                string opcion = string.Empty;

                if (dato.codigoGrupo == 0)
                    opcion = "IN";
                else
                    opcion = "AC";

                cmd.Parameters.Add("@codigoGrupo", SqlDbType.SmallInt);
                cmd.Parameters["@codigoGrupo"].Value = dato.codigoGrupo;
                cmd.Parameters.Add("@nombreGrupo", SqlDbType.VarChar, 255);
                cmd.Parameters["@nombreGrupo"].Value = dato.nombreGrupo;
                cmd.Parameters.Add("@codigoZona", SqlDbType.SmallInt);
                cmd.Parameters["@codigoZona"].Value = dato.codigoZona;
                cmd.Parameters.Add("@estado", SqlDbType.Bit);
                cmd.Parameters["@estado"].Value = dato.estadoGrupo;
                cmd.Parameters.Add("@opcion", SqlDbType.Char, 2);
                cmd.Parameters["@opcion"].Value = opcion;
                cmd.Parameters.Add("@co_msg", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@ds_msg", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output;
                respuesta = await cmd.ExecuteNonQueryAsync();
                sqlTransaccion.Commit();

                return new RespuestaDTO(
                    Convert.ToInt32(cmd.Parameters["@co_msg"].Value),
                    Convert.ToString(cmd.Parameters["@ds_msg"].Value),
                    ""
                    );

            }
            catch (SqlException ex)
            {
                try
                {
                    sqlTransaccion.Rollback();
                    return new RespuestaDTO(ex.ErrorCode, ex.Message, "");
                }
                catch (Exception ex2)
                {
                    return new RespuestaDTO(ex.ErrorCode, ex2.Message, "");
                }
            }
            catch (Exception e)
            {
                return new RespuestaDTO(-1, e.Message, "");
            }
            finally
            {
                sql.Close();
            }


        }
        public async Task<RespuestaDTO> obtenerGrupo()
        {
            try
            {
                using SqlConnection sql = new SqlConnection(_cadenaConexion);
                using SqlCommand cmd = new SqlCommand("dbo.obtenerGrupos", sql);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
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
                return new RespuestaDTO(-1, e.Message, "");
            }
        }
        #endregion

        #region Certificaciones


        #endregion

    }
}

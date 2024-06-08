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
                cmd.Parameters.Add("@nombreTipoCacao", SqlDbType.VarChar, 250);
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
                cmd.Parameters.Add("@nombreGrupo", SqlDbType.VarChar, 250);
                cmd.Parameters["@nombreGrupo"].Value = dato.nombreGrupo;
                cmd.Parameters.Add("@codigoZona", SqlDbType.VarChar,10);
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
        public async Task<RespuestaDTO> mantenimientoObtenerCertificacion(CertificacionesDTO dato)
        {
            try
            {
                using SqlConnection sql = new SqlConnection(_cadenaConexion);
                using SqlCommand cmd = new SqlCommand("dbo.mantenimientoCertificaciones", sql);
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
        public async Task<RespuestaDTO> mantenimientoGrabarCertificacion(CertificacionesDTO dato)
        {
            int respuesta = 0;
            using SqlConnection sql = new SqlConnection(_cadenaConexion);
            using SqlCommand cmd = new SqlCommand("dbo.mantenimientoCertificaciones", sql);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            await sql.OpenAsync();
            SqlTransaction sqlTransaccion = sql.BeginTransaction();
            cmd.Transaction = sqlTransaccion;
            try
            {
                string opcion = string.Empty;

                if (dato.codigoCertificacion == 0)
                    opcion = "IN";
                else
                    opcion = "AC";

                cmd.Parameters.Add("@codigoCertificacion", SqlDbType.SmallInt);
                cmd.Parameters["@codigoCertificacion"].Value = dato.codigoCertificacion;
                cmd.Parameters.Add("@codigoCertificacionAlfanumerico", SqlDbType.SmallInt);
                cmd.Parameters["@codigoCertificacionAlfanumerico"].Value = dato.codigoCertificacionAlfanumerico;
                cmd.Parameters.Add("@nombreCertificacion", SqlDbType.VarChar, 250);
                cmd.Parameters["@nombreCertificacion"].Value = dato.nombreCertificacion;
                cmd.Parameters.Add("@fechaInicial", SqlDbType.VarChar,10);
                cmd.Parameters["@fechaInicial"].Value = dato.fechaInicial;
                cmd.Parameters.Add("@fechaFinal", SqlDbType.VarChar, 10);
                cmd.Parameters["@fechaFinal"].Value = dato.fechaFinal;
                cmd.Parameters.Add("@toleranciaCuota", SqlDbType.Decimal);
                cmd.Parameters["@toleranciaCuota"].Value = dato.toleranciaCuota;
                cmd.Parameters.Add("@estado", SqlDbType.Bit);
                cmd.Parameters["@estado"].Value = dato.estadoCertificacion;
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
        public async Task<RespuestaDTO> obtenerCertificacion()
        {
            try
            {
                using SqlConnection sql = new SqlConnection(_cadenaConexion);
                using SqlCommand cmd = new SqlCommand("dbo.obtenerCertificaciones", sql);
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

        #region Programas
        public async Task<RespuestaDTO> mantenimientoObtenerPrograma(ProgramasDTO dato)
        {
            try
            {
                using SqlConnection sql = new SqlConnection(_cadenaConexion);
                using SqlCommand cmd = new SqlCommand("dbo.mantenimientoProgramas", sql);
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
        public async Task<RespuestaDTO> mantenimientoGrabarPrograma(ProgramasDTO dato)
        {
            int respuesta = 0;
            using SqlConnection sql = new SqlConnection(_cadenaConexion);
            using SqlCommand cmd = new SqlCommand("dbo.mantenimientoProgramas", sql);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            await sql.OpenAsync();
            SqlTransaction sqlTransaccion = sql.BeginTransaction();
            cmd.Transaction = sqlTransaccion;
            try
            {
                string opcion = string.Empty;

                if (dato.codigoPrograma == 0)
                    opcion = "IN";
                else
                    opcion = "AC";

                cmd.Parameters.Add("@codigoPrograma", SqlDbType.SmallInt);
                cmd.Parameters["@codigoPrograma"].Value = dato.codigoPrograma;
                cmd.Parameters.Add("@codigoProgramaAlfanumerico", SqlDbType.SmallInt);
                cmd.Parameters["@codigoProgramaAlfanumerico"].Value = dato.codigoProgramaAlfanumerico;
                cmd.Parameters.Add("@nombrePrograma", SqlDbType.VarChar, 250);
                cmd.Parameters["@nombrePrograma"].Value = dato.nombrePrograma;
                cmd.Parameters.Add("@codigoCertificacion", SqlDbType.SmallInt);
                cmd.Parameters["@codigoCertificacion"].Value = dato.codigoCertificacion;
                cmd.Parameters.Add("@estado", SqlDbType.Bit);
                cmd.Parameters["@estado"].Value = dato.estadoPrograma;
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
        public async Task<RespuestaDTO> obtenerPrograma()
        {
            try
            {
                using SqlConnection sql = new SqlConnection(_cadenaConexion);
                using SqlCommand cmd = new SqlCommand("dbo.obtenerProgramas", sql);
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

        #region Grupo Vendor Factura
        public async Task<RespuestaDTO> mantenimientoObtenerGrupoVendorFactura(GrupoVendorFacturaDTO dato)
        {
            try
            {
                using SqlConnection sql = new SqlConnection(_cadenaConexion);
                using SqlCommand cmd = new SqlCommand("dbo.mantenimientoGrupoVendorFactura", sql);
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
        public async Task<RespuestaDTO> mantenimientoGrabarVendorFactura(GrupoVendorFacturaDTO dato)
        {
            int respuesta = 0;
            using SqlConnection sql = new SqlConnection(_cadenaConexion);
            using SqlCommand cmd = new SqlCommand("dbo.mantenimientoGrupoVendorFactura", sql);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            await sql.OpenAsync();
            SqlTransaction sqlTransaccion = sql.BeginTransaction();
            cmd.Transaction = sqlTransaccion;
            try
            {
                string opcion = string.Empty;

                if (dato.codigoGrupoVendor == 0)
                    opcion = "IN";
                else
                    opcion = "AC";

                cmd.Parameters.Add("@codigoGrupoVendor", SqlDbType.SmallInt);
                cmd.Parameters["@codigoGrupoVendor"].Value = dato.codigoGrupoVendor;
                cmd.Parameters.Add("@codigoVendorPrincipal", SqlDbType.VarChar,20);
                cmd.Parameters["@codigoVendorPrincipal"].Value = dato.codigoGrupoVendor;
                cmd.Parameters.Add("@nombreVendorPrincipal", SqlDbType.VarChar, 250);
                cmd.Parameters["@nombreVendorPrincipal"].Value = dato.nombreVendorPrincipal;
                cmd.Parameters.Add("@codigoVendorFactura", SqlDbType.VarChar, 20);
                cmd.Parameters["@codigoVendorFactura"].Value = dato.codigoGrupoVendor;
                cmd.Parameters.Add("@nombreVendorFactura", SqlDbType.VarChar, 250);
                cmd.Parameters["@nombreVendorFactura"].Value = dato.nombreVendorPrincipal;
                cmd.Parameters.Add("@codigoPrograma", SqlDbType.SmallInt);
                cmd.Parameters["@codigoPrograma"].Value = dato.codigoPrograma;
                cmd.Parameters.Add("@cupoCompra", SqlDbType.Decimal);
                cmd.Parameters["@cupoCompra"].Value = dato.cupoCompra;
                cmd.Parameters.Add("@excedente", SqlDbType.Decimal);
                cmd.Parameters["@excedente"].Value = dato.excedente;
                cmd.Parameters.Add("@estado", SqlDbType.Bit);
                cmd.Parameters["@estado"].Value = dato.estadoGrupoVendor;
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
        #endregion

        #region Pronóstico Cosecha
        public async Task<RespuestaDTO> mantenimientoObtenerPronosticoCosecha(PronosticoCosechaDTO dato)
        {
            try
            {
                using SqlConnection sql = new SqlConnection(_cadenaConexion);
                using SqlCommand cmd = new SqlCommand("dbo.mantenimientoPronosticoCosecha", sql);
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
        public async Task<RespuestaDTO> mantenimientoGrabarPronosticoCosecha(PronosticoCosechaDTO dato)
        {
            int respuesta = 0;
            using SqlConnection sql = new SqlConnection(_cadenaConexion);
            using SqlCommand cmd = new SqlCommand("dbo.mantenimientoPronosticoCosecha", sql);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            await sql.OpenAsync();
            SqlTransaction sqlTransaccion = sql.BeginTransaction();
            cmd.Transaction = sqlTransaccion;
            try
            {
                string opcion = string.Empty;

                if (dato.codigoPronosticoCosecha == 0)
                    opcion = "IN";
                else
                    opcion = "AC";

                cmd.Parameters.Add("@codigoPronosticoCosecha", SqlDbType.SmallInt);
                cmd.Parameters["@codigoPronosticoCosecha"].Value = dato.codigoPronosticoCosecha;
                cmd.Parameters.Add("@codigoZona", SqlDbType.SmallInt);
                cmd.Parameters["@codigoZona"].Value = dato.codigoZona;
                cmd.Parameters.Add("@enero", SqlDbType.Decimal);
                cmd.Parameters["@enero"].Value = dato.enero;
                cmd.Parameters.Add("@febrero", SqlDbType.Decimal);
                cmd.Parameters["@febrero"].Value = dato.febrero;
                cmd.Parameters.Add("@marzo", SqlDbType.Decimal);
                cmd.Parameters["@marzo"].Value = dato.marzo;
                cmd.Parameters.Add("@abril", SqlDbType.Decimal);
                cmd.Parameters["@abril"].Value = dato.abril;
                cmd.Parameters.Add("@mayo", SqlDbType.Decimal);
                cmd.Parameters["@mayo"].Value = dato.mayo;
                cmd.Parameters.Add("@junio", SqlDbType.Decimal);
                cmd.Parameters["@junio"].Value = dato.junio;
                cmd.Parameters.Add("@julio", SqlDbType.Decimal);
                cmd.Parameters["@julio"].Value = dato.julio;
                cmd.Parameters.Add("@agosto", SqlDbType.Decimal);
                cmd.Parameters["@agosto"].Value = dato.agosto;
                cmd.Parameters.Add("@septiembre", SqlDbType.Decimal);
                cmd.Parameters["@septiembre"].Value = dato.septiembre;
                cmd.Parameters.Add("@octubre", SqlDbType.Decimal);
                cmd.Parameters["@octubre"].Value = dato.octubre;
                cmd.Parameters.Add("@noviembre", SqlDbType.Decimal);
                cmd.Parameters["@noviembre"].Value = dato.noviembre;
                cmd.Parameters.Add("@diciembre", SqlDbType.Decimal);
                cmd.Parameters["@diciembre"].Value = dato.diciembre;
                cmd.Parameters.Add("@estado", SqlDbType.Bit);
                cmd.Parameters["@estado"].Value = dato.estadoPronosticoCosecha;
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
        #endregion
    }
}

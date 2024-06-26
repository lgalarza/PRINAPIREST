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
    public class SeguridadData
    {
        private readonly string _cadenaConexion;
        public SeguridadData(IConfiguration configuracion)
        {
            _cadenaConexion = configuracion.GetConnectionString("defaultConnection");
        }

        public async Task<RespuestaDTO> mantenimientoObtenerPerfil(PerfilDTO perfil)
        {
            try
            {
                using SqlConnection sql = new SqlConnection(_cadenaConexion);
                using SqlCommand cmd = new SqlCommand("dbo.mantenimientoPerfil", sql);
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

        public async Task<RespuestaDTO> mantenimientoGrabarPerfil(PerfilDTO perfil)
        {
            int respuesta = 0;
            using SqlConnection sql = new SqlConnection(_cadenaConexion);
            using SqlCommand cmd = new SqlCommand("dbo.mantenimientoPerfil", sql);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            await sql.OpenAsync();
            SqlTransaction sqlTransaccion = sql.BeginTransaction();
            cmd.Transaction = sqlTransaccion;
            try
            {
                string opcion = string.Empty;

                if (perfil.codigoPerfil == 0)
                    opcion = "IN";
                else
                    opcion = "AC";

                cmd.Parameters.Add("@codigoPerfil", SqlDbType.SmallInt);
                cmd.Parameters["@codigoPerfil"].Value = perfil.codigoPerfil;
                cmd.Parameters.Add("@nombrePerfil", SqlDbType.VarChar, 50);
                cmd.Parameters["@nombrePerfil"].Value = perfil.nombrePerfil;
                cmd.Parameters.Add("@estadoPerfil", SqlDbType.Bit);
                cmd.Parameters["@estadoPerfil"].Value = perfil.estadoPerfil;
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

        public async Task<RespuestaDTO> obtenerPerfiles()
        {
            try
            {
                using SqlConnection sql = new SqlConnection(_cadenaConexion);
                using SqlCommand cmd = new SqlCommand("dbo.obtenerPerfiles", sql);
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

    }
}

using Microsoft.Extensions.Configuration;
using PRINAPIREST.Dto;
using PRINAPIREST.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

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

    }
}

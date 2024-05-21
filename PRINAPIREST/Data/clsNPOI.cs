using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PRINAPIREST.Data
{
    public class clsNPOI
    {
        public static HSSFWorkbook? DataTableToExcel(DataTable dt)
        {
            HSSFWorkbook? workbook = null;
            IRow row;
            ISheet sheet;
            ICell cell;
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    workbook = new HSSFWorkbook();
                    sheet = workbook.CreateSheet("Hoja1");//Crear una tabla llamada Hoja1 
                    int rowCount = dt.Rows.Count;//cantidad de filas
                    int columnCount = dt.Columns.Count;//cantidad de columnas

                    //Establecer encabezado de columna
                    row = sheet.CreateRow(0);//Primera fila como encabezado de columna
                    for (int c = 0; c < columnCount; c++)
                    {
                        cell = row.CreateCell(c);
                        cell.SetCellValue(dt.Columns[c].ColumnName);
                    }

                    //Establecer las celdas para cada fila y columna
                    for (int i = 0; i < rowCount; i++)
                    {
                        row = sheet.CreateRow(i + 1);
                        for (int j = 0; j < columnCount; j++)
                        {
                            cell = row.CreateCell(j);//La segunda fila de Excel comienza a escribir datos. 
                            cell.SetCellValue(dt.Rows[i][j].ToString());

                        }
                    }

                }

                return workbook;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

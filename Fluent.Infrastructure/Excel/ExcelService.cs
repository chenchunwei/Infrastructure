using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Aspose.Cells;

namespace Fluent.Infrastructure.Excel
{
    public class ExcelService
    {
        public void ConvertToExcel(DataTable dataTable, string filePath)
        {
            var workbook = new Workbook();
            if (dataTable == null)
                throw new ArgumentNullException("dataTable");
            //为单元格添加样式    
            Style style = workbook.Styles[workbook.Styles.Add()];
            style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
            style.ForegroundColor = System.Drawing.Color.FromArgb(213, 213, 213);
            style.Pattern = BackgroundType.Solid;
            style.Font.IsBold = true;
            int rowIndex = 0;
            //设置标题
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                DataColumn col = dataTable.Columns[i];
                string columnName = col.Caption ?? col.ColumnName;
                workbook.Worksheets[0].Cells[rowIndex, i].PutValue(columnName);
                workbook.Worksheets[0].Cells[rowIndex, i].SetStyle(style);
            }
            rowIndex++;
            //设置内容行
            foreach (DataRow row in dataTable.Rows)
            {
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    workbook.Worksheets[0].Cells[rowIndex, i].PutValue(row[i].ToString());
                }
                rowIndex++;
            }
            //定义列格式
            for (int k = 0; k < dataTable.Columns.Count; k++)
            {
                workbook.Worksheets[0].AutoFitColumn(k, 0, 150);
            }
            //冻结标题行
            workbook.Worksheets[0].FreezePanes(1, 0, 1, dataTable.Columns.Count);
            workbook.Save(filePath);
        }
    }
}

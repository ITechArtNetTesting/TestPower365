using Microsoft.Office.Interop.Excel;
using System;


namespace Product.Framework
{
    public class ExcelReader
    {
        private Application excelApp;
        private Workbook excelWorkbook;
        private Worksheet excelWorksheet;
        private Range excelRange;
        private int rowCount;
        private int colCount;

        public ExcelReader(string docPath)
        {           
            excelApp = new Application();
            excelWorkbook = excelApp.Workbooks.Open(docPath);
            excelWorksheet = excelWorkbook.ActiveSheet;
            excelRange = excelWorksheet.UsedRange;
            rowCount = excelRange.Rows.Count;
            colCount = excelRange.Columns.Count;
        }

        public ExcelReader(string docPath, int sheet)
        {
            excelApp = new Application();
            excelWorkbook = excelApp.Workbooks.Open(docPath);
            excelWorksheet = excelWorkbook.Sheets[sheet];
            excelRange = excelWorksheet.UsedRange;
            rowCount = excelRange.Rows.Count;
            colCount = excelRange.Columns.Count;
        }

        public bool IsMessageExist(string message)
        {
            bool result = false;            
            for (int row = 1; row <= rowCount; row++)
            {
                for (int col = 1; col <= colCount; col++)
                {
                    if (((excelRange.Cells[row, col] as Range).Value2 as String).Contains(message))
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

    }
}

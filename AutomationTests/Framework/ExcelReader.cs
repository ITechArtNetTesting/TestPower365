using Microsoft.Office.Interop.Excel;
using System;
using System.Runtime.InteropServices;

namespace Product.Framework
{
    public class ExcelReader
    {
        private string FilePath;
        private int NumberOfStartSheet;
        private Application excelApp;
        private Workbook excelWorkbook;
        private Worksheet excelWorksheet;
        private Range excelRange;
        private int rowCount;
        private int colCount;

        public ExcelReader(string docPath)
        {
            FilePath = docPath;           
        }

        public ExcelReader(string docPath, int sheet)
        {
            FilePath = docPath;
            NumberOfStartSheet = sheet;           
        }

        void SetUpReader()
        {
            excelApp = new Application();
            excelWorkbook = excelApp.Workbooks.Open(FilePath);
            if (NumberOfStartSheet == 0)
            {
                excelWorksheet = excelWorkbook.ActiveSheet;
            }
            else
            {
                excelWorksheet = excelWorkbook.Sheets[NumberOfStartSheet];
            }
            excelRange = excelWorksheet.UsedRange;
            rowCount = excelRange.Rows.Count;
            colCount = excelRange.Columns.Count;
        }

        void CleanUpReader()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Marshal.ReleaseComObject(excelRange);
            Marshal.ReleaseComObject(excelWorksheet);
            excelWorkbook.Close();
            Marshal.ReleaseComObject(excelWorkbook);
            excelApp.Quit();
            Marshal.ReleaseComObject(excelApp);
        }

        public bool IsTextExistDoc(string message)
        {
            SetUpReader();
            bool result = false;            
            for (int row = 1; row <= rowCount; row++)
            {
                for (int col = 1; col <= colCount; col++)
                {
                    if (((excelRange.Cells[row, col] as Range).Value2 as String)!=null&&((excelRange.Cells[row, col] as Range).Value2 as String).Contains(message))
                    {
                        var watch = ((excelRange.Cells[row, col] as Range).Value2 as String);
                        result = true;
                    }
                }
            }
            CleanUpReader();
            return result;
        }

    }
}

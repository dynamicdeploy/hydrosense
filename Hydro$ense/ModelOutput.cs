using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;

namespace HydroSense
{
    class ModelOutput
    {
        private double[][] m_Q;

        public ModelOutput(ModelInput mi)
        {
            m_Q = Util.CopyArray(mi.Q);
        }

        internal void ToExcel(string fileName)
        {
            IWorkbook wkbk = GetWorkbook(fileName);
            ISheet quantity = GetOrCreateSheet(wkbk, "Quantity Delivered");

            for (int i = 0; i < m_Q.Length; i++)
            {
                IRow row = quantity.CreateRow(i);
                for (int j = 0; j < m_Q[i].Length; j++)
                {
                    row.CreateCell(j).SetCellValue(m_Q[i][j]);
                }
            }

            FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
            wkbk.Write(fs);
            fs.Close();
        }

        private IWorkbook GetWorkbook(string fileName)
        {
            IWorkbook workbook;
            if (File.Exists(fileName))
            {
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite);
                workbook = WorkbookFactory.Create(fs);
                fs.Close();
            }
            else
            {
                if (fileName.EndsWith("xls"))
                    workbook = new HSSFWorkbook();
                else
                    workbook = new XSSFWorkbook();
            }
            return workbook;
        }

        private ISheet GetOrCreateSheet(IWorkbook workbook, string sheetname)
        {
            ISheet rval = workbook.GetSheet(sheetname);
            if (rval == null)
            {
                rval = workbook.CreateSheet(sheetname);
            }
            else
            {
                int idx = 0;
                for (int i = 0; i < workbook.NumberOfSheets; i++)
                {
                    ISheet sheet = workbook.GetSheetAt(i);
                    if (sheet.SheetName == rval.SheetName)
                    {
                        idx = i;
                        break;
                    }
                }
                workbook.RemoveSheetAt(idx);
                rval = workbook.CreateSheet(sheetname);
            }
            return rval;
        }
    }
}

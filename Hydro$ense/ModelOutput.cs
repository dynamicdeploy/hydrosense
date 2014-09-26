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
        private double[][] m_quantS;
        private double[][] m_quantD;
        private double m_netBenefit;

        public ModelOutput(double[][] supplyQuantity, double[][] demandQuantity, double netBenefit)
        {
            m_quantS = supplyQuantity;
            m_quantD = demandQuantity;
            m_netBenefit = netBenefit;
        }

        internal void ToExcel(string fileName)
        {
            IWorkbook wkbk = GetWorkbook(fileName);
            WriteToSheet(wkbk, "Maximum Net Benefit", m_netBenefit);
            WriteToSheet(wkbk, "Optimal Supply", m_quantS);
            WriteToSheet(wkbk, "Optimal Delivery", m_quantD);

            FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
            wkbk.Write(fs);
            fs.Close();
        }

        private void WriteToSheet(IWorkbook wkbk, string sheetname, double netBenefit)
        {
            ISheet sheet = GetOrCreateSheet(wkbk, sheetname);
            IRow row;
            row = sheet.CreateRow(0);
            row.CreateCell(0).SetCellValue("Total benefits of water use minus the total cost of water supply and delivery");
            row = sheet.CreateRow(1);
            row.CreateCell(0).SetCellValue(netBenefit);
        }

        private void WriteToSheet(IWorkbook wkbk, string sheetname, double[][] quantities)
        {
            ISheet sheet = GetOrCreateSheet(wkbk, sheetname);

            for (int i = 0; i < quantities.Length; i++)
            {
                IRow row = sheet.CreateRow(i);
                for (int j = 0; j < quantities[i].Length; j++)
                {
                    row.CreateCell(j).SetCellValue(quantities[i][j]);
                }
            }
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

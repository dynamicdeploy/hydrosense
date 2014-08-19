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
            
            IWorkbook wkbk;
            if (fileName.EndsWith("xls"))
                wkbk = new HSSFWorkbook();
            else
                wkbk = new XSSFWorkbook();

            ISheet quantity = wkbk.CreateSheet("Quantity Delivered");
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
    }
}

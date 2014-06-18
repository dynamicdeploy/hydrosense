using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;

namespace HydroSense
{
    class ModelInput
    {
        public Nodes supplyNodes { get; set; }
        public Nodes demandNodes { get; set; }
        public LinkCosts linkCosts { get; set; }
        public LinkLosses linkLosses { get; set; }
        public double[][] Q { get; set; }
        
        public ModelInput()
        {
        }

        public void ReadFromExcel(string fileName)
        {
            HSSFWorkbook wkbk = new HSSFWorkbook(File.OpenRead(fileName));

            ISheet supply = wkbk.GetSheet("supply curves");
            ISheet demand = wkbk.GetSheet("demand curves");
            ISheet linkcost = wkbk.GetSheet("transportation costs");
            ISheet linkloss = wkbk.GetSheet("transportation losses");
            ISheet guess = wkbk.GetSheet("initial guess");

            ReadNodesDataFromSheet(supply, supplyNodes);
            ReadNodesDataFromSheet(demand, demandNodes);
            ReadLinksDataFromSheet(linkcost, linkCosts);
            ReadLinksDataFromSheet(linkloss, linkLosses);
            ReadQuantityDataFromSheet(guess, Q);
        }

        private void ReadQuantityDataFromSheet(ISheet guess, double[][] Q)
        {
        }

        private void ReadLinksDataFromSheet(ISheet linkcost, Links linkPoints)
        {
        }

        private void ReadNodesDataFromSheet(ISheet sheet, Nodes nodePoints)
        {
            int rowCount = sheet.PhysicalNumberOfRows;
            double[][] x = new double[rowCount][];
            double[][] y = new double[rowCount][];

            for (int i = 0; i < rowCount; i+=2)
            {
                int colCountx = sheet.GetRow(i).LastCellNum;
                int colCounty = sheet.GetRow(i + 1).LastCellNum;

                if (colCountx != colCounty)
                {
                    throw new DataMisalignedException("must have the same number of quantity/cost points for each node, check inputs");
                }

                x[i] = new double[colCountx];
                y[i] = new double[colCountx];

                IRow rowx = sheet.GetRow(i);
                IRow rowy = sheet.GetRow(i + 1);
                for (int j = 0; j < colCountx; j++)
                {
                    x[i][j] = rowx.GetCell(j).NumericCellValue;
                    y[i][j] = rowy.GetCell(j).NumericCellValue;
                }
            }
            nodePoints = new Nodes(x, y);
        }

        public void ReadHardcoded()
        {
            double[][] Xs;
            double[][] Ys;
            double[][] Xd;
            double[][] Yd;
            double[][][] Xt;
            double[][][] Yt;
            double[][][] Xtl;
            double[][][] Ytl;

            // marginal supply curves
            Xs = new double[2][];
            Ys = new double[2][];
            Xs[0] = new double[] { 0.0, 2000.0, 4000.0, 5000.0, 7500.0, 10000.0, 20000.0, 30000.0, 50000.0, 65000.0 };
            Xs[1] = new double[] { 0.0, 1000.0, 2500.0, 5000.0, 8000.0, 10000.0, 15000.0, 25000.0 };
            Ys[0] = new double[] { 3.0, 3.061, 3.122, 3.154, 3.234, 3.316, 3.664, 4.050, 4.946, 5.747 };
            Ys[1] = new double[] { 1.50, 1.546, 1.617, 1.743, 1.907, 2.025, 2.352, 3.176 };
            supplyNodes = new Nodes(Xs, Ys);

            // marginal demand curves
            Xd = new double[3][];
            Yd = new double[3][];
            Xd[0] = new double[] { 0.0, 2000.0, 4000.0, 7500.0, 10000.0, 15000.0, 20000.0, 30000.0, 40000.0 };
            Xd[1] = new double[] { 0.0, 3000.0, 7500.0, 12500.0, 20000.0, 30000.0 };
            Xd[2] = new double[] { 0.0, 2500.0, 5000.0, 10000.0, 15000.0, 25000.0, 35000.0 };
            Yd[0] = new double[] { 15.0, 12.782, 10.892, 8.232, 6.740, 4.518, 3.028, 1.361, 0.611 };
            Yd[1] = new double[] { 10.0, 8.607, 6.873, 5.353, 3.679, 2.231 };
            Yd[2] = new double[] { 25.0, 19.470, 15.163, 9.197, 5.578, 2.052, 0.755 };
            demandNodes = new Nodes(Xd, Yd);

            // transportation costs
            Xt = new double[3][][];
            Xt[0] = new double[2][];
            Xt[1] = new double[2][];
            Xt[2] = new double[2][];
            Xt[0][0] = new double[] { 0.0, 40000.0 };
            Xt[0][1] = new double[] { 0.0, 20000.0 };
            Xt[1][0] = new double[] { 0.0, 43000.0 };
            Xt[1][1] = new double[] { 0.0, 50000.0 };
            Xt[2][0] = new double[] { 0.0, 50000.0 };
            Xt[2][1] = new double[] { 0.0, 17500.0 };
            Yt = new double[3][][];
            Yt[0] = new double[2][];
            Yt[1] = new double[2][];
            Yt[2] = new double[2][];
            Yt[0][0] = new double[] { 1.5, 1.5 };
            Yt[0][1] = new double[] { 0.75, 0.75 };
            Yt[1][0] = new double[] { 1.25, 1.25 };
            Yt[1][1] = new double[] { 100.0, 100.0 };
            Yt[2][0] = new double[] { 100.0, 100.0 };
            Yt[2][1] = new double[] { 1.3, 1.3 };
            linkCosts = new LinkCosts(Xt, Yt);

            // transportation losses
            Xtl = new double[3][][];
            Xtl[0] = new double[2][];
            Xtl[1] = new double[2][];
            Xtl[2] = new double[2][];
            Xtl[0][0] = new double[] { 0.0, 5000.0, 10000.0, 15000.0, 20000.0, 30000.0, 40000.0 };
            Xtl[0][1] = new double[] { 0.0, 2500.0, 5000.0, 10000.0, 15000.0, 20000.0 };
            Xtl[1][0] = new double[] { 0.0, 5000.0, 10000.0, 17500.0, 25000.0, 35000.0, 43000.0 };
            Xtl[1][1] = new double[] { 0.0, 50000.0 };
            Xtl[2][0] = new double[] { 0.0, 50000.0 };
            Xtl[2][1] = new double[] { 0.0, 2500.0, 5000.0, 7500.0, 10000.0, 15000.0, 17500.0 };
            Ytl = new double[3][][];
            Ytl[0] = new double[2][];
            Ytl[1] = new double[2][];
            Ytl[2] = new double[2][];
            Ytl[0][0] = new double[] { 0.0, 2661.0, 8147.0, 13753.0, 19134.0, 29426.0, 39487.0 };
            Ytl[0][1] = new double[] { 0.0, 0.0, 1390.0, 6275.0, 11866.0, 17515.0 };
            Ytl[1][0] = new double[] { 0.0, 1467.0, 5821.0, 13959.0, 22448.0, 33443.0, 41917.0 };
            Ytl[1][1] = new double[] { 0.0, 0.0 };
            Ytl[2][0] = new double[] { 0.0, 0.0 };
            Ytl[2][1] = new double[] { 0.0, 303.0, 1717.0, 3707.0, 6071.0, 11403.0, 14209.0 };
            linkLosses = new LinkLosses(Xtl, Ytl);
            
            // initial guess
            Q = new double[3][];
            Q[0] = new double[] { 10050.64038617956, 11312.969207627437 };
            Q[1] = new double[] { 22028.15601517901, 0.0 };
            Q[2] = new double[] { 0.0, 13684.530792372565 };

        }
    }
}

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
            if (!File.Exists(fileName))
                throw new FileNotFoundException("cannot find file: " + fileName);

            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);

            IWorkbook wkbk = WorkbookFactory.Create(fs);
            fs.Close();

            ISheet sheetSupply = wkbk.GetSheet("supply curves");
            ISheet sheetDemand = wkbk.GetSheet("demand curves");
            ISheet sheetLinkcost = wkbk.GetSheet("transportation costs");
            ISheet sheetLinkloss = wkbk.GetSheet("transportation losses");
            ISheet sheetGuess = wkbk.GetSheet("initial guess");

            if (sheetSupply == null)
                throw new InvalidDataException("Supply Curves worksheet not found, check spelling");
            if (sheetDemand == null)
                throw new InvalidDataException("Demand Curves worksheet not found, check spelling");
            if (sheetLinkcost == null)
                throw new InvalidDataException("Transportation Costs worksheet not found, check spelling");
            if (sheetLinkloss == null)
                throw new InvalidDataException("Transportation Losses worksheet not found, check spelling");
            if (sheetGuess == null)
                throw new InvalidDataException("Initial Guess worksheet not found, check spelling");

            Nodes sNodes = ReadNodesDataFromSheet(sheetSupply);
            supplyNodes = new Nodes(sNodes.x, sNodes.y);
            
            Nodes dNodes = ReadNodesDataFromSheet(sheetDemand);
            demandNodes = new Nodes(dNodes.x, dNodes.y);

            Links costs = ReadLinksDataFromSheet(sheetLinkcost);
            linkCosts = new LinkCosts(costs.x, costs.y);

            Links losses = ReadLinksDataFromSheet(sheetLinkloss);
            linkLosses = new LinkLosses(losses.x, losses.y);

            Q = ReadQuantityDataFromSheet(sheetGuess);
        }

        private double[][] ReadQuantityDataFromSheet(ISheet sheet)
        {
            int rowCount = GetRowCount(sheet);
            double[][] rval = new double[rowCount][];

            for (int i = 0; i < rowCount; i++)
            {
                IRow row = sheet.GetRow(i);

                int numPoints = GetPointCount(row);
                rval[i] = new double[numPoints];
                for (int j = 0; j < numPoints; j++)
                {
                    rval[i][j] = row.GetCell(j).NumericCellValue;
                }
            }

            CheckEqualNumberPoints(rval);
            return rval;
        }

        private Links ReadLinksDataFromSheet(ISheet sheet)
        {
            int rowCount = GetRowCount(sheet);
            int numDemands = Convert.ToInt32(sheet.GetRow(rowCount - 1).GetCell(0).NumericCellValue);
            
            int numSupplies = 0;
            while (Convert.ToInt32(sheet.GetRow(numSupplies).GetCell(0).NumericCellValue) == 1)
            {
                numSupplies++;
            }
            numSupplies /= 2;

            double[][][] x = new double[numDemands][][];
            double[][][] y = new double[numDemands][][];
            for (int i = 0; i < numDemands; i++)
            {
                x[i] = new double[numSupplies][];
                y[i] = new double[numSupplies][];
            }

            for (int i = 0; i < rowCount; i++)
            {
                IRow row = sheet.GetRow(i);
                int dem = Convert.ToInt32(sheet.GetRow(i).GetCell(0).NumericCellValue) - 1;
                int sup = Convert.ToInt32(sheet.GetRow(i).GetCell(1).NumericCellValue) - 1;

                int numPoints = GetPointCount(row) - 2;

                if (numPoints > 0)
                {
                    if (i % 2 == 0)
                    {
                        x[dem][sup] = new double[numPoints];
                        for (int k = 0; k < numPoints; k++)
                        {
                            x[dem][sup][k] = row.GetCell(k + 2).NumericCellValue;
                        }
                    }
                    else
                    {
                        y[dem][sup] = new double[numPoints];
                        for (int k = 0; k < numPoints; k++)
                        {
                            y[dem][sup][k] = row.GetCell(k + 2).NumericCellValue;
                        }
                    } 
                }
            }
            CheckEqualNumberPoints(x, y);
            return new Links(x, y);
        }

       

        private Nodes ReadNodesDataFromSheet(ISheet sheet)
        {
            int rowCount = GetRowCount(sheet);
            double[][] x = new double[rowCount / 2][];
            double[][] y = new double[rowCount / 2][];

            int xidx = 0;
            int yidx = 0;
            for (int i = 0; i < rowCount; i++)
            {
                IRow row = sheet.GetRow(i);

                int numPoints = GetPointCount(row);
                if (numPoints > 0)
                {
                    if (i % 2 == 0)
                    {
                        x[xidx] = new double[numPoints];
                        for (int j = 0; j < numPoints; j++)
                        {
                            x[xidx][j] = row.GetCell(j).NumericCellValue;
                        }
                        xidx++;
                    }
                    else
                    {
                        y[yidx] = new double[numPoints];
                        for (int j = 0; j < numPoints; j++)
                        {
                            y[yidx][j] = row.GetCell(j).NumericCellValue;
                        }
                        yidx++;
                    } 
                }
            }
            CheckEqualNumberPoints(x, y);
            return new Nodes(x, y);
        }

        private int GetRowCount(ISheet sheet)
        {
            int rval = 0;

            for (int i = 0; i < sheet.PhysicalNumberOfRows; i++)
            {
                try
                {
                    double val = sheet.GetRow(i).GetCell(0).NumericCellValue;
                    rval++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return rval;
        }

        private int GetPointCount(IRow row)
        {
            int rval = 0;

            for (int i = 0; i < row.PhysicalNumberOfCells; i++)
            {
                ICell cell = row.GetCell(i);
                if (cell.ToString() == "")
                {
                    continue;
                }

                try
                {
                    double val = cell.NumericCellValue;
                    rval++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return rval;
        }

        /// <summary>
        /// Check that initial guess is not a jagged array even though by definition it could be,
        /// I used a jagged array because they are supposed to be faster
        /// </summary>
        /// <param name="q"></param>
        private void CheckEqualNumberPoints(double[][] q)
        {
            int numPoints = q[0].Length;
            for (int i = 0; i < q.Length; i++)
            {
                if (q[i].Length != numPoints)
                {
                    throw new DataMisalignedException("must have an initial guess for every demand/supply, set it to zero if there is no relationship");
                }
            }
        }

        /// <summary>
        /// Check that the x,y jagged array has the same number of rows and points in each row
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void CheckEqualNumberPoints(double[][] x, double[][] y)
        {
            if (x.Length != y.Length)
            {
                throw new DataMisalignedException("must have a quantity/cost relationship for each node, check inputs");
            }

            for (int i = 0; i < x.Length; i++)
            {
                if (x[i].Length != y[i].Length)
                {
                    throw new DataMisalignedException("must have the same number of quantity/cost points for each node, check inputs");
                }
            }
        }

        /// <summary>
        /// Check that the x,y,z jagged array has the same number of rows/columns and points in each row
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void CheckEqualNumberPoints(double[][][] x, double[][][] y)
        {
            if (x.Length != y.Length)
            {
                throw new DataMisalignedException("must have the same number of quantity/cost nodes, check inputs");
            }

            for (int i = 0; i < x.Length; i++)
            {
                if (x[i].Length != y[i].Length)
                {
                    throw new DataMisalignedException("must have a quantity/cost relationship for each node, check inputs");
                }

                for (int j = 0; j < x[i].Length; j++)
                {
                    if (x[i][j].Length != y[i][j].Length)
                    {
                        throw new DataMisalignedException("must have the same number of quantity/cost points for each node, check inputs");
                    }
                }
            }
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
            Q[0] = new double[] { 10000, 10000 };
            Q[1] = new double[] { 10000, 0 };
            Q[2] = new double[] { 0, 10000 };

        }
    }
}
